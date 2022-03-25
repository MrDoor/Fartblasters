using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;

public class ReadInputManager
{
    // static UnityEngine.Object inputManager;
    // static SerializedObject obj;
    // static SerializedProperty axisArray;
    // static List<string> names;

    // public static void ReadAxes()
    // {
    //     var inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];
    //     SerializedObject obj = new SerializedObject(inputManager);
    //     SerializedProperty axisArray = obj.FindProperty("m_Axes");

    //     if (axisArray.arraySize == 0)
    //         Debug.Log("No Axes");

    //     for (int i = 0; i < axisArray.arraySize; ++i)
    //     {
    //         var axis = axisArray.GetArrayElementAtIndex(i);

    //         var name = axis.FindPropertyRelative("m_Name").stringValue;
    //         var axisVal = axis.FindPropertyRelative("axis").intValue;
    //         var inputType = (InputType)axis.FindPropertyRelative("type").intValue;

    //         Debug.Log($"name: {name}\taxisVal: {axisVal}\tinputType: {inputType}");
    //     }
    // }

    // public static async void CheckIsPressed(bool showAll = false)
    // {
    //     bool inputManagerExists = CheckInputManagerInitialization();
    //     if (inputManagerExists)
    //     {
    //         Debug.Log("Could not load Input Manager.");
    //         return;
    //     }

    //     string str = "\n";
    //     for (int i = 0; i < names.Count; i++)
    //     {
    //         bool pressed = Input.GetButtonDown(names[i]);
    //         if (showAll || pressed)
    //         {
    //             str += $"{names[i]} isPressed: {pressed}\n";
    //         }
    //     }

    //     Debug.Log(str);
    // }

    // public static bool CheckInputManagerInitialization()
    // {
    //     if (ReadInputManager.axisArray == null)
    //     {
    //         ReadInputManager.LoadInputManager();
    //         ReadInputManager.names = new List<string>();
    //         for (int i = 0; i < axisArray.arraySize; ++i)
    //         {
    //             var axis = axisArray.GetArrayElementAtIndex(i);
    //             var name = axis.FindPropertyRelative("m_Name").stringValue;

    //             ReadInputManager.names.Add(name);
    //         }
    //     }
    //     return ReadInputManager.axisArray == null;
    // }

    // public static void LoadInputManager()
    // {
    //     ReadInputManager.inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];
    //     ReadInputManager.obj = new SerializedObject(inputManager);
    //     ReadInputManager.axisArray = obj.FindProperty("m_Axes");
    // }

    // public static void PrintInputVals()
    // {
    //     bool inputManagerExists = CheckInputManagerInitialization();
    //     if (inputManagerExists)
    //     {
    //         Debug.Log("Could not load Input Manager.");
    //         return;
    //     }

    //     string output = "\n";
    //     for (int i = 0; i < axisArray.arraySize; ++i)
    //     {
    //         var axis = axisArray.GetArrayElementAtIndex(i);
    //         var name = axis.FindPropertyRelative("m_Name").stringValue;
    //         var axisVal = axis.FindPropertyRelative("axis").intValue;
    //         var inputType = (InputType)axis.FindPropertyRelative("type").intValue;

    //         output += string.Format("{0, -15} {1, -50} {2, -30} {3}\n", $"input[{i}]:", $"name: {name}", $"axisVal: {axisVal}", $"inputType: {inputType}");
    //     }
    //     Debug.Log(output);
    // }

    // public enum InputType
    // {
    //     KeyOrMouseButton,
    //     MouseMovement,
    //     JoystickAxis,
    // };

    // [MenuItem("Assets/ReadInputManager")]
    // public static void DoRead()
    // {
    //     ReadAxes();
    // }

}