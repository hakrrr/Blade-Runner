using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private readonly string dodgeL = "Dodge_Right";
    private readonly string dodgeR = "Dodge_Left";

    [SerializeField] private GameObject GestureMg;
    private GestureSourceManager GestureSrcMg;
    private Animator m_Animator;

    private void Start()
    {
        GestureSrcMg = GestureMg.GetComponent<GestureSourceManager>();
        m_Animator = GetComponent<Animator>();

        if(GestureSrcMg != null)
        {
            GestureSrcMg.GestureDetectedEvent += GestureDetectedHandler;
        }
    }

    private void GestureDetectedHandler(string name, float conf)
    {
        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Running") && conf > 0.5f)
        {
            if (name.Equals(dodgeL))
            {
                m_Animator.SetTrigger("DodgeL");
            }else if (name.Equals(dodgeR))
            {
                m_Animator.SetTrigger("DodgeR");
            }
        }
    }
}
