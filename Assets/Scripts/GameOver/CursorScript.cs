using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CursorScript : MonoBehaviour
{
    public float screenDist;
    public bool locked = false;

    [SerializeField] private Camera CameraTrack;
    [SerializeField] private GameObject BodyMg = null;
    [SerializeField] private JointType TrackedJoint = 0;

    private BodySourceManager BodySrcMg;
    private Body[] bodies;
    private SceneMg sM;
    private float time = 0f;
    private bool pc;

    private const float ScreenShift = 0.4f;
    void Start()
    {
        if (BodyMg == null) Debug.Log("BodyMg is empty!");
        else BodySrcMg = BodyMg.GetComponent<BodySourceManager>();

        pc = GameObject.Find("Data").GetComponent<Data>().Pc;
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
        if (pc)
        {
            Vector3 local = Input.mousePosition; local.z = screenDist;
            transform.position = CameraTrack.ScreenToWorldPoint(local);
        }
        else
            TrackJoint();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        time += Time.deltaTime;
        collision.GetComponent<Image>().color -= new Color(0, 0, 0, Time.deltaTime);
        if (!locked && time >= 1f)
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
                collision.GetComponent<Button>().onClick.Invoke();

            if (collision.name == "Exit")
                sM.FadeToScene(0);
            if (collision.name == "Retry")
                sM.FadeToScene(1);

            locked = true;
            time = 0;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        time = 0; locked = false;
        Color local = collision.GetComponent<Image>().color; local.a = 1;
        collision.GetComponent<Image>().color = local;
    }
    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.layer == 8 || locked)
            return;

        time += Time.deltaTime;
        collision.GetComponent<Image>().color -= new Color(0, 0, 0, Time.deltaTime);

        if (!locked && time >= 1f)
        {
            Color local = collision.GetComponent<Image>().color; local.a = 1;
            collision.GetComponent<Image>().color = local;
            locked = true;
            time = 0;

            if (SceneManager.GetActiveScene().buildIndex == 0)
                collision.GetComponent<Button>().onClick.Invoke();

            if (collision.name == "Exit")
                sM.FadeToScene(0);
            if (collision.name == "Retry")
                sM.FadeToScene(1);
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.layer == 8)
            return;
        time = 0; locked = false;
        Color local = collision.GetComponent<Image>().color; local.a = 1;
        collision.GetComponent<Image>().color = local;
    }
    private void TrackJoint()
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
                Vector3 ScreenPos = CameraTrack.ScreenToWorldPoint(new Vector3(newX, newY, screenDist));
                transform.DOMove(ScreenPos, 0.1f);
            }

        }
    }
}
