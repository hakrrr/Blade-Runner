using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;
using DG.Tweening;
using UnityEngine.UI;

public class CursorScript : MonoBehaviour
{

    [SerializeField] private Camera CameraTrack;
    [SerializeField] private float projectedZ;
    [SerializeField] private GameObject BodyMg = null;
    [SerializeField] private JointType TrackedJoint = 0;
    
    private const float ScreenShift = 0.4f;
    private BodySourceManager BodySrcMg;
    private Body[] bodies;
    private float time = 0f;
    private SceneMg sM;

    
    void Start()
    {
        if (BodyMg == null) Debug.Log("BodyMg is empty!");
        else BodySrcMg = BodyMg.GetComponent<BodySourceManager>();

        sM = GameObject.Find("SceneMg").GetComponent<SceneMg>();
        time = 0;
    }
    /// <summary>
    /// HandPosition into CameraView
    /// KinectCoordinates are in meters. 
    /// ScreenShift marks the corner-distance of the Screen from the center-point
    /// </summary>
    private void Update()
    {
        if (BodySrcMg == null) return;

        bodies = BodySrcMg.GetData();
        
        if (bodies == null) return;

        foreach (var body in bodies)
        {
            if (body == null) continue;

            //Transform Kinect Coordinates to Screen Coordinates
            if (body.IsTracked)
            {
                var pos = body.Joints[TrackedJoint].Position;
                float newX = (pos.X + ScreenShift) * Screen.width / (2f * ScreenShift),
                    newY = (pos.Y + ScreenShift) * Screen.height / (2f * ScreenShift);
                Vector3 ScreenPos = CameraTrack.ScreenToWorldPoint(new Vector3(newX, newY, projectedZ));
                transform.DOMove(ScreenPos, 0.1f);
            }

        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        time += Time.deltaTime;
        collision.GetComponent<Image>().color -= new Color(0, 0, 0, Time.deltaTime);
        if (time >= 1f)
        {
            if (collision.name == "Exit")
                sM.FadeToScene(0);
            if (collision.name == "Retry")
                sM.FadeToScene(1);

            time = -100f;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        time = 0;
        Color local = collision.GetComponent<Image>().color;
        local.a = 1;
        collision.GetComponent<Image>().color = local;
    }
}
