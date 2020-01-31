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
    private ParticleSystem[] m_SpeedParticles;
    private CinemachineComposer m_CineComposer;
    private CinemachineTransposer m_CineTransposer;
    private float m_timeCount = 0;
    private float m_velocity = 1f;
    private bool m_locked = false;

    private const float m_powerMult = 0.001f;

    private void Awake()
    {
        GestureSrcMg = m_GestureMg.GetComponent<GestureSourceManager>();
        m_Transform = GetComponent<Transform>();
        m_Animator = GetComponent<Animator>();
        m_CapsuleCollider = GetComponent<CapsuleCollider>();
        m_CineComposer = m_Cmvs.GetCinemachineComponent<CinemachineComposer>();
        m_CineTransposer = m_Cmvs.GetCinemachineComponent<CinemachineTransposer>();
        m_SpeedParticles = GetComponentsInChildren<ParticleSystem>();
    }
    private void Start()
    {
        m_Hand.SetActive(false);

        if (GestureSrcMg != null)
            GestureSrcMg.GestureDetectedEvent += GestureDetectedHandler;
    }
    private void Update()
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
        else if (Input.GetMouseButtonUp(1))
        {
            EndBladeMode();
        }
        else
        {
            m_timeCount = 0;
            RunningMode();
        }
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
        m_velocity = (m_Animator.GetBool("Running") ? 1.3f : 1f);
        m_Transform.position = Vector3.MoveTowards(m_Transform.position, InitPos, 0.1f);
        m_CapsuleCollider.center = Vector3.up;
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
            else if(!p.isPlaying)
                p.Play();
        }

        DOVirtual.Float(m_CineComposer.m_TrackedObjectOffset.y, RotoffsetY, .4f, SetRotOffset);
        DOVirtual.Float(m_CineTransposer.m_FollowOffset.y, PosoffsetY, .4f, SetPosOffset);
        DOVirtual.Float(m_Cmvs.m_Lens.FieldOfView, FOV, .3f, SetFOV);
        DOVirtual.Float(m_velocity, bladeMode ? .05f : 1f, .3f, (float x) => m_velocity = x);

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
