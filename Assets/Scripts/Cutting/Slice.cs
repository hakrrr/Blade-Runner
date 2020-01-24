using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using Cinemachine;
using DG.Tweening;

public class Slice : MonoBehaviour
{
    [SerializeField] private Transform plane;
    [SerializeField] private Blade bladeScript;
    [SerializeField] private Material crossMaterial;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject cineCamera;
    [SerializeField] private GameObject bladeParticleParent;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Animator animator;

    const float bTDIst = 8f;
    bool animLocked = false;
    Vector3 handPos = Vector3.zero;
    ParticleSystem[] bladeParticles;


    private const float sliceRadius = 25f;
    private void Awake()
    {
        bladeParticles = bladeParticleParent.GetComponentsInChildren<ParticleSystem>();
    }
    private void OnEnable()
    {
        bladeScript.OnLineDrawn += OnLineDrawn;
    }
    private void OnDisable()
    {
        bladeScript.OnLineDrawn -= OnLineDrawn;
    }
    private void Update()
    {
        if (!animLocked)
        {
            handPos = mainCamera.WorldToViewportPoint(transform.position);
            DOVirtual.Float(animator.GetFloat("x"), handPos.x * 2 - 1, .5f, (float x) => animator.SetFloat("x", x));
            DOVirtual.Float(animator.GetFloat("y"), handPos.y * 2 - 1, .5f, (float y) => animator.SetFloat("y", y));
        }
    }
    private void SetPlane(Vector3 start, Vector3 end, Vector3 normalVec)
    {
        Quaternion rotate = Quaternion.FromToRotation(Vector3.up, normalVec);

        plane.localRotation = rotate;
        plane.position = (end + start) / 2;
        plane.gameObject.SetActive(true);
    }
    private void OnLineDrawn(Vector3 A, Vector3 B, Vector3 dir)
    {
        Vector3 tangent = (B - A).normalized;
        SetPlane(A, B, Vector3.Cross(dir, tangent));
        SetBladeTrail(A, B);

        ///Visual Update and Slice
        InitSlice();
        AnimateSlice(tangent);
        BladeEffect();
    }
    private void InitSlice()
    {
        Collider [] hits = Physics.OverlapBox(plane.position, new Vector3(sliceRadius, 0.1f, sliceRadius), 
            plane.rotation, layerMask);

        if (hits.Length <= 0) return;

        foreach(Collider c in hits)
        {
            SlicedHull hull = SliceObject(c.gameObject, crossMaterial);
            if(hull != null)
            {
                GameObject upper = hull.CreateUpperHull(c.gameObject, crossMaterial);
                GameObject lower = hull.CreateLowerHull(c.gameObject, crossMaterial);
                AddHullComp(upper);
                AddHullComp(lower);
                Destroy(c.gameObject);
            }
        }
    }
    private void AddHullComp(GameObject target)
    {
        target.layer = 8;
        MeshCollider collider = target.AddComponent<MeshCollider>();
        collider.convex = true;
        Rigidbody rb = target.AddComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.useGravity = false;
        rb.AddExplosionForce(20, target.transform.position, 20);
        target.AddComponent<Hull>();
    }
    private void BladeEffect()
    {
        cineCamera.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
        foreach (ParticleSystem p in bladeParticles)
        {
            p.Play();
        }
    }
    /// <summary>
    /// Cast A on to the edge of the screen, by determining the shortest distance to each edge
    /// then cast back to worldposition. Set Bladetrail to this position and rotation to it
    /// </summary>
    private void SetBladeTrail(Vector3 A, Vector3 B)
    {
        Vector2 _A = mainCamera.WorldToViewportPoint(A);
        Vector2 _B = mainCamera.WorldToViewportPoint(B);
        Vector2 dir = (B - A).normalized;
        Vector2 _dir = (_B - _A).normalized;
        float length1 = 0, length2 = 0;
        if(_dir.x != 0)
        {
            //Negative value is the bigger one!
            length1 = (_A.x) / _dir.x;
            length1 = Mathf.Max(length1, (_A.x - 1) / _dir.x);
        }
        if(_dir.y != 0)
        {
            length2 = (_A.y) / _dir.y;
            length2 = Mathf.Max(length2, (_A.y - 1) / _dir.y);
        }
        length1 = Mathf.Min(length1, length2);
        _A -= (length1 * _dir);
        Vector3 newPos = mainCamera.ViewportToWorldPoint(new Vector3(_A.x,_A.y, bTDIst));
        bladeParticles[0].transform.position = newPos;
        bladeParticles[0].transform.rotation = Quaternion.LookRotation(dir);
        bladeParticles[0].transform.Rotate(new Vector3(90f, 90f, 90f));
    }
    private void AnimateSlice(Vector2 normDir)
    {
        handPos = mainCamera.WorldToViewportPoint(transform.position);
        animLocked = true;
        Vector2 end = Vector2.zero + normDir;
        animator.SetFloat("x", end.x);
        animator.SetFloat("y", end.y);

        DOVirtual.Float(end.x, handPos.x * 2 - 1, 0.1f, (float x) => animator.SetFloat("x", x));
        DOVirtual.Float(end.y, handPos.y * 2 - 1, 0.1f, (float y) => animator.SetFloat("y", y));

        animLocked = false;
    }
    private SlicedHull SliceObject(GameObject target, Material crossM)
    {
        if (target.GetComponent<MeshRenderer>() == null)
            return null;

        return target.Slice(plane.position, plane.up, crossM);
    }

}
