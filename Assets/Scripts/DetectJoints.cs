using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class DetectJoints : MonoBehaviour
{
    [SerializeField] private GameObject BodyMg;
    [SerializeField] private JointType TrackedJoint;
    [SerializeField] private float Multiplier = 10f;
    private BodySourceManager BodySrcMg;
    private Body[] bodies;


    void Start()
    {
        if (BodyMg == null) Debug.Log("BodyMg is empty!");
        else BodySrcMg = BodyMg.GetComponent<BodySourceManager>();
    }

    private void Update()
    {
        if (BodySrcMg == null) return;
        bodies = BodySrcMg.GetData();
        if (bodies == null) return;

        foreach (var body in bodies)
        {
            if (body == null) continue;

            if (body.IsTracked)
            {
                var pos = body.Joints[TrackedJoint].Position;
                transform.position = new Vector3(pos.X * Multiplier, pos.Y * Multiplier);
            }

        }
    }

}
