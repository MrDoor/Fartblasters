using UnityEngine;
using System.Collections;

public class CollectFood : MonoBehaviour 
{
	public float pickupJuice = 0.0f;

	void OnTriggerEnter2D( Collider2D obj )
	{
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
				
				checkFoodType(pControl);				
				Debug.Log( "Food Checked!" );
			}

			Destroy( this.gameObject );	
		}
	}
	
	void checkFoodType(PlayerControl pControl)
	{
		if(this.gameObject.name.Equals("Jalapeno"))
		{	
			Vector2 boost = new Vector2();
			if(pControl.Animation_GetFacingRight())
			{
				boost.x = 4000;
			}
			else
			{
				boost.x = -4000;
			}
			pControl.transform.rigidbody2D.AddForce(boost);			
			Debug.Log( "Fart Boost!" );
		}
		else
		{			
			Debug.Log( this.gameObject.name );
		}
	}

}
