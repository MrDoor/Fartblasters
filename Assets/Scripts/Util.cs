using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Util
{
    public static string level;

    public static GameObject FindByTagName(string tag)
    {
        // GameObject go = GameObject.F
        List<GameObject> transforms = new List<GameObject>();
        Transform[] objects = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objects.Length; i++)
        {
            if (!objects[i].gameObject.active)
            {
                continue;
            }
            if (objects[i].hideFlags == HideFlags.None)
            {
                if (objects[i].gameObject.CompareTag(tag))
                {
                    transforms.Add(objects[i].gameObject);
                }
            }
        }

        if (transforms.Count > 1)
        {
            Debug.LogWarning($"Found too many objects tagged as {tag}");
            string tNames = "";
            foreach (var t in transforms)
            {
                tNames += t.name + "  ";
            }
            Debug.LogWarning($"{tNames}...returning first.");
        }
        else if (transforms.Count <= 0)
        {
            Debug.LogWarning($"No objects tagged as {tag} were found.");
            return null;
        }
        return transforms[0];
    }

    public static GameObject FindOrCreateNew(string name, string tag = "Untagged")
    {
        GameObject go = SafeGameObjectFind(name);
        if (go == null)
        {
            go = new GameObject();
            go.name = name;
            go.tag = tag;
        }

        return go;
    }

    public static GameObject FindOrCreateFromPrefab(string name)
    {
        GameObject go = SafeGameObjectFind(name);
        if (go == null)
        {
            go = Resources.Load("Prefabs/" + name) as GameObject;
        }

        return go;
    }

#if UNITY_EDITOR
    public static List<GameObject> GetAllPrefabs()
    {
        List<GameObject> objects = new List<GameObject>();

        string sAssetFolderPath = "Assets/Resources/Prefabs";
        // Construct the system path of the asset folder 
        string sDataPath = Application.dataPath;
        string sFolderPath = sDataPath.Substring(0, sDataPath.Length - 6) + sAssetFolderPath;
        // get the system file paths of all the files in the asset folder
        string[] aFilePaths = Directory.GetFiles(sFolderPath, searchPattern: "*.prefab", searchOption: SearchOption.AllDirectories);
        // enumerate through the list of files loading the assets they represent and getting their type

        foreach (string sFilePath in aFilePaths)
        {
            if (sFilePath.EndsWith(".meta")) continue;
            string sAssetPath = sFilePath.Substring(sDataPath.Length - 6);
            // Debug.Log(sAssetPath);

            GameObject objAsset = AssetDatabase.LoadAssetAtPath(sAssetPath, typeof(UnityEngine.Object)) as GameObject;

            // Debug.Log(objAsset.GetType().Name);
            objects.Add(objAsset);
        }
        return objects;
    }
#endif    

    public static GameObject SafeGameObjectFind(string name)
    {
        GameObject newGameObject = GameObject.Find(name);
        if (newGameObject == null)
        {
            Debug.LogWarning("Could not find GameObject '" + name + "' with safe game object find.");
            return null;
        }
        else
        {
            return newGameObject;
        }
    }

    public static GameObject SafeGameObjectFindByTagName(string tagName)
    {
        // GameObject newGameObject = GameObject.Find(tagName);
        GameObject newGameObject = FindByTagName("Player");
        if (newGameObject == null)
        {
            Debug.LogWarning("Could not find GameObject '" + name + "' with safe game object find.");
            return null;
        }
        else
        {
            return newGameObject;
        }
    }

    public static PlayerControl SafePlayerControlFind()
    {
        string tagName = "Player";
        GameObject newGameObject = SafeGameObjectFindByTagName(tagName);
        PlayerControl playerControlRef = newGameObject.GetComponent<PlayerControl>() as PlayerControl;
        if (playerControlRef == null)
        {
            Debug.LogError("Could not find PlayerControl component of '" + tagName + "'.");
            return null;
        }
        else
        {
            return playerControlRef;
        }
    }

    public static bool IsPrefab(GameObject go)
    {
        //Debug.Log(go.name + " is prefab: " + (go.gameObject.scene.name == null || go.gameObject.scene.rootCount == 0) +
        //    " scene name: " + go.gameObject.scene.name + " rootCount: " + go.gameObject.scene.rootCount);
        return go.gameObject.scene.name == null || go.gameObject.scene.rootCount == 0;
    }

    public static bool IsObjectDebug(GameObject go)
    {
        return go.tag.Equals("Debug");
    }

    public static void setLevel(string index)
    {
        level = index;
    }

    public static string getlevel()
    {
        return level;
    }


    public static IEnumerator Destroy_Now(GameObject go, float delayTime, Action callback)//had to remove defaults.  Unity said they weren't allowed.
    {
        yield return new WaitForSeconds(delayTime);
        if (go)
        {
            if (callback != null)
            {
                callback();
            }

            MonoBehaviour.Destroy(go);
        }
    }
}
