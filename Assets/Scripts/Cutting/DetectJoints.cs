using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;
using DG.Tweening;

public class DetectJoints : MonoBehaviour
{

    public Camera CameraTrack;

    [SerializeField] private GameObject BodyMg = null;
    [SerializeField] private JointType TrackedJoint = 0;
    
    private const float ScreenShift = 0.4f;
    private BodySourceManager BodySrcMg;
    private Body[] bodies;


    void Start()
    {
        if (BodyMg == null) Debug.Log("BodyMg is empty!");

        else BodySrcMg = BodyMg.GetComponent<BodySourceManager>();

    }

    /// <summary>
    /// HandPosition into CameraView
    /// KinectCoordinates are in meters. 
    /// ScreenShift marks the corner-distance of the Screen from the center-point
    /// </summary>
    private void Update()
    {
        if (BodySrcMg == null) return;

        bodies = BodySrcMg.GetData();
        
        if (bodies == null) return;

        foreach (var body in bodies)
        {
            if (body == null) continue;

            //Transform Kinect Coordinates to Screen Coordinates
            if (body.IsTracked)
            {
                var pos = body.Joints[TrackedJoint].Position;
                float newX = (pos.X + ScreenShift) * Screen.width / (2f * ScreenShift),
                    newY = (pos.Y + ScreenShift) * Screen.height / (2f * ScreenShift);
                Vector3 ScreenPos = CameraTrack.ScreenToWorldPoint(new Vector3(newX, newY, 1f));
                transform.DOMove(ScreenPos, 0.1f);
            }

        }

    }

}
