using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class DetectJoints : MonoBehaviour
{
    [SerializeField] private GameObject BodySrcMg;
    [SerializeField] private JointType TrackedJoint;
    [SerializeField] private float Multiplier = 10f;
    private BodySourceManager BodyMg;
    private Body[] bodies;


    void Start()
    {
        if (BodySrcMg == null) Debug.Log("BodySrcMg is empty!");
        else BodyMg = BodySrcMg.GetComponent<BodySourceManager>();
    }

    private void Update()
    {
        if (BodyMg == null) return;
        bodies = BodyMg.GetData();
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
