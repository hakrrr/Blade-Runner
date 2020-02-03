using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Windows.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;

public class GestureSourceManager : MonoBehaviour
{
    public delegate void GestureDetected(string name ,float conf);
    public event GestureDetected GestureDetectedEvent;

    [SerializeField] private GameObject bodyMg;
    private BodySourceManager bodySrcMg;
    private KinectSensor sensor;
    private VisualGestureBuilderFrameSource vgbFrameSource = null;
    private VisualGestureBuilderFrameReader vgbFrameReader = null;

    /// <summary>
    /// GestureSourceManager is Publisher for Event: GestureDetected()
    /// Init VGB FrameSource and FrameReader. Reference bodySrcMg to track bodyID
    /// Pause Reader and subscribe to Event. Load database and add to Source
    /// </summary>
    private void Start()
    {
        sensor = KinectSensor.GetDefault();
        bodySrcMg = bodyMg.GetComponent<BodySourceManager>();

        vgbFrameSource = VisualGestureBuilderFrameSource.Create(sensor, 0);
        vgbFrameReader = vgbFrameSource.OpenReader();

        if (vgbFrameReader != null)
        {
            vgbFrameReader.IsPaused = true;
            vgbFrameReader.FrameArrived += GestureFrameArrived;
        }

        var gestureDb = Path.Combine(Application.streamingAssetsPath, "ASG000.gbd");
        using (VisualGestureBuilderDatabase database = VisualGestureBuilderDatabase.Create(gestureDb))
        {
            foreach (Gesture gesture in database.AvailableGestures)
            {
                vgbFrameSource.AddGesture(gesture);
                //Debug.Log(gesture.Name + " successfully added to source");
            }
        }
    }
    private void Update()
    {
        if (!vgbFrameSource.IsTrackingIdValid)
        {
            FindValidBody();
        }
    }
    void FindValidBody()
    {
        if (bodySrcMg != null)
        {
            Body[] bodies = bodySrcMg.GetData();
            if (bodies != null)
            {
                foreach (Body body in bodies)
                {
                    if (body.IsTracked)
                    {
                        SetBody(body.TrackingId);
                        break;
                    }
                }
            }
        }
    }
    public void SetBody(ulong id)
    {
        if (id > 0)
        {
            vgbFrameSource.TrackingId = id;
            vgbFrameReader.IsPaused = false;
        }
        else
        {
            vgbFrameSource.TrackingId = 0;
            vgbFrameReader.IsPaused = true;
        }
    }
    private void GestureFrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs e)
    {
        VisualGestureBuilderFrameReference frameReference = e.FrameReference;
        using(VisualGestureBuilderFrame frame = frameReference.AcquireFrame())
        {
            if(frame != null)
            {
                IDictionary<Gesture, DiscreteGestureResult> discreteResults = frame.DiscreteGestureResults;
                IDictionary<Gesture, ContinuousGestureResult> continResults = frame.ContinuousGestureResults;

                if(discreteResults != null)
                {
                    foreach(Gesture gesture in vgbFrameSource.Gestures)
                    {
                        if(gesture.GestureType == GestureType.Discrete)
                        {
                            DiscreteGestureResult result = null;
                            discreteResults.TryGetValue(gesture, out result);

                            if(result != null)
                                GestureDetectedEvent(gesture.Name, result.Confidence);
                        }

                        if(gesture.GestureType == GestureType.Continuous)
                        {
                            ContinuousGestureResult result = null;
                            continResults.TryGetValue(gesture, out result);

                            if (result != null)
                                GestureDetectedEvent(gesture.Name, result.Progress);
                        }
                    }
                }
            }
        }
    }

}
