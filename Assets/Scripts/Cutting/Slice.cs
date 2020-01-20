using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class Slice : MonoBehaviour
{
    [SerializeField] private Transform plane;
    [SerializeField] private Blade bladeScript;
    [SerializeField] private Material crossMaterial;
    [SerializeField] private LayerMask layerMask;

    private const float sliceRadius = 6f;
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

        plane.transform.localRotation = rotate;
        plane.transform.position = (end + start) / 2;
        plane.gameObject.SetActive(true);
    }
    private void OnLineDrawn(Vector3 A, Vector3 B, Vector3 dir)
    {
        Vector3 tangent = (B - A).normalized;
        SetPlane(A, B, Vector3.Cross(dir, tangent));
        InitSlice();
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
    private SlicedHull SliceObject(GameObject target, Material crossM)
    {
        if (target.GetComponent<MeshRenderer>() == null)
            return null;

        return target.Slice(plane.position, plane.up, crossM);
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

}
