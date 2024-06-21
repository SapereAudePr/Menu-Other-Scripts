using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Slider loadingSlider;  // Reference to the UI Slider

    void OnEnable()
    {
        StartCoroutine(LoadSceneAsync("Prologue"));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        // Start loading the scene
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        // Ensure the scene does not activate immediately
        asyncOperation.allowSceneActivation = false;

        // While the scene is still loading
        while (!asyncOperation.isDone)
        {
            // Update the slider's value to the progress (0 to 0.9)
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            loadingSlider.value = progress;

            // Check if the loading is complete (progress is 0.9)
            if (asyncOperation.progress >= 0.9f)
            {
                // Update the slider to full
                loadingSlider.value = 1f;

                // Activate the scene
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
