using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The names for dodge are switched
/// Detect gestures -> Set animator -> FixedUpdate
/// </summary>
public class PlayerController : MonoBehaviour
{
    private readonly string dodgeL = "Dodge_Right";
    private readonly string dodgeR = "Dodge_Left";
    private readonly Vector3 ShiftPosR = new Vector3(1f, -4.4f, -3f);
    private readonly Vector3 ShiftPosL = new Vector3(-1f, -4.4f, -3f);
    private readonly Vector3 InitPos = new Vector3(0, -4.4f, -3f);
    
    [SerializeField] private GameObject GestureMg;
    [SerializeField] private float m_MoveSpeed;
    private GestureSourceManager GestureSrcMg;
    private Transform m_Transform;
    private Animator m_Animator;
    private CapsuleCollider m_CapsuleCollider;
    private Vector3 m_currPos;
    private float m_timeCount = 0;
    private float m_velocity = 1f;
    private bool m_locked = false;

    private void Start()
    {
        GestureSrcMg = GestureMg.GetComponent<GestureSourceManager>();
        m_Transform = GetComponent<Transform>();
        m_Animator = GetComponent<Animator>();
        m_CapsuleCollider = GetComponent<CapsuleCollider>();

        if(GestureSrcMg != null)
        {
            GestureSrcMg.GestureDetectedEvent += GestureDetectedHandler;
        }
    }

    private void FixedUpdate()
    {
        m_currPos = m_Transform.position;
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
        else
        {
            m_timeCount = 0;
            m_velocity = (m_Animator.GetBool("Running") ? 1.5f : 1f);
            m_Transform.position = Vector3.MoveTowards(m_Transform.position, InitPos, 0.1f);
            m_CapsuleCollider.center = Vector3.up;
            m_Animator.SetFloat("RunningMult", m_velocity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision!");
    }
    public float Velocity
    {
        get { return m_velocity; }
    }
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
        }
    }
    private void Move(bool right)
    {
        m_Transform.position = Vector3.Lerp(InitPos, right? ShiftPosR : ShiftPosL, m_timeCount += Time.deltaTime * m_MoveSpeed);
    }
    private void Jump()
    {
        m_CapsuleCollider.center = Vector3.up * 2f;
    }
    private IEnumerator AnimationLock()
    {
        m_locked = true;
        yield return new WaitForSeconds(1f);
        m_locked = false;
    }

}
