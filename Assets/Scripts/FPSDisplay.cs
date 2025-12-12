using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    float smoothTime;

    void Update()
    {
        smoothTime += (Time.unscaledDeltaTime - smoothTime) * 0.1f;
    }

    void OnGUI()
    {
        float fps = 1f / smoothTime;
        GUI.Label(new Rect(10, 10, 150, 30), $"FPS: {fps:0}");
    }
}