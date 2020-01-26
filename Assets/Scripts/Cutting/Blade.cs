using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    [SerializeField] private GameObject trackedHand;
    [SerializeField] private bool mouseDrawn;

    //Blade is Publisher for Event OnLineDrawn
    //delegate is a "pointer" to a function and specifies the listener function's signature
    public delegate void LineDrawnHandler(Vector3 begin, Vector3 end, Vector3 depth);
    public event LineDrawnHandler OnLineDrawn;

    private const float minBladeSpeed = .5f;
    private const float bladeDur = .2f;
    private readonly Vector3 initPos = new Vector3(0, -20f, -10f);

    private Camera cam;
    //private GameObject bladeTrail;
    private DetectJoints detectJoints;
    private Transform handTf;
    private Vector3 lastPosition;
    private bool bladeActive = false;

    private void Awake()
    {
        handTf = trackedHand.GetComponent<Transform>();
        detectJoints = GetComponent<DetectJoints>();
        //bladeTrail = transform.GetChild(0).gameObject;
        if (mouseDrawn)
        {
            detectJoints.enabled = false;
        }
    }
    private void OnEnable()
    {
        lastPosition = initPos;
        cam = GetComponent<DetectJoints>().CameraTrack;
        //bladeTrail.SetActive(false);
    }
    private void Update()
    {
        float velocity = (handTf.position - lastPosition).magnitude * Time.deltaTime * 1000f;
        if(!mouseDrawn && !bladeActive && lastPosition != initPos && velocity > minBladeSpeed)
        {
            bladeActive = true;
            StartCoroutine(StartCut());
        }
        else if(mouseDrawn && Input.GetMouseButtonDown(0))
        {
            bladeActive = true;
            StartCoroutine(StartCut());
        }

        lastPosition = handTf.position;

        if (mouseDrawn)
        {
            handTf.position = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1f));
        }
    }

    /// <summary>
    /// Start´s Cutanimation / Trail
    /// Ends when the velocity goes back => bladeActive == false;
    /// A and B are the Start and End Point of the Cut;
    /// </summary>
    /// <returns></returns>
    IEnumerator StartCut()
    {
        Vector3 A = mouseDrawn ? Input.mousePosition : cam.WorldToScreenPoint(handTf.position);

        //Activate Blade; Bladetime == bladeDur
        //bladeTrail.SetActive(true);
        yield return new WaitForSeconds(bladeDur);
        //bladeTrail.SetActive(false);
        bladeActive = false;
        Vector3 B = mouseDrawn ? Input.mousePosition : cam.WorldToScreenPoint(handTf.position);

        //Invoke Event passing the Data to HandSlice
        var start = cam.ScreenPointToRay(A);
        var end = cam.ScreenPointToRay(B);

        OnLineDrawn?.Invoke(start.GetPoint(cam.nearClipPlane),
            end.GetPoint(cam.nearClipPlane),
            end.direction.normalized);
    }

}
