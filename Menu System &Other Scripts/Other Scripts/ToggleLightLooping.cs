using UnityEngine;
using System.Collections;

public class ToggleLightLooping : MonoBehaviour
{
    // Reference to the Light component
    private Light helicopterLight;

    [SerializeField] float lightStartDelay = 10f;
    public float delayBeforeEnable = 1f; // Delay before the light is enabled again
    public float enableTime = 1f; // Duration for which the light stays enabled

    void Start()
    {
        // Get the Light component attached to the helicopter
        helicopterLight = GetComponent<Light>();
        helicopterLight.enabled = false;

        // Check if there's a Light component attached
        if (helicopterLight == null)
        {
            return;
        }

        // Start the toggle coroutine after 10 seconds
        StartCoroutine(StartToggleLightLoopingWithDelay());
    }

    // Coroutine to start light looping after a delay
    IEnumerator StartToggleLightLoopingWithDelay()
    {
        // Wait for 10 seconds before starting the loop
        yield return new WaitForSeconds(lightStartDelay);

        // Start the toggle coroutine
        StartCoroutine(ToggleLightLoopingWithDelay());
    }

    // Coroutine to toggle light looping with separate delay and enable time
    IEnumerator ToggleLightLoopingWithDelay()
    {
        while (true)
        {
            // Enable the light
            helicopterLight.enabled = true;

            // Wait for the specified enable time
            yield return new WaitForSeconds(enableTime);

            // Disable the light
            helicopterLight.enabled = false;

            // Wait for the specified delay before enabling the light again
            yield return new WaitForSeconds(delayBeforeEnable);
        }
    }
}
