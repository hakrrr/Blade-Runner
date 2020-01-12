using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Windows.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;

public class PlayerController : MonoBehaviour
{
    //private readonly string gestureDb = @"Database\Dodge000";
    private readonly string dodgeL = "Dodge_Left";
    private readonly string dodgeR = "Dodge_Right";

    private KinectSensor sensor;
    private VisualGestureBuilderFrameSource vgbFrameSource = null;
    private VisualGestureBuilderFrameReader vgbFrameReader = null;

    /// <summary>
    /// Init VGB FrameSource and FrameReader. Then load database with streaming asset
    /// Add desired gestures into the FrameSource
    /// </summary>
    private void Start()
    {
        //sensor = KinectSensor.GetDefault();

        //vgbFrameSource = VisualGestureBuilderFrameSource.Create(sensor, 0);
        //vgbFrameReader = vgbFrameSource.OpenReader();

        //if (vgbFrameReader != null) vgbFrameReader.IsPaused = true;

        var gestureDb = Path.Combine(Application.streamingAssetsPath, "Dodge000.gbd");
        using (VisualGestureBuilderDatabase database = VisualGestureBuilderDatabase.Create(gestureDb))
        {
            foreach (Gesture gesture in database.AvailableGestures)
            {
                //vgbFrameSource.AddGesture(gesture);
                Debug.Log("gesture added");
            }
        }
    }


}
