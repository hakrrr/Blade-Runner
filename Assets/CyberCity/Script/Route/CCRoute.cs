using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCRoute : MonoBehaviour 
{
    [Header("[simple, rough system of route]")]
    [Header("USAGE:")]
    [Header("A. ROUTE")]
    [Header("1. create new GameObject;")]
    [Header("2. add 'CCRoute' script on it;")]
    [Header("3. make children GameObjects = route points.")]
    [Space(10)]
    [Header("B. VEHICLE")]
    [Header("1. add your vehicle to scene (+rigidbody);")]
    [Header("2. add 'CCVehicle' script on it;")]
    [Header("3. add your route (see A.1.) to 'Route' slot;")]
    [Header("4. adjust intuitive params;")]
    [Header("5. DONE.")]

    [Space(10)]
    public Transform[] RoutePoints;

    //collect all points (full route) in children
    //route is sensitive to the children gameobject hierarchy
    void Awake()
    {
        if (transform.childCount != 0)
        {
            RoutePoints = new Transform[transform.childCount];
        }
        else
        {
            Debug.Log(" <color=yellow> no route points! </color>");
            return;
        }
        for (int k = 0; k < transform.childCount; k++)
        {
            RoutePoints[k] = transform.GetChild(k);
        }
    }

	void Start () 
    {

	}
	
	void Update () 
    {
		
	}

    //draw lines (full route) in editor
    void OnDrawGizmos()
    {
        Vector3 Point = Vector3.zero;
        Vector3 LastPoint = Vector3.zero;
        for (int k = 0; k < transform.childCount; k++)
        {
            Point = transform.GetChild(k).transform.position;
            if (LastPoint != Vector3.zero)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(Point, LastPoint);
            }
            LastPoint = Point;
        }
    }
}
