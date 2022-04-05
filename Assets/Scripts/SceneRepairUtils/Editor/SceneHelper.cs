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

        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add To Item Array"))
        {
            builder.SearchAndAdd();
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Find New Objects"))
        {
            builder.SearchAndAddReplacements();
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Perform Swap"))
        {
            builder.SwapObjects();
        }

        // if (GUILayout.Button("Organize Game Objects"))
        // {
        //     repairer.DoOrganize();
        // }
    }

}
