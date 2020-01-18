using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private GameObject m_Lookat;

    private void FixedUpdate()
    {
        transform.LookAt(m_Lookat.transform);
    }
}
