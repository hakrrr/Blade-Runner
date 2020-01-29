//CyberCity script for demo

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCFollower : MonoBehaviour 
{
    public float SpeedMove = 1.1f;
    public float SpeedLook = 1.0f;
    public float StopDistance = 2.0f;
    public GameObject Player;
    bool DontMove;

	void Start () 
    {

	}
	
	void Update () 
    {

	}

    void FixedUpdate()
    {
        //lookat
        var localTarget = transform.InverseTransformPoint(Player.transform.position);
        float angle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
        Vector3 eulerAngleVelocity = new Vector3(0, angle, 0);
        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime * SpeedLook);
        GetComponent<Rigidbody>().MoveRotation(GetComponent<Rigidbody>().rotation * deltaRotation);

        //move
        Vector3 futur_pos = transform.TransformDirection(new Vector3(0, 0, SpeedMove * Time.deltaTime));
        Vector3 step_pos = transform.position + futur_pos;
        if (!DontMove) GetComponent<Rigidbody>().MovePosition(step_pos);

        //stop
        if (Vector3.Distance(Player.transform.position, transform.position) < StopDistance)
        {
            DontMove = true;
        }
        else
        {
            DontMove = false;
        }
    }
}
