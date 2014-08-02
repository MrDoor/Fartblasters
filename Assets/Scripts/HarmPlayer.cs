using UnityEngine;
using System.Collections;

public class HarmPlayer : MonoBehaviour {

	public float damageAmount;
	
	private PlayerControl pControl;
	// Use this for initialization
	void Start () {
	/*
		try
		{
			pControl = GameObject.FindGameObjectWithTag ( "Player" );
		}
		catch( UnityException ue )
		{
			Debug.LogError ( "Could not find Player Control. Error: " + ue.ToString() );
		}
	*/	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter2D ( Collision2D coll )
	{
		if ( coll.gameObject.CompareTag ( "Player" ) )
		{			
			Debug.Log( "Player!" );
			PlayerControl pControl = coll.gameObject.GetComponent<PlayerControl>();
			pControl.playerHealth.Hit( this.transform, damageAmount );
			//StartCoroutine( "IgnorePlayer" );
		}
	}
	
	//Turns off collision for a short time between Player and Enemy
	IEnumerator IgnorePlayer()
	{
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer( "Player" ), LayerMask.NameToLayer( "Enemies" ), true);
		
		yield return new WaitForSeconds(3f);		
		
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer( "Player" ), LayerMask.NameToLayer( "Enemies" ), false);
	}
}
