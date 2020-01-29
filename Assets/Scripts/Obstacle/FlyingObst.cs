using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class FlyingObst : MonoBehaviour
{

    [SerializeField] private float m_Speed;
    [SerializeField] private float m_rotSpeed;
    private Vector3 m_Velocity;
    private PlayerController m_Controller;
    private Rigidbody m_Rigidbody;
    private Vector3 m_RotateAngle;
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Velocity = Vector3.back * m_Speed;
        m_Controller = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void Start()
    {
        m_Rigidbody.velocity = m_Velocity;
        m_RotateAngle = Random.Range(0, 1) == 0 ? Vector3.right : Vector3.up;
    }

    private void FixedUpdate()
    {
        m_Rigidbody.velocity = m_Velocity * m_Controller.Velocity;
        transform.Rotate(m_RotateAngle * m_rotSpeed * m_Controller.Velocity);
    }

}
