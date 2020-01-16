using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject bladeTrail;

    private Rigidbody rb;
    private GameObject currBTrail;
    private Transform rHandTf;
    private Transform lHandTf;
    private bool bladeActive = false;

    private void Awake()
    {
        rHandTf = rightHand.GetComponent<Transform>();
        lHandTf = leftHand.GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //if(Vector2.Distance(rHandTf.position, lHandTf.position) < 2f)
        if(true)
        {
            bladeActive = true;
            UpdateBlade();
            //currBTrail = Instantiate(bladeTrail, transform);
        }
        else
        {
            bladeActive = false;
            //currBTrail.transform.SetParent(null);
            //Destroy(currBTrail, 2f);
        }
    }

    void UpdateBlade()
    {
        rb.position = rHandTf.position;
    }
}
