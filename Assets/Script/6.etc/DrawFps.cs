using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DrawFps : MonoBehaviour {

    private double updateInterval = 1.0;
    private double lastInterval;
    private float frames = 0.0f;
    private string fpsText = "";

    void Start () {
        lastInterval = Time.realtimeSinceStartup;
        frames = 0.0f;
    }

    void Update ()
    {
        ++frames;
        double timeNow = Time.realtimeSinceStartup;

        if (timeNow > lastInterval + updateInterval)
        {
            double fps = frames / (timeNow - lastInterval);
            fpsText = "FPS :" + fps.ToString("f2");
            frames = 0.0f;
            lastInterval = timeNow;
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(300, 20, 200, 30), fpsText);
    }

    public void SetText(string str)
    {
        fpsText = str;
    }
}
