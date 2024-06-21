using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomInputManager))]
public class CustomInputManagerEditor : Editor
{
    SerializedProperty sensitivity;
    SerializedProperty invertY;
    SerializedProperty playerController;

    private void OnEnable()
    {
        sensitivity = serializedObject.FindProperty("sensitivity");
        invertY = serializedObject.FindProperty("invertY");
        playerController = serializedObject.FindProperty("playerController");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(sensitivity, new GUIContent("Sensitivity"));
        EditorGUILayout.PropertyField(invertY, new GUIContent("Invert Y Axis"));
        EditorGUILayout.PropertyField(playerController, new GUIContent("Player Controller"));

        serializedObject.ApplyModifiedProperties();
    }
}
