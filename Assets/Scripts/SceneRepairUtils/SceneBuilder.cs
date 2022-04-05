using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Swapable
{
    public GameObject originalType;
    public GameObject newType;
}

[System.Serializable]
public class Searchable
{
    public GameObject typeToFind;
    [Header(" - or - ")]
    public string searchString;
    public bool endsWith;
    public bool startsWith;
}

public class SceneBuilder : MonoBehaviour
{
    [Header("Search for game objects within scene.", order = 0)]
    [Header("If text is supplied, the 'typeToFind' will be ignored.", order = 1)]
    [Space(5)]
    public Searchable search;

    public void SearchAndAdd()
    {
        string strToFind = search.searchString;
        if (search.typeToFind != null)
        {
            Debug.Log($"Searching for {search.typeToFind.name}...");
            strToFind = search.typeToFind.name;
            // return;
        }

        Debug.Log($"Search String: {search.searchString} startsWith: {search.startsWith} endsWith: {search.endsWith}");

        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
        {
            if (go.activeInHierarchy)
            {
                if (search.startsWith)
                {
                    if (go.name.StartsWith(strToFind))
                    {
                        Swapable s = new Swapable();
                        s.originalType = go;
                        items.Add(s);
                    }
                }
                else if (search.endsWith)
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
        string strToFind = search.searchString;
        if (search.typeToFind != null)
        {
            // Debug.Log($"Searching for {search.typeToFind.name}...");
            strToFind = search.typeToFind.name;
        }

        // Debug.Log($"Search String: {search.searchString} startsWith: {search.startsWith} endsWith: {search.endsWith}");

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
                if (search.startsWith)
                {
                    str = strToFind + s.originalType.name;
                }
                else if (search.endsWith)
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

    public void SwapObjects()
    {
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
            newSr.sortingLayerName = sr.sortingLayerName;

            // Remove Original
            Debug.Log($"Destroying {s.originalType.name}...");
            DestroyImmediate(s.originalType);
        }
    }
}
