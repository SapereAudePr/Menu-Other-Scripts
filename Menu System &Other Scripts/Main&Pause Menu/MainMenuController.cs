using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer audioMixer;
    public string masterVolumeParam = "MasterVolume";
    public string musicVolumeParam = "MusicVolume";
    public string sfxVolumeParam = "SFXVolume";

    [Header("Volume Setting")]
    public TMP_Text volumeTextValue = null;
    public Slider volumeSlider = null;

    [Header("Music Volume")]
    public TMP_Text musicVolumeTextValue = null;
    public Slider musicVolumeSlider = null;

    [Header("SFX Volume")]
    public TMP_Text sfxVolumeTextValue = null;
    public Slider sfxVolumeSlider = null;

    [Header("Gameplay Settings")]
    public TMP_Text controllerSenTextValue = null;
    public Slider controllerSenSlider = null;
    public int mainControllerSen = 4;

    [Header("Toggle Settings")]
    public Toggle invertYToggle = null;

    [Header("Graphic Settings")]
    public Slider brightnessSlider = null;
    public TMP_Text brightnessTextValue = null;

    [Space(10)]
    public TMP_Dropdown qualityDropdown = null;

    [Header("Resolution Dropdowns")]
    public TMP_Dropdown resolutionDropdown = null;

    [Header("Display Dropdowns")]
    public TMP_Dropdown displayDropdown = null;

    [Header("Aspect Ratio Dropdowns")]
    public TMP_Dropdown aspectRatioDropdown = null;

    [Header("Screen Mode Dropdowns")]
    public TMP_Dropdown screenModeDropdown = null;

    [Header("Confirmation Prompt")]
    public GameObject confirmationPrompt = null;

    [Header("Levels To Load")]
    public string newGameLevel = "Prologue";
    public GameObject noSavedGameDialog = null;

    private void Start()
    {
        AssignUIElements();
        CentralSettingsManager.instance.InitializeUI();
        LoadSettings();
    }

    private void AssignUIElements()
    {
        CentralSettingsManager.instance.resolutionDropdown = resolutionDropdown;
        CentralSettingsManager.instance.displayDropdown = displayDropdown;
        CentralSettingsManager.instance.aspectRatioDropdown = aspectRatioDropdown;
        CentralSettingsManager.instance.screenModeDropdown = screenModeDropdown;
        CentralSettingsManager.instance.qualityDropdown = qualityDropdown;
        CentralSettingsManager.instance.volumeSlider = volumeSlider;
        CentralSettingsManager.instance.volumeTextValue = volumeTextValue;
        CentralSettingsManager.instance.musicVolumeSlider = musicVolumeSlider;
        CentralSettingsManager.instance.musicVolumeTextValue = musicVolumeTextValue;
        CentralSettingsManager.instance.sfxVolumeSlider = sfxVolumeSlider;
        CentralSettingsManager.instance.sfxVolumeTextValue = sfxVolumeTextValue;
        CentralSettingsManager.instance.brightnessSlider = brightnessSlider;
        CentralSettingsManager.instance.brightnessTextValue = brightnessTextValue;
        CentralSettingsManager.instance.controllerSenSlider = controllerSenSlider;
        CentralSettingsManager.instance.controllerSenTextValue = controllerSenTextValue;
        CentralSettingsManager.instance.invertYToggle = invertYToggle;
        CentralSettingsManager.instance.confirmationPrompt = confirmationPrompt;
    }

    public void SetResolution(int resolutionIndex) => CentralSettingsManager.instance.SetResolution(resolutionIndex);
    public void SetDisplay(int displayIndex) => CentralSettingsManager.instance.SetDisplay(displayIndex);
    public void SetAspectRatio(int aspectRatioIndex) => CentralSettingsManager.instance.SetAspectRatio(aspectRatioIndex);
    public void SetScreenMode(int screenModeIndex) => CentralSettingsManager.instance.SetScreenMode(screenModeIndex);
    public void SetVolume(float volume) => CentralSettingsManager.instance.SetVolume(volume);
    public void SetMusicVolume(float volume) => CentralSettingsManager.instance.SetMusicVolume(volume);
    public void SetSFXVolume(float volume) => CentralSettingsManager.instance.SetSFXVolume(volume);
    public void SetBrightness(float brightness) => CentralSettingsManager.instance.SetBrightness(brightness);
    public void SetControllerSen(float sensitivity) => CentralSettingsManager.instance.SetControllerSen(sensitivity);
    public void SetInvertY(bool isInverted) => CentralSettingsManager.instance.SetInvertY(isInverted);
    public void SetQuality(int qualityIndex) => CentralSettingsManager.instance.SetQuality(qualityIndex);

    public void VolumeApply()
    {
        CentralSettingsManager.instance.SaveVolumeSettings(volumeSlider, musicVolumeSlider, sfxVolumeSlider);
        StartCoroutine(ConfirmationBox());
    }

    public void GameplayApply()
    {
        CentralSettingsManager.instance.SaveGameplaySettings(controllerSenSlider, invertYToggle);
        StartCoroutine(ConfirmationBox());
    }

    public void GraphicsApply()
    {
        CentralSettingsManager.instance.SaveGraphicsSettings(brightnessSlider, qualityDropdown, screenModeDropdown);
        StartCoroutine(ConfirmationBox());
    }

    public void ResetButton(string menuType)
    {
        CentralSettingsManager.instance.ResetSettings(menuType);
        StartCoroutine(ConfirmationBox());
    }

    private IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }

    public void NewGameDialogYes() => SceneManager.LoadScene(newGameLevel);
    public void LoadGameDialogYes()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            string levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            noSavedGameDialog.SetActive(true);
        }
    }

    public void ExitButton() => Application.Quit();

    private void LoadSettings()
    {
        CentralSettingsManager.instance.LoadSettings();
    }

    void OnEnable()
    {
        volumeSlider.onValueChanged.AddListener(SetVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        brightnessSlider.onValueChanged.AddListener(SetBrightness);
        controllerSenSlider.onValueChanged.AddListener(SetControllerSen);
        invertYToggle.onValueChanged.AddListener(SetInvertY);
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        screenModeDropdown.onValueChanged.AddListener(SetScreenMode);
        aspectRatioDropdown.onValueChanged.AddListener(SetAspectRatio);
    }

    void OnDisable()
    {
        volumeSlider.onValueChanged.RemoveAllListeners();
        musicVolumeSlider.onValueChanged.RemoveAllListeners();
        sfxVolumeSlider.onValueChanged.RemoveAllListeners();
        brightnessSlider.onValueChanged.RemoveAllListeners();
        controllerSenSlider.onValueChanged.RemoveAllListeners();
        invertYToggle.onValueChanged.RemoveAllListeners();
        resolutionDropdown.onValueChanged.RemoveAllListeners();
        screenModeDropdown.onValueChanged.RemoveAllListeners();
        aspectRatioDropdown.onValueChanged.RemoveAllListeners();
    }
}
