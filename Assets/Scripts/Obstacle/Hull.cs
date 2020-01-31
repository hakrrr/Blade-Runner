using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hull : MonoBehaviour
{
    float lastPos;
    Transform parent;

    private void Start()
    {
        parent = transform.parent;
        lastPos = parent.position.z;
    }

    void Update()
    {
        float currPos = parent.position.z;
        transform.position += Vector3.back * Mathf.Abs(lastPos - currPos);
        lastPos = currPos;
    }
}
