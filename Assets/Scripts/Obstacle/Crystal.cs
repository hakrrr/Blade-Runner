using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Crystal : MonoBehaviour
{

    [SerializeField] private float m_Speed;
    private Vector3 m_Velocity;
    private PlayerController m_Controller;
    private Rigidbody m_Rigidbody;
    private Material m_Material;
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Velocity = Vector3.back * m_Speed;
        m_Controller = GameObject.Find("Player").GetComponent<PlayerController>();
        m_Material = GetComponent<Renderer>().material;
        transform.localScale = Vector3.one * Random.Range(100, 300);
    }

    private void Start()
    {
        m_Rigidbody.velocity = m_Velocity;
        Color rnColor = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        m_Material.SetColor("_EmissionColor", rnColor);
    }

    private void FixedUpdate()
    {
        m_Rigidbody.velocity = m_Velocity * m_Controller.Velocity;
    }

}
