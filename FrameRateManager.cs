using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class FrameRateManager : MonoBehaviour
{
    public int MaxFrames = 60;  //maximum frames to average over

    private static float lastFPSCalculated = 0f;
    private List<float> frameTimes = new List<float>();
    //[Header("Frame Settings")]
    //int MaxRate = 9999;
    //public float TargetFrameRate = 60.0f;
    float currentFrameTime;
    public TextMeshProUGUI fpsCounter;
    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = MaxFrames;
        currentFrameTime = Time.realtimeSinceStartup;
        StartCoroutine("WaitForNextFrame");
    }

    IEnumerator WaitForNextFrame()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            currentFrameTime += 1.0f / MaxFrames;
            var t = Time.realtimeSinceStartup;
            var sleepTime = currentFrameTime - t - 0.01f;
            if (sleepTime > 0)
                Thread.Sleep((int)(sleepTime * 1000));
            while (t < currentFrameTime)
                t = Time.realtimeSinceStartup;
        }
    }

    // Use this for initialization
    void Start()
    {
        lastFPSCalculated = 0f;
        frameTimes.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        fpsCounter.text = "Current FPS: " + GetCurrentFPS();
        addFrame();
        lastFPSCalculated = calculateFPS();
    }

    private void addFrame()
    {
        frameTimes.Add(Time.unscaledDeltaTime);
        if (frameTimes.Count > MaxFrames)
        {
            frameTimes.RemoveAt(0);
        }
    }

    private float calculateFPS()
    {
        float newFPS = 0f;

        float totalTimeOfAllFrames = 0f;
        foreach (float frame in frameTimes)
        {
            totalTimeOfAllFrames += frame;
        }
        newFPS = ((float)(frameTimes.Count)) / totalTimeOfAllFrames;

        return newFPS;
    }

    public static float GetCurrentFPS()
    {
        return lastFPSCalculated;
    }
}