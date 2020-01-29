using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GroundObst : MonoBehaviour
{

    [SerializeField] private float m_Speed;
    private Vector3 m_Velocity;
    private PlayerController m_Controller;
    private Rigidbody m_Rigidbody;
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Velocity = Vector3.back * m_Speed;
        m_Controller = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void Start()
    {
        m_Rigidbody.velocity = m_Velocity;
    }

    private void FixedUpdate()
    {
        m_Rigidbody.velocity = m_Velocity * m_Controller.Velocity;
    }

}
