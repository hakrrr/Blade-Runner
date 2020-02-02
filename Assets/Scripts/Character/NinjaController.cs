using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaController : BasicAvatarController
{
    // Start is called before the first frame update
    void Start()
    {
        // find transforms of model
        // upper 
        SpineBase = GameObject.Find("root.x").transform;
        SpineMid = GameObject.Find("spine_01.x").transform;
        Neck = GameObject.Find("neck.x").transform;
        Head = GameObject.Find("head.x").transform;

        ShoulderLeft = GameObject.Find("arm_stretch.l").transform;
        ElbowLeft = GameObject.Find("forearm_stretch.l").transform;
        WristLeft = GameObject.Find("forearm_twist.l").transform;
        HandLeft = GameObject.Find("hand.l").transform;

        ShoulderRight = GameObject.Find("arm_stretch.r").transform;
        ElbowRight = GameObject.Find("forearm_stretch.r").transform;
        WristRight = GameObject.Find("forearm_twist.r").transform;
        HandRight = GameObject.Find("hand.r").transform;

        // under
        HipLeft = GameObject.Find("thigh_stretch.l").transform;
        KneeLeft = GameObject.Find("leg_stretch.l").transform;
        AnkleLeft = GameObject.Find("foot.l").transform;
        FootLeft = GameObject.Find("toes_01.l").transform;
        HipRight = GameObject.Find("thigh_stretch.r").transform;
        KneeRight = GameObject.Find("leg_stretch.r").transform;
        AnkleRight = GameObject.Find("foot.r").transform;
        FootRight = GameObject.Find("toes_01.r").transform;

        SpineShoulder = GameObject.Find("spine_02.x").transform;


        HandTipLeft = GameObject.Find("c_index3.l").transform;
        ThumbLeft = GameObject.Find("c_thumb3.l").transform;
        HandTipRight = GameObject.Find("c_index3.r").transform;
        ThumbRight = GameObject.Find("c_thumb3.r").transform;

        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
