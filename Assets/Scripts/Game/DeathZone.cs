using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Parent") || other.gameObject.layer == 8)
        {
            Destroy(other.gameObject);
        }

    }
}
