using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

enum lerpDirection
{
    up,
    down,
    none
}

public class ItemOfInterest : MonoBehaviour
{
    Light2D light;

    public float smallestSize;
    public float largestSize;
    public float speed;

    private float currentSize;
    private float currentDest;
    private lerpDirection direction = lerpDirection.up;
    private float lerp = 0f;
    private float duration = 2f;
    // Start is called before the first frame update
    void Start()
    {
        // light = gameObject.GetComponent(typeof(Light2D)) as Light2D;
        light = gameObject.GetComponent<Light2D>();
        if (light == null)
        {
            Debug.LogWarning($"Could not find Light2D on {gameObject.name}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (direction == lerpDirection.up && currentSize >= largestSize)
        {
            currentDest = smallestSize;
            direction = lerpDirection.down;
        }
        else if (direction == lerpDirection.down && currentSize <= smallestSize)
        {
            currentDest = largestSize;
            direction = lerpDirection.up;
        }

        light.pointLightOuterRadius = currentSize;
        // Debug.Log($"currentSize: {currentSize}");
        throb();
    }

    //  float lerp = 0f, duration = 2f;

    public void throb()
    {
        //  score = 1000;
        //  scoreTo = 800;
        // lerp += Time.deltaTime / duration;
        // currentSize = Mathf.Lerp(currentSize, currentDest, lerp);

        if (direction == lerpDirection.up)
        {
            currentSize += (speed * 0.01f);
        }
        else if (direction == lerpDirection.down)
        {
            currentSize -= (speed * 0.01f);
        }
    }
}
