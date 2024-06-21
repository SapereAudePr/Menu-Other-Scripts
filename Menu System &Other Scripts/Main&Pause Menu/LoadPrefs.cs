using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class LoadPrefs : MonoBehaviour
{
    [Header("General Setting")]
    [SerializeField] private bool canUse = false;
    [SerializeField] private MainMenuController menuController;

    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;

    [Header("Music Volume Setting")]
    [SerializeField] private TMP_Text musicVolumeTextValue = null;
    [SerializeField] private Slider musicVolumeSlider = null;

    [Header("SFX Volume Setting")]
    [SerializeField] private TMP_Text sfxVolumeTextValue = null;
    [SerializeField] private Slider sfxVolumeSlider = null;

    [Header("Quality Level Setting")]
    [SerializeField] private TMP_Dropdown qualityDropdown = null;

    [Header("Screen Mode Setting")]
    [SerializeField] private TMP_Dropdown screenModeDropdown = null;

    [Header("Sensitivity Setting")]
    [SerializeField] private TMP_Text controllerSenTextValue = null;
    [SerializeField] private Slider controllerSenSlider = null;

    [Header("Invert Y Setting")]
    [SerializeField] private Toggle invertYToggle = null;

    private void Awake()
    {
        if (canUse)
        {
            StartCoroutine(InitializeWithDelay());
        }
    }

    private IEnumerator InitializeWithDelay()
    {
        yield return new WaitForSeconds(0.1f);

        if (menuController == null)
        {
            Debug.LogError("menuController is not assigned in the Inspector.");
            yield break;
        }

        menuController.ResetButton("Audio");
        menuController.ResetButton("Graphics");
        menuController.ResetButton("Gameplay");

        LoadVolumeSettings();
        LoadMusicVolumeSettings();
        LoadSFXVolumeSettings();
        LoadQualitySettings();
        LoadScreenModeSettings();
        LoadSensitivitySettings();
        LoadInvertYSettings();
    }

    private void LoadScreenModeSettings()
    {
        if (PlayerPrefs.HasKey("screenModeIndex"))
        {
            int screenModeIndex = PlayerPrefs.GetInt("screenModeIndex");
            if (screenModeDropdown != null)
            {
                screenModeDropdown.value = screenModeIndex;
                FullScreenMode screenMode;
                switch (screenModeIndex)
                {
                    case 0:
                        screenMode = FullScreenMode.ExclusiveFullScreen;
                        break;
                    case 1:
                        screenMode = FullScreenMode.FullScreenWindow;
                        break;
                    case 2:
                        screenMode = FullScreenMode.Windowed;
                        break;
                    default:
                        screenMode = FullScreenMode.FullScreenWindow;
                        break;
                }
                Screen.fullScreenMode = screenMode;
            }
        }
    }

    private void LoadVolumeSettings()
    {
        if (PlayerPrefs.HasKey("masterVolume"))
        {
            float localVolume = PlayerPrefs.GetFloat("masterVolume");
            if (volumeTextValue != null) volumeTextValue.text = localVolume.ToString("0.0");
            if (volumeSlider != null) volumeSlider.value = localVolume;
            if (menuController != null) menuController.SetVolume(localVolume);
        }
    }

    private void LoadMusicVolumeSettings()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            float localMusicVolume = PlayerPrefs.GetFloat("musicVolume");
            if (musicVolumeTextValue != null) musicVolumeTextValue.text = localMusicVolume.ToString("0.0");
            if (musicVolumeSlider != null) musicVolumeSlider.value = localMusicVolume;
            if (menuController != null) menuController.SetMusicVolume(localMusicVolume);
        }
    }

    private void LoadSFXVolumeSettings()
    {
        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            float localSFXVolume = PlayerPrefs.GetFloat("sfxVolume");
            if (sfxVolumeTextValue != null) sfxVolumeTextValue.text = localSFXVolume.ToString("0.0");
            if (sfxVolumeSlider != null) sfxVolumeSlider.value = localSFXVolume;
            if (menuController != null) menuController.SetSFXVolume(localSFXVolume);
        }
    }

    private void LoadQualitySettings()
    {
        if (PlayerPrefs.HasKey("masterQuality"))
        {
            int localQuality = PlayerPrefs.GetInt("masterQuality");
            if (qualityDropdown != null)
            {
                qualityDropdown.value = localQuality;
                QualitySettings.SetQualityLevel(localQuality);
            }
        }
    }

    private void LoadSensitivitySettings()
    {
        if (PlayerPrefs.HasKey("masterSen"))
        {
            float localSensitivity = PlayerPrefs.GetFloat("masterSen");
            if (controllerSenTextValue != null) controllerSenTextValue.text = localSensitivity.ToString("0");
            if (controllerSenSlider != null) controllerSenSlider.value = localSensitivity;
            if (CustomInputManager.instance != null) CustomInputManager.instance.sensitivity = localSensitivity;
        }
    }

    private void LoadInvertYSettings()
    {
        if (PlayerPrefs.HasKey("masterInvertY"))
        {
            bool isInverted = PlayerPrefs.GetInt("masterInvertY") == 1;
            if (invertYToggle != null) invertYToggle.isOn = isInverted;
            if (CustomInputManager.instance != null) CustomInputManager.instance.invertY = isInverted;
        }
    }
}
