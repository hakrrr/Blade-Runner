using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveCollider : MonoBehaviour
{
    private void Awake()
    {
        foreach(Collider c in GetComponentsInChildren<Collider>())
        {
            if (c.gameObject.layer != 8)
                Destroy(c);
        }
    }
}
