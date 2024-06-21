using UnityEngine;
using StarterAssets;

public class CustomInputManager : MonoBehaviour
{
    public static CustomInputManager instance;

    public float sensitivity = 1.0f;
    public bool invertY = false;
    public FirstPersonController playerController;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Update()
    {
        AdjustInputSensitivity();
    }

    private void AdjustInputSensitivity()
    {
        if (playerController != null)
        {
            StarterAssetsInputs inputs = playerController.GetComponent<StarterAssetsInputs>();
            if (inputs != null)
            {
                Vector2 lookInput = inputs.look;
                lookInput.x *= sensitivity;
                lookInput.y *= sensitivity * (invertY ? -1 : 1);
                inputs.look = lookInput;
            }
            else
            {
                Debug.LogWarning($"'StarterAssetsInputs' component not found on {playerController.GetType().Name}.");
            }
        }
    }
}
