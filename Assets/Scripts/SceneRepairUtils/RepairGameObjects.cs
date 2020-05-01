using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Fixable
{
    public string Name;
    public string SortLayer;

    public bool RepairAllChildren = false;
    public bool StartsWith = false;
}

public class RepairGameObjects : MonoBehaviour
{
    //public List<string> typesToFix;
    public List<Fixable> ThingsToFix;

    public bool ReplaceSortingOrder = false;
    public string SortLayerToChangeTo = "Default";

    public void FixGameObjects()
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
        {
            if (go.activeInHierarchy)
            {
                //Debug.Log(go.name);
                FixObject(go);
            }
        }
    }

    private void FixObject(GameObject go)
    {
        if (!ReplaceSortingOrder) { return; }

        //foreach (string typeString in typesToFix)
        foreach (Fixable thing in ThingsToFix)
        {
            //if (typeString == go.name)
            bool match;
            if (thing.StartsWith)
            {
                match = go.name.StartsWith(thing.Name);
            }
            else
            {
                match = thing.Name == go.name;
            }
            //if (thing.Name == go.name)
            if (match)
            {
                if (thing.RepairAllChildren)
                {
                    if (go.transform.childCount <= 0)
                    {
                        SetLayerName(go, thing);
                    }
                    else
                    {
                        RepairChildren(go, thing);
                    }
                }

                SetLayerName(go, thing);
                //SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                //if (sr == null)
                //{
                //    continue;
                //}



                //string oldSortLayerName = sr.sortingLayerName;
                //string newSortLayerName = SortLayerToChangeTo;
                //if (SortLayerToChangeTo == "")
                //{
                //    newSortLayerName = thing.SortLayer;
                //}
                //Debug.Log("Changing " + go.name + " " + oldSortLayerName + " to: " + newSortLayerName);
                //try
                //{
                //    sr.sortingLayerName = newSortLayerName;
                //}
                //catch (Exception ex)
                //{
                //    Debug.Log("Could not change name, reverting back. ex: " + ex.ToString());
                //    sr.sortingLayerName = oldSortLayerName;
                //}

            }
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
                SetLayerName(child, thing);
            }
        }
    }

    private void SetLayerName(GameObject go, Fixable thing)
    {
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            return;
        }

        string oldSortLayerName = sr.sortingLayerName;
        string newSortLayerName = SortLayerToChangeTo;
        if (SortLayerToChangeTo == "")
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
}
