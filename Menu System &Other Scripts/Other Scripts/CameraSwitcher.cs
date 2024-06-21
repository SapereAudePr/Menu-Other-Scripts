using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera[] cameras;

    private void Start()
    {
        if (cameras == null || cameras.Length == 0)
        {
            Debug.LogError("No cameras assigned to CameraSwitcher.");
            return;
        }
        UpdateCameras();
    }

    public void SetActiveCamera(int index)
    {
        if (cameras == null || index >= cameras.Length)
        {
            Debug.LogError("Camera index out of bounds or cameras array not assigned.");
            return;
        }

        for (int i = 0; i < cameras.Length; i++)
        {
            bool isActive = i == index;
            cameras[i].gameObject.SetActive(isActive);

            AudioListener audioListener = cameras[i].GetComponent<AudioListener>();
            if (audioListener != null)
            {
                audioListener.enabled = isActive;
            }
        }
    }

    private void UpdateCameras()
    {
        if (cameras == null)
        {
            Debug.LogError("Cameras array not assigned.");
            return;
        }

        for (int i = 0; i < cameras.Length; i++)
        {
            bool isActive = cameras[i].gameObject.activeSelf;
            AudioListener audioListener = cameras[i].GetComponent<AudioListener>();
            if (audioListener != null)
            {
                audioListener.enabled = isActive;
            }
        }
    }
}
