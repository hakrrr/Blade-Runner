using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using Cinemachine;

public class Slice : MonoBehaviour
{
    [SerializeField] private Transform plane;
    [SerializeField] private Blade bladeScript;
    [SerializeField] private Material crossMaterial;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject cineCamera;
    [SerializeField] private GameObject bladeParticleParent;
    [SerializeField] private LayerMask layerMask;

    const float bTDIst = 6f;
    ParticleSystem[] bladeParticles;

    private const float sliceRadius = 50f;
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
        InitSlice();
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
        Rigidbody rb = target.AddComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.useGravity = false;
        MeshCollider collider = target.AddComponent<MeshCollider>();
        collider.convex = true;

        rb.AddExplosionForce(30, target.transform.position, 20);
        //rb.velocity = Vector3.back;
    }
    private void BladeEffect()
    {
        cineCamera.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
        foreach (ParticleSystem p in bladeParticles)
        {
            p.Play();
        }
    }
    private void SetBladeTrail(Vector3 A, Vector3 B)
    {
        Vector2 dir = (B - A).normalized;
        Vector2 _A = mainCamera.WorldToViewportPoint(A);
        float length1 = 0, length2 = 0;
        //if not parallel to Axes
        if(dir.x != 0)
        {
            //Negative value is the bigger one!
            length1 = (_A.x) / dir.x;
            length1 = Mathf.Max(length1, (_A.x - 1) / dir.x);
        }
        if(dir.y != 0)
        {
            length2 = (_A.y) / dir.y;
            length2 = Mathf.Max(length2, (_A.y - 1) / dir.y);
        }
        length1 = Mathf.Min(length1, length2);
        _A -= (length1 * dir);
        Vector3 newPos = mainCamera.ViewportToWorldPoint(new Vector3(_A.x,_A.y, bTDIst));
        bladeParticles[0].transform.position = newPos;
        bladeParticles[0].transform.rotation = Quaternion.LookRotation(dir);
        bladeParticles[0].transform.Rotate(new Vector3(90, 90, 90));

    }
    private SlicedHull SliceObject(GameObject target, Material crossM)
    {
        if (target.GetComponent<MeshRenderer>() == null)
            return null;

        return target.Slice(plane.position, plane.up, crossM);
    }
}
