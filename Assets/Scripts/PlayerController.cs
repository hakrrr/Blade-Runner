using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// The names for dodge are switched
/// Detect gestures -> Set animator -> FixedUpdate
/// </summary>
public class PlayerController : MonoBehaviour
{
    public delegate void UpdatePower(float p, int s);
    public UpdatePower OnUpdatePower;

    private readonly string dodgeL = "Dodge_Right";
    private readonly string dodgeR = "Dodge_Left";
    
    [SerializeField] private bool Pc;
    [SerializeField] private GameObject m_GestureMg;
    [SerializeField] private GameObject m_Hand;
    [SerializeField] private Gamemanager m_Gm;
    [SerializeField] private Camera m_Cam;
    [SerializeField] private CinemachineVirtualCamera m_Cmvs;

    private GestureSourceManager GestureSrcMg;
    private Animator m_Animator;
    private ParticleSystem[] m_SpeedParticles;
    private CinemachineComposer m_CineComposer;
    private CinemachineTransposer m_CineTransposer;
    private BoxCollider m_Coll;
    private float m_startX = 0;
    private float m_velocity = 1f;
    private bool m_bladeRdy = false;
    private bool m_locked = false;

    private const float m_powerMult = 0.003f;

    /// <summary>
    /// SpeedParticles Index 0 = SwordLight, 1 = Warpstr, 2 = FootLight
    /// </summary>
    private void Awake()
    {
        GestureSrcMg = m_GestureMg.GetComponent<GestureSourceManager>();
        m_Animator = GetComponent<Animator>();
        m_CineComposer = m_Cmvs.GetCinemachineComponent<CinemachineComposer>();
        m_CineTransposer = m_Cmvs.GetCinemachineComponent<CinemachineTransposer>();
        m_Coll = GetComponent<BoxCollider>();
        m_SpeedParticles = GetComponentsInChildren<ParticleSystem>(true);
    }
    private void Start()
    {
        m_Hand.SetActive(false);

        if (GestureSrcMg != null)
            GestureSrcMg.GestureDetectedEvent += GestureDetectedHandler;
    }
    private void Update()
    {
        UpdateBladeRdy();
        if (Pc && !m_locked)
            PCInput();
        if(m_Gm.GetStatus().y == 0f)
            EndBladeMode();
        AnimationInput();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Hit!");
        }
    }
    private void PCInput()
    {
        if (Input.GetMouseButtonDown(1) && m_bladeRdy)
        {
            m_locked = true;
            m_Animator.SetBool("BladeMode", true);
        }
        else if (Input.GetKeyDown("space"))
        {
            m_locked = true;
            m_Animator.SetTrigger("Jump");
        }
        else if (Input.GetKeyDown("d"))
        {
            m_locked = true;
            m_startX = transform.position.x;
            m_Animator.SetTrigger("DodgeR");
        }
        else if (Input.GetKeyDown("a"))
        {
            m_locked = true;
            m_startX = transform.position.x;
            m_Animator.SetTrigger("DodgeL");
        }
        else if (Input.GetKey("w"))
        {
            m_Animator.SetBool("Running", true);
        }
        else
        {
            m_Animator.SetBool("Running", false);
        }
    }
    private void AnimationInput()
    {
        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("DodgeL"))
        {
            if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < .5f)
            {
                transform.DOMoveX(Mathf.Clamp(m_startX - 1.75f, -1.75f, 2.75f), 0.3f);
                DOVirtual.Float(m_velocity, .5f, 0.1f, (float x) => m_velocity = x);
            }
            EnableFootPart(false);
        }
        else if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("DodgeR"))
        {
            if(m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < .5f)
            {
                transform.DOMoveX(Mathf.Clamp(m_startX + 1.75f, -1.75f, 2.75f), 0.3f);
                DOVirtual.Float(m_velocity, .5f, 0.1f, (float x) => m_velocity = x);
            }
            EnableFootPart(false);
        }
        else if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            m_Coll.enabled = 
                !(m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < .7f);
            EnableFootPart(false);
        }
        else if (m_Animator.GetBool("BladeMode"))
        {
            BladeMode();
        }
        else
        {
            RunningMode();
        }
    }
    public float Velocity { get { return m_velocity; }}
    private void GestureDetectedHandler(string name, float conf)
    {
        if (!m_locked &&m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Running") && conf > 0.5f)
        {
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
        m_Hand.SetActive(true);
        CameraZoom(true);
    }
    private void EndBladeMode()
    {
        float hits = m_Hand.GetComponent<Slice>().hitCounter;
        OnUpdatePower?.Invoke(hits * m_powerMult, (int) hits * 10);
        m_Animator.SetBool("BladeMode", false);
        m_Hand.SetActive(false);
    }
    private void RunningMode()
    {
        CameraZoom(false);

        if (!m_Animator.GetBool("Running"))
            EnableFootPart(false);
        else
            EnableFootPart(true);

        //Note: Animation of Run should be dampened
        m_Animator.SetFloat("RunningMult", m_velocity);
    }
    private void CameraZoom(bool bladeMode)
    {
        float RotoffsetY = bladeMode ? 0.3f : 0f;
        float PosoffsetY = bladeMode ? 0.3f : 0f;
        float FOV = bladeMode ? 15.1f : 34.4f;

        foreach (ParticleSystem p in m_SpeedParticles)
        {
            if (bladeMode)
                p.Stop();
        }

        DOVirtual.Float(m_CineComposer.m_TrackedObjectOffset.y, RotoffsetY, .4f, SetRotOffset);
        DOVirtual.Float(m_CineTransposer.m_FollowOffset.y, PosoffsetY, .4f, SetPosOffset);
        DOVirtual.Float(m_Cmvs.m_Lens.FieldOfView, FOV, .3f, SetFOV);
        DOVirtual.Float(m_velocity, bladeMode ? .05f : m_Animator.GetBool("Running")? 1.5f : 1f,
            .3f, (float x) => m_velocity = x);

        //Post Processing
        float currChrom = m_Cam.GetComponentInChildren<PostProcessVolume>().profile.
            GetSetting<ChromaticAberration>().intensity.value;
        float currVign = m_Cam.GetComponentInChildren<PostProcessVolume>().profile.
            GetSetting<Vignette>().intensity.value;

        DOVirtual.Float(currChrom, bladeMode ? .25f : .5f, .3f, SetChromatic);
        DOVirtual.Float(currVign, bladeMode ? .6f : 0f, .3f, SetVignette);
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
    private void SetChromatic(float chrom)
    {
        m_Cam.GetComponentInChildren<PostProcessVolume>().profile.GetSetting<ChromaticAberration>().
            intensity.value = chrom;
    }
    private void SetVignette(float vign)
    {
        m_Cam.GetComponentInChildren<PostProcessVolume>().profile.GetSetting<Vignette>().
            intensity.value = vign;
    }
    #endregion

    #region Particles
    private void EnableFootPart(bool enable)
    {
        if (!enable)
            m_SpeedParticles[2].Stop();
        else if (!m_SpeedParticles[2].isPlaying)
            m_SpeedParticles[2].Play();
    }    
    private void EnableSwordPart(bool enable)
    {
        if (!enable)
            m_SpeedParticles[0].Stop();
        else if(!m_SpeedParticles[0].isPlaying)
            m_SpeedParticles[0].Play();
    }
    #endregion
    private void UpdateBladeRdy()
    {
        m_bladeRdy = (m_Gm.GetStatus().y > 0.6f);
        EnableSwordPart(m_bladeRdy);
    }
    //For Run Animation to call to free Animation lock
    public void Unlock()
    {
        m_locked = false;
    }
}
