using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clean : MonoBehaviour
{
    GameObject Terrain;
    private void Awake()
    {
        Terrain = gameObject;
        if (Terrain != null)
        {
            for (int i = 0; i < Terrain.transform.childCount; i++)
            {
                GameObject child = Terrain.transform.GetChild(i).gameObject;
                if (child.GetComponents<Component>().Length == 1)
                    Destroy(child);
                else if (child.GetComponent<Collider>() != null && child.layer != 8)
                    Destroy(child.GetComponent<Collider>());
            }
        }
    }
}

