using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RepairGameObjects))]
public class SceneDoctor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        RepairGameObjects repairer = (RepairGameObjects)target;

        if (GUILayout.Button("Repair"))
        {
            repairer.FixGameObjects();
        }
    }

}
