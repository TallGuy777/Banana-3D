using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;

public class FPS : MonoBehaviour
{
    int MaxRate = 9999;
    public float TargetFrameRate = 60.0f;
    float currentFrameTime;
    public TextMeshProUGUI fpsText;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = MaxRate;
        currentFrameTime = Time.realtimeSinceStartup;
        StartCoroutine("WaitForNextFrame");
    }
    IEnumerator WaitForNextFrame()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            currentFrameTime += 1.0f / TargetFrameRate;
            var t = Time.realtimeSinceStartup;
            var sleepTime = currentFrameTime - t - 0.01f;
            if (sleepTime > 0)
            {
                Thread.Sleep((int)(sleepTime * 1000));
                while (t < currentFrameTime)
                {
                    t = Time.realtimeSinceStartup;
                }
            }
        }
    }
    void FixedUpdate()
    {
        fpsText.text="FPS: "+TargetFrameRate.ToString();
    }
}
