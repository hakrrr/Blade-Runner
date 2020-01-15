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
    private readonly float MoveShift = 0.75f;
    
    [SerializeField] private GameObject GestureMg;
    private GestureSourceManager GestureSrcMg;
    private Transform m_Transform;
    private Animator m_Animator;
    private Vector3 m_currPos;
    private float m_velocity = 1f;
    private bool m_locked = false;

    private void Start()
    {
        GestureSrcMg = GestureMg.GetComponent<GestureSourceManager>();
        m_Transform = GetComponent<Transform>();
        m_Animator = GetComponent<Animator>();

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
            m_velocity = (m_Animator.GetBool("Running") ? 1.75f : 1f);
            m_Transform.position = Vector3.Slerp(m_currPos, new Vector3(0f, -4.4f, -3f), 0.2f);
            m_Animator.SetFloat("RunningMult", m_velocity);
        }
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
        m_Transform.position = new Vector3(Mathf.Lerp(m_currPos.x, MoveShift * (right? 1 : -1), 0.1f), m_currPos.y, m_currPos.z);
    }
    private void Jump()
    {
        m_Transform.position = new Vector3(m_currPos.x, Mathf.Lerp(m_currPos.y, -3.9f, 0.1f), m_currPos.z);
    }
    private IEnumerator AnimationLock()
    {
        m_locked = true;
        yield return new WaitForSeconds(0.5f);
        m_locked = false;
    }

}
