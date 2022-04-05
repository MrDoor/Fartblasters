using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;


[System.Serializable]
public class Fixable
{
    public int LayerSortOrder;
    public string Name;
    public string OrganizationLayer;
    public bool RepairAllChildren = false;
    public bool ReplaceLayerName = false;
    public bool ReplaceLayerOrder = false;
    public bool ShouldOrganize;
    public string SortLayer;
    public bool StartsWith = false;
}

public class RepairGameObjects : MonoBehaviour
{
    public List<Fixable> ThingsToFix;

    public string ChangeAllLayersTo = ""; // If blank it will not be used
    public bool CleanUpNames;
    public bool OrganizeScene;

    private Dictionary<string, List<GameObject>> Organizer;

    private string CalculateSortingLayer(GameObject parent)
    {
        SortingLayer layer = SortingLayer.layers[0];// Should be defualt layer?
        string layerName = layer.name;
        if (parent.transform.childCount <= 0)
        {
            SpriteRenderer sr = parent.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                return sr.sortingLayerName;
            }
        }
        else
        {
            Dictionary<string, int> layers = new Dictionary<string, int>();
            foreach (SortingLayer l in SortingLayer.layers)
            {
                layers.Add(l.name, 0);
            }

            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Transform child = parent.transform.GetChild(i);
                SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
                if (sr == null)
                {
                    continue;
                }

                string sl = sr.sortingLayerName;
                if (layers.ContainsKey(sl))
                {
                    layers[sl]++;
                }

                int max = 0;
                foreach (var keyValuePair in layers)
                {
                    if (max < keyValuePair.Value)
                    {
                        max = keyValuePair.Value;
                        layerName = keyValuePair.Key;
                    }
                }
            }
        }

        return layerName;
    }

    public void CleanUpName(GameObject go)
    {
        if (go.name.EndsWith(")"))
        {
            //Debug.Log("Trying regex on : " + go.name);
            Regex regex = new Regex(@"\([0-9a-zA-Z\s]+\)");
            Match match = regex.Match(go.name);
            Debug.Log("Removing " + match + " from " + go.name);
            go.name = go.name.Replace(match.ToString(), "").Trim();
        }
    }

    public void DoOrganize()
    {
        InitializeOrganization();

        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
        {
            string suggestedSortingLayer = CalculateSortingLayer(go);
            if (suggestedSortingLayer == "Default")
            {
                Debug.Log($"{suggestedSortingLayer} found for {go.name}, cannot determine a reasonable sorting location, skipping...");
                continue;
            }

            GameObject topParent = FindTopLevelParent(go);
            if (go.name == topParent.name)
            {
                Debug.Log($"Adding to Organize list: {go.name} into {suggestedSortingLayer}...");
                Organizer[suggestedSortingLayer].Add(go);
            }

            if (CleanUpNames)
            {
                CleanUpName(go);
            }
        }

        Organize();
    }

    private GameObject FindTopLevelParent(GameObject go)
    {
        GameObject temp = go;
        if (temp.transform.parent != null)
        {
            Debug.Log($"{go.name} has a parent: {temp.transform.parent.name} and root is: {go.transform.root}...");
            temp = go.transform.root.gameObject;
        }
        return temp;
    }

    private GameObject FindFirstParentWhoIsContainer(GameObject go)
    {
        GameObject temp = go;
        DateTime start = DateTime.Now;
        DateTime then;
        int secondDiff = 0;
        while (temp.transform.parent != null)
        {
            if (IsContainer(temp))
            {
                break;
            }
            else
            {
                temp = temp.transform.parent.gameObject;
            }
            then = DateTime.Now;
            secondDiff = Math.Abs(then.Second - start.Second);
            //Debug.Log("Looking for Container.  start: " + start + " then: " + then + " diff: " + secondDiff);
            if (secondDiff >= 5) { break; } // Bail if the search takes over 5 seconds
        }

        //Debug.Log("Before Check... " + temp);

        if (IsContainer(temp))
        {
            // Debug.Log("Found " + temp.name);
            return temp;
        }
        else
        {
            // Debug.Log("Not found.");
            return null;
        }
    }

    public void FixGameObjects()
    {
        if (OrganizeScene)
        {
            InitializeOrganization();
        }

        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
        {
            if (go.activeInHierarchy)
            {
                FixObject(go);

                if (CleanUpNames)
                {
                    CleanUpName(go);
                }
            }
        }

        if (OrganizeScene)
        {
            Organize();
        }
    }

    private void FixObject(GameObject go)
    {
        foreach (Fixable thing in ThingsToFix)
        {
            bool match;
            if (thing.StartsWith)
            {
                match = go.name.StartsWith(thing.Name);
            }
            else
            {
                match = thing.Name == go.name;
            }

            if (match)
            {
                if (thing.RepairAllChildren)
                {
                    if (go.transform.childCount <= 0)
                    {
                        //SetLayerName(go, thing);
                        RepairGameObject(go, thing);
                    }
                    else
                    {
                        RepairChildren(go, thing);
                    }
                }

                //SetLayerName(go, thing);
                RepairGameObject(go, thing);
            }
        }
    }

    public void InitializeOrganization()
    {
        Organizer = new Dictionary<string, List<GameObject>>();
        foreach (SortingLayer layer in SortingLayer.layers)
        {
            // Debug.Log("Sorting layer " + layer.name);
            if (layer.name == "Default") { continue; }

            Organizer.Add(layer.name, new List<GameObject>());

            GameObject container = Util.FindOrCreateNew(layer.name + " Container", "Container");
        }
    }

    public bool IsContainer(GameObject go)
    {
        if (go == null) { return false; }
        return go.CompareTag("Container") ||
        (go.GetComponents(typeof(Component))).Length == 1;//Transform only
    }

    public void Organize()
    {
        foreach (string containerName in Organizer.Keys)
        {
            Debug.Log($"Working on {containerName}, count: {Organizer[containerName].Count}");
            if (Organizer[containerName].Count <= 0) { continue; }
            GameObject container = Util.FindOrCreateNew(containerName + " Container", "Container");
            foreach (GameObject go in Organizer[containerName])
            {
                GameObject parent = FindFirstParentWhoIsContainer(go);

                if (parent != null)
                {
                    string sortingLayer = CalculateSortingLayer(parent);
                    if (sortingLayer == containerName)
                    {
                        parent.transform.SetParent(container.transform);
                    }
                    else
                    {
                        GameObject containerForDifferentLayer = Util.FindOrCreateNew(sortingLayer + " Container", "Container");
                        parent.transform.SetParent(containerForDifferentLayer.transform);
                    }
                }
                else
                {
                    if (go.transform.parent != null && Util.IsPrefab(go.transform.parent.gameObject)) // Move parent, not GameObject within prefab
                    {
                        Debug.Log(go.name + " is a prefab");
                        go.transform.parent.SetParent(container.transform);

                        //GameObject newGameObject = (GameObject)Instantiate(go);
                        //newGameObject.transform.parent = container.transform;
                        //Remove prefab?
                    }
                    else
                    {
                        go.transform.SetParent(container.transform);
                        Debug.Log(go.name + " parent set to " + container.name);
                    }
                }
            }
        }
    }

    private void ReplaceLayerProperties(GameObject go, Fixable thing)
    {
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();

        if (thing.ShouldOrganize)
        {
            string organizationLayer = thing.OrganizationLayer;
            if (sr != null && organizationLayer == "")
            {
                organizationLayer = sr.sortingLayerName;
            }
            //if (Organizer.ContainsKey(sr.sortingLayerName))
            if (Organizer.ContainsKey(organizationLayer))
            {
                Debug.Log("Adding " + go.name + " to " + organizationLayer + " list.");
                Organizer[organizationLayer].Add(go);
            }
            else
            {
                Debug.Log("Could not find " + organizationLayer + " in Organizer for " + go.name);
            }
        }

        if (sr == null)
        {
            return;
        }

        if (thing.ReplaceLayerName)
        {
            SetLayerName(go, thing, sr);
        }

        if (thing.ReplaceLayerOrder)
        {
            SetLayerOrder(go, thing, sr);
        }
    }

    private void RepairChildren(GameObject go, Fixable thing)
    {
        for (int i = 0; i < go.transform.childCount; i++)
        {
            GameObject child = go.transform.GetChild(i).gameObject;
            if (child.transform.childCount > 0)
            {
                RepairChildren(child, thing);
            }
            else
            {
                //SetLayerName(child, thing);
                RepairGameObject(child, thing);
            }
        }
    }

    private void RepairGameObject(GameObject go, Fixable thing)
    {
        if (thing.ReplaceLayerName || thing.ReplaceLayerOrder || thing.ShouldOrganize)
        {
            ReplaceLayerProperties(go, thing);
        }
    }

    private void SetLayerName(GameObject go, Fixable thing, SpriteRenderer sr)
    {
        string oldSortLayerName = sr.sortingLayerName;
        string newSortLayerName = ChangeAllLayersTo;
        if (ChangeAllLayersTo == "")
        {
            newSortLayerName = thing.SortLayer;
        }
        Debug.Log("Changing " + go.name + " " + oldSortLayerName + " to: " + newSortLayerName);
        try
        {
            sr.sortingLayerName = newSortLayerName;
        }
        catch (Exception ex)
        {
            Debug.Log("Could not change name, reverting back. ex: " + ex.ToString());
            sr.sortingLayerName = oldSortLayerName;
        }
    }

    private void SetLayerOrder(GameObject go, Fixable thing, SpriteRenderer sr)
    {
        int oldSortOrder = sr.sortingOrder;
        int newSortOrder = thing.LayerSortOrder;

        Debug.Log("Changing " + sr.sortingOrder + " " + oldSortOrder + " to: " + newSortOrder);
        try
        {
            sr.sortingOrder = newSortOrder;
        }
        catch (Exception ex)
        {
            Debug.Log("Could not change sorting order, reverting back. ex: " + ex.ToString());
            sr.sortingOrder = oldSortOrder;
        }
    }
}
