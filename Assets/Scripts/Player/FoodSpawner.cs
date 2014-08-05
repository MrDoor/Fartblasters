using UnityEngine;
using System.Collections;

public class FoodSpawner : MonoBehaviour 
{
    public GameObject foodPrefab;
    
    private int maxFoodCount        = 3;
    private int currFoodCount       = 0;
    private float foodDestroyTime   = 3.0f;


    public void CheckSpawnFood()
    {
        if( Input.GetButtonDown( "Debug Spawn Food" ) )
        {
            SpawnFood();
        }
    }
    
    private void SpawnFood()
    {
        if( foodPrefab )
        {
            if( currFoodCount < maxFoodCount )
            {
                float foodYOffset   = 1.5f;
                Vector3 newFoodPos  = transform.position;
                newFoodPos.y += foodYOffset;
                
                GameObject newFood = (GameObject)Instantiate( foodPrefab, newFoodPos, Quaternion.identity );
                IncFoodCount();
                
                Destroy_Self( newFood, foodDestroyTime );
            }
            else
            {
                Debug.Log( "Debug Food Count: " + currFoodCount );
            }
        }
        else
        {
            Debug.Log( "Pressed Space but no prefab was set to Debug Spawn Food." );
        }
    }
    
    public void DecFoodCount()
    {
        currFoodCount--;
    }
    
    public void IncFoodCount()
    {
        currFoodCount++;
    }
    
    public void Destroy_Self( GameObject go, float delayTime )
    {
        StartCoroutine( Util.Destroy_Now( go, delayTime, () => DecDebugObj(go) ) );
    }
    
    private void DecDebugObj(GameObject go)
    {
        if( Util.IsObjectDebug( go ) )
        {
            DecFoodCount();
        }
    }
}
