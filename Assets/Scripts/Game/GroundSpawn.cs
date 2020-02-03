using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawn : MonoBehaviour
{
    [SerializeField] private Transform forward;
    [SerializeField] private float begin;
    [SerializeField] private float offset;
    
    private void Update()
    {
        Vector3 currPos = transform.localPosition;
        if(currPos.z <= begin)
        {
            transform.localPosition = new Vector3(currPos.x, currPos.y,
                forward.localPosition.z + offset);
        }
    }

}
