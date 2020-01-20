﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

/// <summary>
/// The names for dodge are switched
/// Detect gestures -> Set animator -> FixedUpdate
/// </summary>
public class PlayerController : MonoBehaviour
{
    private readonly string dodgeL = "Dodge_Right";
    private readonly string dodgeR = "Dodge_Left";
    private readonly Vector3 ShiftPosR = new Vector3(0.75f, -4.4f, -3f);
    private readonly Vector3 ShiftPosL = new Vector3(-0.75f, -4.4f, -3f);
    private readonly Vector3 InitPos = new Vector3(0, -4.4f, -3f);

    [SerializeField] private GameObject m_GestureMg;
    [SerializeField] private GameObject m_Hand;
    [SerializeField] private Camera m_Cam;
    [SerializeField] private CinemachineVirtualCamera m_Cmvs;
    [SerializeField] private float m_DodgeSpeed;


    private GestureSourceManager GestureSrcMg;
    private Transform m_Transform;
    private Animator m_Animator;
    private CapsuleCollider m_CapsuleCollider;
    private ParticleSystem m_Warpstreak;

    private CinemachineComposer m_CineComposer;
    private CinemachineTransposer m_CineTransposer;

    private float m_timeCount = 0;
    private float m_velocity = 1f;
    private bool m_locked = false;


    private void Awake()
    {
        GestureSrcMg = m_GestureMg.GetComponent<GestureSourceManager>();
        m_Transform = GetComponent<Transform>();
        m_Animator = GetComponent<Animator>();
        m_CapsuleCollider = GetComponent<CapsuleCollider>();
        m_CineComposer = m_Cmvs.GetCinemachineComponent<CinemachineComposer>();
        m_CineTransposer = m_Cmvs.GetCinemachineComponent<CinemachineTransposer>();
        m_Warpstreak = GetComponentInChildren<ParticleSystem>(true);
    }
    private void Start()
    {
        m_Hand.SetActive(false);

        if (GestureSrcMg != null)
            GestureSrcMg.GestureDetectedEvent += GestureDetectedHandler;
    }
    private void FixedUpdate()
    {
        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("DodgeL"))
        {
            Move(false);
        }
        else if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("DodgeR"))
        {
            Move(true);
        }
        else if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            Jump();
        }
        //BladeMode
        else if (Input.GetMouseButton(1))
        {
            BladeMode();
        }
        else
        {
            m_timeCount = 0;
            EndBladeMode();
            RunningMode();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision!");
    }
    public float Velocity { get { return m_velocity; }}
    private void GestureDetectedHandler(string name, float conf)
    {
        if (!m_locked &&m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Running") && conf > 0.5f)
        {
            StartCoroutine(AnimationLock());
            if (name.Equals(dodgeL))
            {
                m_Animator.SetTrigger("DodgeL");
            }else if (name.Equals(dodgeR))
            {
                m_Animator.SetTrigger("DodgeR");
            }

            //Todo: Draw Katana
        }
    }

    #region Mode
    private void BladeMode()
    {
        m_Animator.SetBool("BladeMode", true);
        m_Hand.SetActive(true);
        Vector3 handPos = m_Cam.WorldToViewportPoint(m_Hand.transform.position);
        m_Animator.SetFloat("x", handPos.x * 2 - 1);
        m_Animator.SetFloat("y", handPos.y * 2 - 1);
        CameraZoom(true);
    }
    private void EndBladeMode()
    {
        m_Animator.SetBool("BladeMode", false);
        m_Hand.SetActive(false);
        CameraZoom(false);
    }
    private void RunningMode()
    {
        m_velocity = (m_Animator.GetBool("Running") ? 1.3f : 1f);
        m_Transform.position = Vector3.MoveTowards(m_Transform.position, InitPos, 0.1f);
        m_CapsuleCollider.center = Vector3.up;
        m_Animator.SetFloat("RunningMult", m_velocity);
    }
    private void CameraZoom(bool bladeMode)
    {
        float RotoffsetY = bladeMode ? 0.3f : 0f;
        float PosoffsetY = bladeMode ? 0.3f : 0f;
        float FOV = bladeMode ? 15f : 33.4f;

        if (bladeMode)
            m_Warpstreak.Stop();
        else
            m_Warpstreak.Play();

        DOVirtual.Float(m_CineComposer.m_TrackedObjectOffset.y, RotoffsetY, .4f, SetRotOffset);
        DOVirtual.Float(m_CineTransposer.m_FollowOffset.y, PosoffsetY, .4f, SetPosOffset);
        DOVirtual.Float(m_Cmvs.m_Lens.FieldOfView, FOV, .3f, SetFOV);
    }
    private void SetRotOffset(float offset)
    {
        m_CineComposer.m_TrackedObjectOffset.y = offset;
    }    
    private void SetPosOffset(float offset)
    {
        m_CineTransposer.m_FollowOffset.y = offset;
    }
    private void SetFOV(float fov)
    {
        m_Cmvs.m_Lens.FieldOfView = fov;
    }
    #endregion

    #region Movement
    private void Move(bool right)
    {
        m_Transform.position = Vector3.Lerp(InitPos, right? ShiftPosR : ShiftPosL, m_timeCount += Time.deltaTime * m_DodgeSpeed);
    }
    private void Jump()
    {
        m_CapsuleCollider.center = Vector3.up * 2f;
    }
    #endregion

    private IEnumerator AnimationLock()
    {
        m_locked = true;
        yield return new WaitForSeconds(1f);
        m_locked = false;
    }

}
