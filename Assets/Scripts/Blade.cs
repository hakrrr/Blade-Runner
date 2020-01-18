using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    [SerializeField] private GameObject TrackedHand;

    //Blade is Publisher for Event OnLineDrawn
    //delegate is a "pointer" to a function and specifies the listener function's signature
    public delegate void LineDrawnHandler(Vector3 begin, Vector3 end, Vector3 depth);
    public event LineDrawnHandler OnLineDrawn;

    private const float minBladeSpeed = 1.4f;
    private const float bladeDur = .2f;
    private readonly Vector3 initPos = new Vector3(0, -20f, -10f);

    private Camera cam;
    private GameObject bladeTrail;
    private Transform handTf;
    private Vector3 lastPosition;
    private bool bladeActive = false;

    private void Awake()
    {
        handTf = TrackedHand.GetComponent<Transform>();
        bladeTrail = transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        lastPosition = initPos;
        bladeTrail.SetActive(false);
        cam = GetComponent<DetectJoints>().CameraTrack;
    }

    private void Update()
    {
        float velocity = (handTf.position - lastPosition).magnitude * Time.deltaTime * 1000f;
        if(!bladeActive && lastPosition != initPos && velocity > minBladeSpeed)
        {
            bladeActive = true;
            StartCoroutine(StartCut());
        }

        lastPosition = handTf.position;
    }

    /// <summary>
    /// Start´s Cutanimation / Trail
    /// Ends when the velocity goes back => bladeActive == false;
    /// A and B are the Start and End Point of the Cut;
    /// </summary>
    /// <returns></returns>
    IEnumerator StartCut()
    {
        Vector3 A = cam.WorldToScreenPoint(transform.position);
        bladeTrail.SetActive(true);
        yield return new WaitForSeconds(bladeDur);

        bladeTrail.SetActive(false);
        bladeActive = false;
        Vector3 B = cam.WorldToScreenPoint(transform.position);
        Debug.DrawLine(A, B, Color.white, 1f);
        //Invoke Event passing the Data to HandSlice
        var start = cam.ViewportPointToRay(A);
        var end = cam.ViewportPointToRay(B);

        OnLineDrawn?.Invoke(start.GetPoint(cam.nearClipPlane),
            end.GetPoint(cam.nearClipPlane),
            end.direction.normalized);
    }
}
