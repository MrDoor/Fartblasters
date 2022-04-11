#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

using UnityEditor;

[System.Serializable]
public class Swapable
{
    public GameObject originalType;
    public GameObject newType;
}

// [System.Serializable]
public class Searchable
{
    public GameObject typeToFind;
    [Header(" - or - ")]
    public string searchString;
    public bool endsWith;
    public bool startsWith;
}

[System.Serializable]
public class SceneSearch : Searchable
{

}

[System.Serializable]
public class PrefabSearch : Searchable 
{

}

public class SceneBuilder : MonoBehaviour
{
    [Header("Search for game objects within scene.", order = 0)]
    [Header("If text is supplied, the 'typeToFind' will be ignored.", order = 1)]
    public SceneSearch sceneSearch;
    // [Space(5)]

    [Header("Search for game objects within scene.", order = 0)]
    [Header("If text is supplied, the 'typeToFind' will be ignored.", order = 1)]
    public PrefabSearch prefabSearch;
    // [Space(5)]

    public List<Vector3> positions = new List<Vector3>();

    private void OnDrawGizmos()
    {
        // Draw X on each Game Object position that is in the list 
        foreach (Vector3 pos in positions)
        {
            Vector3 topLeft = pos + new Vector3(-1, 1, 0);
            Vector3 topRight = pos + new Vector3(1, 1, 0);
            Vector3 bottomLeft = pos + new Vector3(-1, -1, 0);
            Vector3 bottomRight = pos + new Vector3(1, -1, 0);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(topLeft, bottomRight);
            Gizmos.DrawLine(topRight, bottomLeft);
        }
    }

    public void SearchAndAdd()
    {
        string strToFind = sceneSearch.searchString;
        if (sceneSearch.typeToFind != null)
        {
            Debug.Log($"Searching for {sceneSearch.typeToFind.name}...");
            strToFind = sceneSearch.typeToFind.name;
            // return;
        }

        Debug.Log($"Search String: {sceneSearch.searchString} startsWith: {sceneSearch.startsWith} endsWith: {sceneSearch.endsWith}");

        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
        {
            if (go.activeInHierarchy)
            {
                if (sceneSearch.startsWith)
                {
                    if (go.name.StartsWith(strToFind))
                    {
                        Swapable s = new Swapable();
                        s.originalType = go;
                        items.Add(s);
                    }
                }
                else if (sceneSearch.endsWith)
                {
                    if (go.name.EndsWith(strToFind))
                    {
                        Swapable s = new Swapable();
                        s.originalType = go;
                        items.Add(s);
                    }
                }
                else
                {
                    if (go.name == strToFind)
                    {
                        Swapable s = new Swapable();
                        s.originalType = go;
                        items.Add(s);
                    }
                }
            }
        }
    }

    public void SearchAndAddReplacements()
    {
        string strToFind = prefabSearch.searchString;
        if (prefabSearch.typeToFind != null)
        {
            // Debug.Log($"Searching for {prefabSearch.typeToFind.name}...");
            strToFind = prefabSearch.typeToFind.name;
        }

        // Debug.Log($"Search String: {prefabSearch.searchString} startsWith: {prefabSearch.startsWith} endsWith: {prefabSearch.endsWith}");

        List<GameObject> prefabs = Util.GetAllPrefabs();
        if (prefabs.Count < 1)
        {
            Debug.LogError("Could not load prefabs.");
            return;
        }

        // GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        string str = "";
        // foreach (GameObject go in allObjects)
        foreach (GameObject go in prefabs)
        {
            foreach (Swapable s in items)
            {
                if (prefabSearch.startsWith)
                {
                    str = strToFind + s.originalType.name;
                }
                else if (prefabSearch.endsWith)
                {
                    str = s.originalType.name + strToFind;
                }
                else
                {
                    str = strToFind;
                }

                if (str == go.name)
                {
                    s.newType = go;
                }
                str = "";
            }
        }
    }

    [Header("These items represent the list of things to be swapped.", order = 4)]
    [Header("The Search can be used to populate this list.", order = 5)]
    [Space(5)]
    public List<Swapable> items;

    public void ClearItems() {
        items.Clear();
    }

    public void SwapObjects()
    {
        positions.Clear();
        if (items.Count < 1)
        {
            Debug.LogWarning("No items added to Scene Builder.");
        }

        foreach (Swapable s in items)
        {
            Debug.Log($"GameObject {s.originalType.name} is at {s.originalType.transform.position}");
            Transform t = s.originalType.transform;
            Vector3 position = t.position;
            Quaternion rotation = t.rotation;
            Vector3 scale = t.localScale;

            SpriteRenderer sr = s.originalType.GetComponent<SpriteRenderer>();

            if (s.newType == null) { continue; }

            // Make new object
            GameObject go = Instantiate(s.newType);

            go.transform.position = position;
            go.transform.rotation = rotation;
            go.transform.localScale = scale;

            SpriteRenderer newSr = go.GetComponent<SpriteRenderer>();
            if (sr != null && newSr != null)
            {
                newSr.sortingLayerName = sr.sortingLayerName;
            }

            // Remove Original
            if (PrefabUtility.IsPartOfAnyPrefab(s.originalType))
            {
                Debug.Log($"{s.originalType.name} is or is part of a Prefab...");

                GameObject prefabParent = PrefabUtility.GetOutermostPrefabInstanceRoot(s.originalType);
                PrefabUtility.UnpackPrefabInstance(prefabParent, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);

                try
                {
                    DestroyImmediate(s.originalType);
                }
                catch (Exception ex)
                {
                    Vector3 pos = s.originalType.transform.position;
                    positions.Add(pos);
                }
                continue;
            }
            else
            {
                Debug.Log($"Destroying {s.originalType.name}...");
                DestroyImmediate(s.originalType);
            }
        }
    }

    public void SwapAllSceneObjects() {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
        {
            if (go.activeInHierarchy)
            {
                Swapable s = new Swapable();
                s.originalType = go;
                items.Add(s);
            }
        }

        SearchAndAddReplacements();

        List<Swapable> toRemove = new List<Swapable>();
        foreach (Swapable item in items) {
            if (item.originalType == null || item.newType == null) {
                toRemove.Add(item);
            }
        }
        
        foreach (Swapable item in toRemove) {
            items.Remove(item);            
        }

        SwapObjects();
    }
}
#endif