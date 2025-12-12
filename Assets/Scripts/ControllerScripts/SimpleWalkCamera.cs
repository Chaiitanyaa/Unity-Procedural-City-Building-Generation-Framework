using UnityEngine;

/// <summary>
/// Very simple keyboard-driven camera controller for exploring the city.
/// WASD - move horizontally
/// Q/E - move down/up (like flying)
/// Arrows - rotate view (left/right = yaw, up/down = pitch)
/// Hold Left Shift - move faster
/// </summary>
[RequireComponent(typeof(Camera))]
public class SimpleWalkCamera : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float fastMultiplier = 3f;

    [Header("Look")]
    public float lookSpeedDegreesPerSecond = 90f;
    public float minPitch = -80f;
    public float maxPitch = 80f;

    void Update()
    {
        // --- Movement ---

        float speed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            speed *= fastMultiplier;
        }

        // WASD for horizontal, Q/E for vertical
        float moveX = 0f;
        if (Input.GetKey(KeyCode.D)) moveX += 1f;
        if (Input.GetKey(KeyCode.A)) moveX -= 1f;

        float moveZ = 0f;
        if (Input.GetKey(KeyCode.W)) moveZ += 1f;
        if (Input.GetKey(KeyCode.S)) moveZ -= 1f;

        float moveY = 0f;
        if (Input.GetKey(KeyCode.E)) moveY += 1f;
        if (Input.GetKey(KeyCode.Q)) moveY -= 1f;

        Vector3 localMove = new Vector3(moveX, moveY, moveZ);
        // normalise so diagonal isn’t faster
        localMove = Vector3.ClampMagnitude(localMove, 1f);

        transform.position += transform.TransformDirection(localMove) * speed * Time.deltaTime;

        // --- Rotation ---

        float yawInput = 0f;
        if (Input.GetKey(KeyCode.RightArrow)) yawInput += 1f;
        if (Input.GetKey(KeyCode.LeftArrow))  yawInput -= 1f;

        float pitchInput = 0f;
        if (Input.GetKey(KeyCode.UpArrow))   pitchInput += 1f;
        if (Input.GetKey(KeyCode.DownArrow)) pitchInput -= 1f;

        float yawDelta = yawInput * lookSpeedDegreesPerSecond * Time.deltaTime;
        float pitchDelta = pitchInput * lookSpeedDegreesPerSecond * Time.deltaTime;

        Vector3 euler = transform.eulerAngles;
        // Unity stores angles 0–360; convert pitch to -180..180 for clamping
        float pitch = euler.x;
        if (pitch > 180f) pitch -= 360f;

        pitch = Mathf.Clamp(pitch + pitchDelta, minPitch, maxPitch);
        float yaw = euler.y + yawDelta;

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}