using UnityEngine;
using System;
using System.Collections;

public class Util
{
	public static string level;

	public static GameObject SafeGameObjectFind( string name )
	{
		GameObject newGameObject = GameObject.Find( name );
		if( newGameObject == null )
		{
			Debug.LogError( "Could not find GameObject '" + name + "'." );
			return null;
		}
		else
		{
			return newGameObject;
		}
	}

    public static PlayerControl SafePlayerControlFind()
    {
        string name = "Player";
        GameObject newGameObject = SafeGameObjectFind( name );
        PlayerControl playerControlRef = newGameObject.GetComponent<PlayerControl>() as PlayerControl;
        if( playerControlRef == null )
        {
            Debug.LogError( "Could not find PlayerControl component of '" + name + "'." );
            return null;
        }
        else
        {
            return playerControlRef;
        }
    }

	public static bool IsObjectDebug( GameObject go )
	{
		return go.tag.Equals( "Debug" );
	}

	public static void setLevel(string index)
	{
		level = index;
	}

	public static string  getlevel()
	{
		return level;
	}

    
    public static IEnumerator Destroy_Now( GameObject go, float delayTime, Action callback = null )
    {
        yield return new WaitForSeconds( delayTime );
        if( go )
        {
            if(callback != null)
            {
                callback();
            }

            MonoBehaviour.Destroy( go );
        }
    }
}
