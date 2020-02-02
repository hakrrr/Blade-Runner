using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Windows.Kinect;
using System;

public abstract class BasicAvatarModel : MonoBehaviour {


    public abstract Quaternion applyRelativeRotationChange(JointType jt, Quaternion initialModelJointRotation);

    public abstract Quaternion getRawWorldRotation(JointType jt);

    public abstract Vector3 getRawWorldPosition(JointType jt);

    public abstract ulong getTrackingID();
}
