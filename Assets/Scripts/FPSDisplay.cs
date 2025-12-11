using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    private float smoothedDeltaTime = 0.0f;

    void Update()
    {
        float dt = Time.unscaledDeltaTime;
        smoothedDeltaTime += (dt - smoothedDeltaTime) * 0.1f;
    }

    void OnGUI()
    {
        float fps = 1.0f / smoothedDeltaTime;
        GUI.Label(new Rect(10, 10, 150, 40), $"FPS: {fps:0.}");
    }
}