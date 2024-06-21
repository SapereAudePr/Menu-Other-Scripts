using UnityEngine;

public class CameraActivationHandler : MonoBehaviour
{
    private AudioListener audioListener;

    private void Awake()
    {
        audioListener = GetComponent<AudioListener>();
        if (audioListener == null)
        {
            audioListener = gameObject.AddComponent<AudioListener>();
        }
    }

    private void OnEnable()
    {
        EnableAudioListener();
    }

    private void OnDisable()
    {
        DisableAudioListener();
    }

    private void EnableAudioListener()
    {
        if (audioListener != null)
        {
            audioListener.enabled = true;
        }

        CameraActivationHandler[] handlers = FindObjectsOfType<CameraActivationHandler>();
        foreach (CameraActivationHandler handler in handlers)
        {
            if (handler != this && handler.audioListener != null)
            {
                handler.audioListener.enabled = false;
            }
        }
    }

    private void DisableAudioListener()
    {
        if (audioListener != null)
        {
            audioListener.enabled = false;
        }
    }
}
