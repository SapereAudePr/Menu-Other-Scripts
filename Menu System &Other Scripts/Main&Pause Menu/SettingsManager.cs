using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;

    [Header("Brightness Settings")]
    public Volume globalVolume;
    public float minBrightness = -10.0f;
    public float maxBrightness = 10.0f;
    public float defaultBrightness = 0.0f;

    [Header("Audio Settings")]
    public AudioMixer audioMixer;
    public string masterVolumeParam = "MasterVolume";
    public string musicVolumeParam = "MusicVolume";
    public string sfxVolumeParam = "SFXVolume";

    [Range(0.0f, 2.0f)]
    public float DefaultMasterVolume = 1f;
    [Range(0.0f, 2.0f)]
    public float DefaultMusicVolume = 1f;
    [Range(0.0f, 2.0f)]
    public float DefaultSFXVolume = 1f;

    [Header("Gameplay Settings")]
    public int defaultSen = 4;

    private ColorAdjustments colorAdjustments;

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

        if (globalVolume != null && globalVolume.profile.TryGet(out colorAdjustments))
        {
            float brightness = PlayerPrefs.GetFloat("masterBrightness", defaultBrightness);
            colorAdjustments.postExposure.value = Mathf.Clamp(brightness, minBrightness, maxBrightness);
        }
    }

    public void SetBrightness(float brightness)
    {
        if (colorAdjustments != null)
        {
            colorAdjustments.postExposure.value = Mathf.Clamp(brightness, minBrightness, maxBrightness);
        }
    }

    public void SetVolume(float volume)
    {
        float dB = volume <= 0.0f ? -80f : Mathf.Log10(volume) * 20;
        audioMixer.SetFloat(masterVolumeParam, dB);
    }

    public void SetMusicVolume(float volume)
    {
        float dB = volume <= 0.0f ? -80f : Mathf.Log10(volume) * 20;
        audioMixer.SetFloat(musicVolumeParam, dB);
    }

    public void SetSFXVolume(float volume)
    {
        float dB = volume <= 0.0f ? -80f : Mathf.Log10(volume) * 20;
        audioMixer.SetFloat(sfxVolumeParam, dB);
    }

    void Start()
    {
        DefaultMasterVolume = PlayerPrefs.GetFloat("masterVolume", DefaultMasterVolume);
        DefaultMusicVolume = PlayerPrefs.GetFloat("musicVolume", DefaultMusicVolume);
        DefaultSFXVolume = PlayerPrefs.GetFloat("sfxVolume", DefaultSFXVolume);

        SetVolume(DefaultMasterVolume);
        SetMusicVolume(DefaultMusicVolume);
        SetSFXVolume(DefaultSFXVolume);

        if (globalVolume != null && globalVolume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.postExposure.value = PlayerPrefs.GetFloat("masterBrightness", defaultBrightness);
        }
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("masterVolume", DefaultMasterVolume);
        PlayerPrefs.SetFloat("musicVolume", DefaultMusicVolume);
        PlayerPrefs.SetFloat("sfxVolume", DefaultSFXVolume);
        if (colorAdjustments != null)
            PlayerPrefs.SetFloat("masterBrightness", colorAdjustments.postExposure.value);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        DefaultMasterVolume = PlayerPrefs.GetFloat("masterVolume", DefaultMasterVolume);
        DefaultMusicVolume = PlayerPrefs.GetFloat("musicVolume", DefaultMusicVolume);
        DefaultSFXVolume = PlayerPrefs.GetFloat("sfxVolume", DefaultSFXVolume);

        SetVolume(DefaultMasterVolume);
        SetMusicVolume(DefaultMusicVolume);
        SetSFXVolume(DefaultSFXVolume);

        if (globalVolume != null && globalVolume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.postExposure.value = PlayerPrefs.GetFloat("masterBrightness", defaultBrightness);
        }
    }
}
