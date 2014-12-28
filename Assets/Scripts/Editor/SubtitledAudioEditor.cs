using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using DeceptionNarrative;

/// <summary>
/// Custom Inspector for SubtitledAudio scriptable object.
/// Displays multiline text boxes for the lines
/// </summary>
[CustomEditor(typeof(SubtitledAudio))]
public class SubtitledAudioEditor : Editor
{
    private SerializedProperty audioClip = null;
    private SerializedProperty subtitles = null;

    private bool subtitlesFoldout;  // state of the subtitles array foldout
    private bool[] subLineFoldouts; // states of individual subtitle line foldouts

    private const string subLinesCountName = "LinesCount";
    private int newSubSize;
    private bool linesCountHadControlLastFrame;

    private void OnEnable()
    {
        // Get audio clip property
        audioClip = serializedObject.FindProperty("audioClip");
        // Get subtitles array property
        subtitles = serializedObject.FindProperty("subtitles");
        // Set subtitle foldout size to current array size
        subLineFoldouts = new bool[subtitles.arraySize];

        // Unfold by default
        subtitlesFoldout = true;
        for (int i = 0; i < subLineFoldouts.Length; i++)
        {
            subLineFoldouts[i] = true;
        }

        newSubSize = subtitles.arraySize;
    }

    public override void OnInspectorGUI()
    {
        // Display Audio Clip field
        EditorGUILayout.PropertyField(audioClip);

        // Display as foldout section
        subtitlesFoldout = EditorGUILayout.Foldout(subtitlesFoldout, "Subtitles");
        if (subtitlesFoldout)
        {
            // Array size field
            GUILayout.BeginHorizontal();
            GUILayout.Label("Elements: ");
            GUI.SetNextControlName(subLinesCountName);
            newSubSize = EditorGUILayout.IntField(newSubSize);

            //If the Elements filed has focus and user presses return, apply the new size to the array
            // This prevents the array from being destroyed because user cleared to field to type something new in
            if (GUI.GetNameOfFocusedControl() == subLinesCountName) 
            {
                if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Return)
                {
                    subtitles.arraySize = newSubSize;
                    UpdateSubtitleFoldouts();
                }

                linesCountHadControlLastFrame = true;
            }
            else if(linesCountHadControlLastFrame)
            {
                newSubSize = subtitles.arraySize;
                linesCountHadControlLastFrame = false;
            }
            GUILayout.EndHorizontal();

            // Display each element in a foldout
            for (int i = 0; i < subLineFoldouts.Length; i++)
            {
                try
                {
                    subLineFoldouts[i] = EditorGUILayout.Foldout(subLineFoldouts[i], string.Format("Line {0}", i + 1));
                    if (subLineFoldouts[i])
                    {
                        // Get the current array element
                        SerializedProperty subtitleLine = subtitles.GetArrayElementAtIndex(i);

                        // Display Time field
                        SerializedProperty time = subtitleLine.FindPropertyRelative("time");
                        EditorGUILayout.PropertyField(time);

                        // Display text field as text area (multiline)
                        SerializedProperty text = subtitleLine.FindPropertyRelative("text");
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Text: ");
                        text.stringValue = EditorGUILayout.TextArea(text.stringValue, GUILayout.MaxWidth(300));
                        GUILayout.EndHorizontal();

                        GUILayout.Space(10);
                    }
                }
                catch(Exception)
                {
                    // When upsizing the array unity internals throw a non-fatal exception. This will prevent it from printing
                }
            }
        }



        serializedObject.ApplyModifiedProperties();
    }

    private void UpdateSubtitleFoldouts()
    {
        // If the array size changed, update the foldouts array
        if(subLineFoldouts.Length != subtitles.arraySize)
        {
            Array.Resize(ref subLineFoldouts, subtitles.arraySize);
        }
    }
}
