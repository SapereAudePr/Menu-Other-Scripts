using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SettingsManager))]
public class SettingsManagerEditor : Editor
{
    SerializedProperty globalVolume;
    SerializedProperty minBrightness;
    SerializedProperty maxBrightness;
    SerializedProperty defaultBrightness;
    SerializedProperty audioMixer;
    SerializedProperty masterVolumeParam;
    SerializedProperty musicVolumeParam;
    SerializedProperty sfxVolumeParam;
    SerializedProperty defaultMasterVolume;
    SerializedProperty defaultMusicVolume;
    SerializedProperty defaultSFXVolume;

    private void OnEnable()
    {
        globalVolume = serializedObject.FindProperty("globalVolume");
        minBrightness = serializedObject.FindProperty("minBrightness");
        maxBrightness = serializedObject.FindProperty("maxBrightness");
        defaultBrightness = serializedObject.FindProperty("defaultBrightness");
        audioMixer = serializedObject.FindProperty("audioMixer");
        masterVolumeParam = serializedObject.FindProperty("masterVolumeParam");
        musicVolumeParam = serializedObject.FindProperty("musicVolumeParam");
        sfxVolumeParam = serializedObject.FindProperty("sfxVolumeParam");
        defaultMasterVolume = serializedObject.FindProperty("DefaultMasterVolume");
        defaultMusicVolume = serializedObject.FindProperty("DefaultMusicVolume");
        defaultSFXVolume = serializedObject.FindProperty("DefaultSFXVolume");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(globalVolume, new GUIContent("Global Volume"));
        EditorGUILayout.PropertyField(minBrightness, new GUIContent("Min Brightness"));
        EditorGUILayout.PropertyField(maxBrightness, new GUIContent("Max Brightness"));
        EditorGUILayout.PropertyField(defaultBrightness, new GUIContent("Default Brightness"));

        EditorGUILayout.PropertyField(audioMixer, new GUIContent("Audio Mixer"));
        EditorGUILayout.PropertyField(masterVolumeParam, new GUIContent("Master Volume Parameter"));
        EditorGUILayout.PropertyField(musicVolumeParam, new GUIContent("Music Volume Parameter"));
        EditorGUILayout.PropertyField(sfxVolumeParam, new GUIContent("SFX Volume Parameter"));

        EditorGUILayout.PropertyField(defaultMasterVolume, new GUIContent("Default Master Volume"));
        EditorGUILayout.PropertyField(defaultMusicVolume, new GUIContent("Default Music Volume"));
        EditorGUILayout.PropertyField(defaultSFXVolume, new GUIContent("Default SFX Volume"));

        serializedObject.ApplyModifiedProperties();
    }
}
