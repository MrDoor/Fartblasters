using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SceneBuilder))]
public class SceneHelper : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SceneBuilder builder = (SceneBuilder)target;

        if (GUILayout.Button("Clear Items"))
        {
            builder.ClearItems();
        }

        GUILayout.FlexibleSpace();
        // EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("Add Scene GameObjects To Array"))
        {
            builder.SearchAndAdd();
        }
        // EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Find Matching Prefabs"))
        {
            builder.SearchAndAddReplacements();
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Perform Swap"))
        {
            builder.SwapObjects();
        }

        if (GUILayout.Button("Perform Swap on Entire Scene"))
        {
            builder.SwapAllSceneObjects();
        }

    }

}
