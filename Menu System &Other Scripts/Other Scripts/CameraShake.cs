using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 1f; // Duration of the shake effect
    public float shakeMagnitude = 0.7f; // Magnitude of the shake effect

    private Vector3 originalPos; // Original position of the camera

    void Start()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            // Shake the camera
            transform.localPosition = originalPos + Random.insideUnitSphere * shakeMagnitude;

            // Decrease shake duration over time
            shakeDuration -= Time.deltaTime * 1.5f;

            // Reset the camera position when the shake duration is over
            if (shakeDuration <= 0)
            {
                transform.localPosition = originalPos;
            }
        }
    }

    // Call this function to start the shake effect
    public void StartShake(float duration = -1f)
    {
        // Use either the custom duration provided or the default duration, whichever is greater
        shakeDuration = Mathf.Max(duration, shakeDuration);
    }
}
