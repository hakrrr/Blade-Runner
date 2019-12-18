using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    [SerializeField] private float m_Speed;
    private Rigidbody m_Rigidbody;
    private Vector3 m_Velocity;

    private void OnDeath()
    {
        Destroy(this.gameObject);
    }

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Velocity = Vector3.back;
    }

    private void Start()
    {
        m_Rigidbody.velocity = m_Velocity * m_Speed;
    }

    private void FixedUpdate()
    {
        if (m_Rigidbody.transform.position.z < -18) OnDeath();
    }

}
