using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Windows.Kinect;
using System;


public class KinectPointManAvatarModel : BasicAvatarModel
{
    // get data from kinect
    public BodySourceManager BodyManager;

    // initial rotation of each joint
    private Dictionary<JointType, Vector3> initialAvatarJointPositions = new Dictionary<JointType, Vector3>();

    // initial direction vector of each joint
    protected Dictionary<JointType, Vector3> initialAvatarJointDirections = new Dictionary<JointType, Vector3>();

    // transforms that correspond to the joints from kinect, used to get the initial kinect directions
    private Dictionary<JointType, Transform> jointTransforms = new Dictionary<JointType, Transform>();

    // currently tracked body
    protected Body currentBody = null;
    

    // dictionary where the connection of the joints is described
    // used to calculate the joint directions
    protected Dictionary<JointType, JointType> fromToJoints = new Dictionary<JointType, JointType>()
    {
{JointType.SpineBase , JointType.SpineMid },
{JointType.SpineMid , JointType.Neck },
{JointType.Neck , JointType.Head },
{JointType.Head , JointType.Neck }, // use inverse direction to rotate the head like the neck
{JointType.ShoulderLeft , JointType.ElbowLeft },
{JointType.ElbowLeft , JointType.WristLeft },
{JointType.WristLeft , JointType.HandLeft },
{JointType.HandLeft , JointType.HandTipLeft },
{JointType.ShoulderRight , JointType.ElbowRight },
{JointType.ElbowRight , JointType.WristRight },
{JointType.WristRight , JointType.HandRight },
{JointType.HandRight , JointType.HandTipRight },
{JointType.HipLeft , JointType.KneeLeft },
{JointType.KneeLeft , JointType.AnkleLeft },
{JointType.AnkleLeft , JointType.FootLeft },
//{JointType.FootLeft , JointType.AnkleLeft },// rotation of bone endings can't be computed from bone positions
{JointType.HipRight , JointType.AnkleRight },
{JointType.KneeRight , JointType.AnkleRight },
{JointType.AnkleRight , JointType.FootRight },
//{JointType.FootRight , JointType.AnkleRight },// rotation of bone endings can't be computed from bone positions 
{JointType.SpineShoulder , JointType.Neck },
//{JointType.HandTipLeft , JointType.HandLeft },// rotation of bone endings can't be computed from bone positions 
//{JointType.ThumbLeft , JointType.WristLeft },// rotation of bone endings can't be computed from bone positions 
//{JointType.HandTipRight , JointType.HandRight },// rotation of bone endings can't be computed from bone positions 
//{JointType.ThumbRight , JointType.WristRight }// rotation of bone endings can't be computed from bone positions 

    };

    public override Quaternion applyRelativeRotationChange(JointType jt, Quaternion initialModelJointRotation)
    {
        //missing information to calculate rotation for joint type
        if (!fromToJoints.ContainsKey(jt))
        {
            return initialModelJointRotation;
        }

        // check if tracking is available
        if (currentBody == null)
        {
            return initialModelJointRotation;
        }

        // original direction of bone
        Vector3 initialDirection = initialAvatarJointDirections[jt];
        // new direction of bone
        Vector3 currentDirection = getJointDirection(jt);
        // rotation between the original an new bone direction in the kinect coordinate system
        Quaternion avatarInitialToCurrentRotation = Quaternion.FromToRotation(initialDirection, currentDirection);
        // because we assured, that the model rotation is always relative to Quaternion.identity, we can simply apply the rotation-change to the original rotation
        Quaternion currentModelJointRotation = avatarInitialToCurrentRotation * initialModelJointRotation;

        return currentModelJointRotation;
    }

    public override Vector3 getRawWorldPosition(JointType jt)
    {
        if (currentBody == null)
            return Vector3.zero;
        CameraSpacePoint point = currentBody.Joints[jt].Position;
        // mirror on X/Y Plane to remove mirroring effect of the kinect data
        return new Vector3(point.X, point.Y, point.Z);
    }

    public override Quaternion getRawWorldRotation(JointType jt)
    {
        if (currentBody == null)
            return Quaternion.identity;
        Windows.Kinect.Vector4 rot = currentBody.JointOrientations[jt].Orientation;
        // is this correct like this?
        return new Quaternion(rot.X, rot.Y, rot.Z, rot.W);

    }

    // set all joints and save their initial directions and positions
    public virtual void Start()
    {
        // first find the transform objects
        foreach (JointType jt in Enum.GetValues(typeof(JointType)))
        {
            Transform joint = GameObject.Find(jt.ToString()).transform;
            if (joint == null)
            {
                Debug.Log(jt.ToString() + " not found");
            }
            jointTransforms[jt] = joint;
        }

        // for all joint mappings compute the initial direction and save it and the initial position
        foreach (JointType jt in fromToJoints.Keys)
        {
            initialAvatarJointDirections[jt] = getJointDirectionFromGO(jt);
            initialAvatarJointPositions[jt] = jointTransforms[jt].position;
        }


           /* Transform[] allChildren = this.GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren)
            {
                if (child.gameObject.GetComponent<MeshRenderer>() != null)
                {
                    child.gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
            }*/

    }

    public virtual void Update()
    {
        // no data? return
        if (BodyManager == null) return;

        Windows.Kinect.Body[] data = BodyManager.GetData();
        if (data == null) return;

        // use the first tracked body
        List<ulong> trackedIds = new List<ulong>();
        foreach (var body in data)
        {
            if (body == null) continue;
            if (body.IsTracked)
            {
                currentBody = body;                
                break;
            }
        }

        if (currentBody == null) return;

        // update debug data
        foreach (JointType jt in fromToJoints.Keys)
        {
            //transforms[jt].rotation = getRawWorldRotation(jt);
            jointTransforms[jt].position = getRawWorldPosition(jt);
            // debug: show computed rotatations
            jointTransforms[jt].rotation = applyRelativeRotationChange(jt, Quaternion.identity);
        }
    }

    public virtual Vector3 getJointDirection(JointType jt)
    {
        Vector3 jointPos = getRawWorldPosition(jt);
        Vector3 nextJointPos = getRawWorldPosition(fromToJoints[jt]);

        return nextJointPos - jointPos;
    }

    public virtual Vector3 getJointDirectionFromGO(JointType jt)
    {
        Vector3 jointPos = jointTransforms[jt].position;
        Vector3 nextJointPos = jointTransforms[fromToJoints[jt]].position;

        return nextJointPos - jointPos;
    }

    public override ulong getTrackingID()
    {
        if (currentBody == null)
            return 0;

        return currentBody.TrackingId;
    }

}
