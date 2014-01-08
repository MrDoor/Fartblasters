﻿using UnityEngine;
using System.Collections;

public class Util
{
	public static GameObject SafeGameObjectFind( string name )
	{
		GameObject newGameObject = GameObject.Find( name );
		if( !newGameObject )
		{
			Debug.LogError( "Could not find GameObject '" + name + "'." );
			return null;
		}
		else
		{
			return newGameObject;
		}
	}

	public static bool IsSpaceBarSpawnedFood( GameObject go )
	{
		return go.name.Contains( "_Grav" );
	}
}