using UnityEngine;
using System.Collections;

public class CollectFood : MonoBehaviour 
{
	public float pickupJuice = 0.0f;

	void OnTriggerEnter2D( Collider2D obj )
	{
		Debug.Log( "Food Eaten!" );

		if( obj.name == "Player" )
		{
			PlayerControl pControl = obj.GetComponent<PlayerControl>();
			if( pControl )
			{
				pControl.Launch_IncCurrentJuice( pickupJuice );
				
				if( this.gameObject.name.Contains( "(Clone)" ) )
				{
					pControl.Debug_DecFoodCount();
				}
			}

			Destroy( this.gameObject );	
		}
	}

}
