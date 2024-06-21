using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class CentralSettingsManager : MonoBehaviour
{
    public static CentralSettingsManager instance;

    [Header("Resolution Dropdowns")]
    public TMP_Dropdown resolutionDropdown;

    [Header("Display Dropdowns")]
    public TMP_Dropdown displayDropdown;

    [Header("Aspect Ratio Dropdowns")]
    public TMP_Dropdown aspectRatioDropdown;

    [Header("Screen Mode Dropdowns")]
    public TMP_Dropdown screenModeDropdown;

    [Space(10)]
    [Header("Quality Dropdowns")]
    public TMP_Dropdown qualityDropdown;

    [Header("Volume Setting")]
    public TMP_Text volumeTextValue = null;
    public Slider volumeSlider = null;

    [Header("Music Volume")]
    public TMP_Text musicVolumeTextValue = null;
    public Slider musicVolumeSlider = null;

    [Header("SFX Volume")]
    public TMP_Text sfxVolumeTextValue = null;
    public Slider sfxVolumeSlider = null;

    [Header("Graphic Settings")]
    public Slider brightnessSlider = null;
    public TMP_Text brightnessTextValue = null;

    [Header("Gameplay Settings")]
    public TMP_Text controllerSenTextValue = null;
    public Slider controllerSenSlider = null;
    public Toggle invertYToggle = null;

    [Header("Confirmation Prompt")]
    public GameObject confirmationPrompt = null;



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
        }
    }

    public void InitializeUI()
    {
        InitializeResolutionOptions();
        InitializeDisplayOptions();
        InitializeAspectRatioOptions();
        InitializeScreenModeOptions();
        InitializeQualityOptions();
    }

    private void InitializeResolutionOptions()
    {
        if (resolutionDropdown == null) return;
        resolutionDropdown.ClearOptions();
        Resolution[] resolutions = Screen.resolutions;
        HashSet<string> uniqueResolutions = new HashSet<string>();

        foreach (Resolution res in resolutions)
        {
            string option = $"{res.width} x {res.height}";
            uniqueResolutions.Add(option);
        }

        List<string> options = uniqueResolutions.ToList();
        options.Sort();

        resolutionDropdown.AddOptions(options);

        string currentResolution = $"{Screen.currentResolution.width} x {Screen.currentResolution.height}";
        int defaultIndex = options.IndexOf(currentResolution);
        resolutionDropdown.value = defaultIndex >= 0 ? defaultIndex : 0;
        resolutionDropdown.RefreshShownValue();
    }

    private void InitializeDisplayOptions()
    {
        if (displayDropdown == null) return;
        displayDropdown.ClearOptions();
        List<string> options = new List<string>();

        for (int i = 0; i < Display.displays.Length; i++)
        {
            string displayName = $"Display {i + 1}";
            if (i == 0) // Primary display
            {
                int refreshRate = (int)(Screen.currentResolution.refreshRateRatio.numerator / (int)Screen.currentResolution.refreshRateRatio.denominator);
                displayName = $"{Screen.currentResolution.width} x {Screen.currentResolution.height} - {refreshRate} Hz";
            }
            else // Other displays
            {
                Display.displays[i].Activate();
                Resolution[] resolutions = Screen.resolutions;
                if (resolutions.Length > 0)
                {
                    Resolution res = resolutions[0]; // Use the first resolution as an example
                    int refreshRate = (int)(res.refreshRateRatio.numerator / (int)res.refreshRateRatio.denominator);
                    displayName = $"{res.width} x {res.height} - {refreshRate} Hz";
                }
            }
            options.Add(displayName);
        }

        displayDropdown.AddOptions(options);
        displayDropdown.value = PlayerPrefs.GetInt("displayIndex", 0);
        displayDropdown.RefreshShownValue();
    }

    private void InitializeAspectRatioOptions()
    {
        if (aspectRatioDropdown == null) return;
        aspectRatioDropdown.ClearOptions();
        List<string> options = new List<string>
        {
            "16:9",
            "16:10",
            "21:9",
            "4:3",
            "5:4"
        };

        aspectRatioDropdown.AddOptions(options);
        aspectRatioDropdown.value = PlayerPrefs.GetInt("aspectRatioIndex", options.IndexOf("16:9"));
        aspectRatioDropdown.RefreshShownValue();
    }

    private void InitializeScreenModeOptions()
    {
        if (screenModeDropdown == null) return;
        screenModeDropdown.ClearOptions();
        List<string> options = new List<string>
        {
            "Fullscreen",
            "Borderless Window",
            "Windowed"
        };

        screenModeDropdown.AddOptions(options);
        screenModeDropdown.value = PlayerPrefs.GetInt("screenModeIndex", 0);
        screenModeDropdown.RefreshShownValue();
    }

    private void InitializeQualityOptions()
    {
        if (qualityDropdown == null) return;
        qualityDropdown.ClearOptions();
        List<string> options = new List<string>
        {
            "Low",
            "Medium",
            "High",
            "Ultra"
        };

        qualityDropdown.AddOptions(options);
        qualityDropdown.value = PlayerPrefs.GetInt("qualityIndex", 2); // Default to Medium
        qualityDropdown.RefreshShownValue();
    }

    // Settings-related methods
    public void SetResolution(int resolutionIndex)
    {
        if (resolutionDropdown == null) return;
        List<string> options = resolutionDropdown.options.ConvertAll(option => option.text);
        string[] dimensions = options[resolutionIndex].Split('x');
        int width = int.Parse(dimensions[0].Trim());
        int height = int.Parse(dimensions[1].Trim());

        Screen.SetResolution(width, height, Screen.fullScreenMode);

        PlayerPrefs.SetInt("resolutionIndex", resolutionIndex);
        PlayerPrefs.SetInt("resolutionWidth", width);
        PlayerPrefs.SetInt("resolutionHeight", height);
        PlayerPrefs.Save();
    }

    public void SetDisplay(int displayIndex)
    {
        if (displayDropdown == null) return;
        if (displayIndex < Display.displays.Length)
        {
            Display.displays[displayIndex].Activate();
            PlayerPrefs.SetInt("displayIndex", displayIndex);
            PlayerPrefs.Save();
        }
    }

    public void SetAspectRatio(int aspectRatioIndex)
    {
        if (aspectRatioDropdown == null) return;
        string aspectRatio = aspectRatioDropdown.options[aspectRatioIndex].text;
        float width = 16f, height = 9f;  // Default to 16:9 if an unknown aspect ratio is selected

        switch (aspectRatio)
        {
            case "16:9":
                width = 16f;
                height = 9f;
                break;
            case "16:10":
                width = 16f;
                height = 10f;
                break;
            case "21:9":
                width = 21f;
                height = 9f;
                break;
            case "4:3":
                width = 4f;
                height = 3f;
                break;
            case "5:4":
                width = 5f;
                height = 4f;
                break;
        }

        float aspectRatioValue = width / height;
        PlayerPrefs.SetInt("aspectRatioIndex", aspectRatioIndex);
        PlayerPrefs.SetFloat("aspectRatioValue", aspectRatioValue);
        PlayerPrefs.Save();
    }

    public void SetScreenMode(int screenModeIndex)
    {
        if (screenModeDropdown == null) return;
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
        PlayerPrefs.SetInt("screenModeIndex", screenModeIndex);
        PlayerPrefs.Save();
    }

    public void SetVolume(float volume)
    {
        if (volumeSlider == null) return;
        float dB = volume <= 0.0f ? -80f : Mathf.Log10(volume) * 20;
        SettingsManager.instance.audioMixer.SetFloat("MasterVolume", dB);
        volumeTextValue.text = volume.ToString("0.0");
    }

    public void SetMusicVolume(float volume)
    {
        if (musicVolumeSlider == null) return;
        float dB = volume <= 0.0f ? -80f : Mathf.Log10(volume) * 20;
        SettingsManager.instance.audioMixer.SetFloat("MusicVolume", dB);
        musicVolumeTextValue.text = volume.ToString("0.0");
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxVolumeSlider == null) return;
        float dB = volume <= 0.0f ? -80f : Mathf.Log10(volume) * 20;
        SettingsManager.instance.audioMixer.SetFloat("SFXVolume", dB);
        sfxVolumeTextValue.text = volume.ToString("0.0");
    }

    public void SetBrightness(float brightness)
    {
        if (brightnessSlider == null) return;
        brightnessTextValue.text = brightness.ToString("0.0");
        SettingsManager.instance.SetBrightness(brightness);
    }

    public void SetControllerSen(float sensitivity)
    {
        if (controllerSenSlider == null) return;
        controllerSenTextValue.text = sensitivity.ToString("0");
        CustomInputManager.instance.sensitivity = sensitivity;
    }

    public void SetInvertY(bool isInverted)
    {
        if (invertYToggle == null) return;
        CustomInputManager.instance.invertY = isInverted;
    }

    public void SetQuality(int qualityIndex)
    {
        if (qualityDropdown == null) return;
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("qualityIndex", qualityIndex);
        PlayerPrefs.Save();
    }

    public void SaveVolumeSettings(Slider volumeSlider, Slider musicVolumeSlider, Slider sfxVolumeSlider)
    {
        SetVolume(volumeSlider.value);
        SetMusicVolume(musicVolumeSlider.value);
        SetSFXVolume(sfxVolumeSlider.value);
        PlayerPrefs.SetFloat("masterVolume", volumeSlider.value);
        PlayerPrefs.SetFloat("musicVolume", musicVolumeSlider.value);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolumeSlider.value);
        PlayerPrefs.Save();
    }

    public void SaveGameplaySettings(Slider controllerSenSlider, Toggle invertYToggle)
    {
        SetControllerSen(controllerSenSlider.value);
        SetInvertY(invertYToggle.isOn);
        PlayerPrefs.SetFloat("controllerSensitivity", controllerSenSlider.value);
        PlayerPrefs.SetInt("invertY", invertYToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SaveGraphicsSettings(Slider brightnessSlider, TMP_Dropdown qualityDropdown, TMP_Dropdown screenModeDropdown)
    {
        SetBrightness(brightnessSlider.value);
        SetQuality(qualityDropdown.value);
        SetScreenMode(screenModeDropdown.value);
    }

    public void ResetSettings(string menuType)
    {
        switch (menuType)
        {
            case "Audio":
                volumeSlider.value = SettingsManager.instance.DefaultMasterVolume;
                musicVolumeSlider.value = SettingsManager.instance.DefaultMusicVolume;
                sfxVolumeSlider.value = SettingsManager.instance.DefaultSFXVolume;
                SaveVolumeSettings(volumeSlider, musicVolumeSlider, sfxVolumeSlider);
                break;
            case "Graphics":
                brightnessSlider.value = SettingsManager.instance.defaultBrightness;
                qualityDropdown.value = 2; // Assume Medium is the default quality
                screenModeDropdown.value = 0; // Assume Fullscreen is the default screen mode
                SaveGraphicsSettings(brightnessSlider, qualityDropdown, screenModeDropdown);
                break;
            case "Gameplay":
                controllerSenSlider.value = SettingsManager.instance.defaultSen;
                invertYToggle.isOn = false; // Assume default is not inverted
                SaveGameplaySettings(controllerSenSlider, invertYToggle);
                break;
            default:
                Debug.LogWarning("Unknown menu type for resetting settings: " + menuType);
                break;
        }
    }


    public void LoadSettings()
    {
        // Load all settings from PlayerPrefs
        // Example:
        volumeSlider.value = PlayerPrefs.GetFloat("masterVolume", 1.0f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume", 1.0f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1.0f);
        controllerSenSlider.value = PlayerPrefs.GetFloat("controllerSensitivity", 4.0f);
        invertYToggle.isOn = PlayerPrefs.GetInt("invertY", 0) == 1;
        brightnessSlider.value = PlayerPrefs.GetFloat("brightness", 1.0f);
        qualityDropdown.value = PlayerPrefs.GetInt("qualityIndex", 2);
        screenModeDropdown.value = PlayerPrefs.GetInt("screenModeIndex", 0);
        // Refresh UI values
    }

    private void OnEnable()
    {
        if (volumeSlider != null)
            volumeSlider.onValueChanged.AddListener(SetVolume);
        if (musicVolumeSlider != null)
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        if (sfxVolumeSlider != null)
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        if (brightnessSlider != null)
            brightnessSlider.onValueChanged.AddListener(SetBrightness);
        if (controllerSenSlider != null)
            controllerSenSlider.onValueChanged.AddListener(SetControllerSen);
        if (invertYToggle != null)
            invertYToggle.onValueChanged.AddListener(SetInvertY);
        if (resolutionDropdown != null)
            resolutionDropdown.onValueChanged.AddListener(SetResolution);
        if (screenModeDropdown != null)
            screenModeDropdown.onValueChanged.AddListener(SetScreenMode);
        if (aspectRatioDropdown != null)
            aspectRatioDropdown.onValueChanged.AddListener(SetAspectRatio);
    }

    private void OnDisable()
    {
        if (volumeSlider != null)
            volumeSlider.onValueChanged.RemoveListener(SetVolume);
        if (musicVolumeSlider != null)
            musicVolumeSlider.onValueChanged.RemoveListener(SetMusicVolume);
        if (sfxVolumeSlider != null)
            sfxVolumeSlider.onValueChanged.RemoveListener(SetSFXVolume);
        if (brightnessSlider != null)
            brightnessSlider.onValueChanged.RemoveListener(SetBrightness);
        if (controllerSenSlider != null)
            controllerSenSlider.onValueChanged.RemoveListener(SetControllerSen);
        if (invertYToggle != null)
            invertYToggle.onValueChanged.RemoveListener(SetInvertY);
        if (resolutionDropdown != null)
            resolutionDropdown.onValueChanged.RemoveListener(SetResolution);
        if (screenModeDropdown != null)
            screenModeDropdown.onValueChanged.RemoveListener(SetScreenMode);
        if (aspectRatioDropdown != null)
            aspectRatioDropdown.onValueChanged.RemoveListener(SetAspectRatio);
    }
}
