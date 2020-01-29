using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCVehicle : MonoBehaviour 
{
    [Header("[simple, rough system of route]")]
    [Space(10)]
    public CCRoute Route;
    [Space(15)]
    [Header("if enabled: first point will be after last point")]
    [Header("if disabled: penultimate point will be after last point")]
    public bool IsLoop;
    [Space(15)]
    public float SpeedMove = 1.1f;
    float HitDistance = 0.7f;
    Transform CurrentTarget;
    int k;
    bool direction = true;
    bool found;
    float SpeedLook;

	void Start () 
    {
        SpeedLook = SpeedMove;

        //get first nearest route point:
        float LastDistance = 10000f;
        for (int i = 0; i < Route.RoutePoints.Length; i++)
        {
            float dist = Vector3.Distance(transform.position, Route.RoutePoints[i].position);
            if (dist < LastDistance)
            {
                CurrentTarget = Route.RoutePoints[i];
                LastDistance = dist;
                found = true;
                k = i;
            }
        }
	}

    void FixedUpdate()
    {
        if (found)
        {
            //physics lookat
            var localTarget = transform.InverseTransformPoint(CurrentTarget.position);
            float angle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
            Vector3 eulerAngleVelocity = new Vector3(0, angle, 0);
            Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime * SpeedLook);
            GetComponent<Rigidbody>().MoveRotation(GetComponent<Rigidbody>().rotation * deltaRotation);

            //physics move
            Vector3 futur_pos = transform.TransformDirection(new Vector3(0, 0, SpeedMove * Time.deltaTime));
            Vector3 step_pos = transform.position + futur_pos;
            GetComponent<Rigidbody>().MovePosition(step_pos);

            //hit route point
            if (FastDistance(transform, CurrentTarget, HitDistance))
            {
                //positive
                if (direction)
                {
                    k++;
                    //get new one
                    if (k < Route.RoutePoints.Length)
                    {
                        CurrentTarget = Route.RoutePoints[k];
                    }
                    else
                    {
                        if (!IsLoop)
                        {
                            k -= 2;
                            direction = false;
                            CurrentTarget = Route.RoutePoints[k];
                        }
                        else
                        {
                            k = 0;
                            CurrentTarget = Route.RoutePoints[k];
                        }
                    }
                }
                //negative
                else
                {
                    k--;
                    //get new one
                    if (k >= 0)
                    {
                        CurrentTarget = Route.RoutePoints[k];
                    }
                    else
                    {
                        if (!IsLoop)
                        {
                            k += 2;
                            direction = true;
                            CurrentTarget = Route.RoutePoints[k];
                        }
                        else
                        {
                            k = Route.RoutePoints.Length - 1;
                            CurrentTarget = Route.RoutePoints[k];
                        }
                    }
                }
            }
        }
    }

    //fast fake distance (w/o 'Y')
    bool FastDistance(Transform Self, Transform Target, float Radius)
    {
        bool Xpass = false;
        bool Zpass = false;

        //x
        if ((Self.position.x >= 0 & Target.position.x >= 0) | (Self.position.x < 0 & Target.position.x < 0))
        {
            if (Mathf.Abs(Mathf.Abs(Self.position.x) - Mathf.Abs(Target.position.x)) < Radius) Xpass = true;
        }
        else
        {
            if (Mathf.Abs(Self.position.x) + Mathf.Abs(Target.position.x) < Radius) Xpass = true;
        }

        //z
        if ((Self.position.z >= 0 & Target.position.z >= 0) | (Self.position.z < 0 & Target.position.z < 0))
        {
            if (Mathf.Abs(Mathf.Abs(Self.position.z) - Mathf.Abs (Target.position.z)) < Radius) Zpass = true;
        }
        else
        {
            if (Mathf.Abs(Self.position.z) + Mathf.Abs(Target.position.z) < Radius) Zpass = true;
        }

        if (Xpass & Zpass) return true;
        else return false;
    }

}
