using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCLines : MonoBehaviour 
{
    [HideInInspector]
    public Transform[] RoutePoints;

    void Awake()
    {

    }

	void Start () 
    {

	}
	
	void Update () 
    {
		
	}

    void OnDrawGizmos()
    {
        Vector3 Point = Vector3.zero;
        Vector3 LastPoint = Vector3.zero;
        for (int k = 0; k < transform.childCount; k++)
        {
            Point = transform.GetChild(k).transform.position;
            if (LastPoint != Vector3.zero)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(Point, LastPoint);
            }
            LastPoint = Point;
        }
    }
}
