using UnityEngine;
using System.Collections;

public class DetectCollisionDistance : MonoBehaviour {

	private PlayerControl pControl;
	private Transform wallTransform;	
	
	private bool seeWall;
	
	// Use this for initialization
	void Start () {
		pControl = Util.SafePlayerControlFind();
	}
	
	// Update is called once per frame
	void Update () {
		
		//Debug.Log ( "sqrMagnitude: " + transform.parent.rigidbody2D.velocity.sqrMagnitude + " angularVelocity: " +  transform.parent.rigidbody2D.angularVelocity );		
		
		//Debug.Log ( "velocity: " + transform.parent.rigidbody2D.velocity);
		if( wallTransform != null )
		{
			this.GetComponent<Animator>().SetFloat( "distFromWall",  Mathf.Abs ( pControl.transform.position.x - wallTransform.position.x ) );	
			if( Mathf.Abs( pControl.transform.position.x - wallTransform.position.x ) < 3 )
			{
				if ( transform.parent.GetComponent<Rigidbody2D>().velocity.x >= 3 && !pControl.GetOnGround() )
				{
					//this.GetComponent<Animator>().SetBool ( "nearWall" , true );
				}
			}
			else
			{
				this.GetComponent<Animator>().SetBool ( "nearWall" , false );
			}
		}
		else
		{
			this.GetComponent<Animator>().SetBool ( "nearWall" , false );
		}
	}
	
	void OnTriggerEnter2D ( Collider2D coll )
	{	
		if( coll.gameObject.name.Equals ( "Wall" ) )
		{	
			wallTransform = coll.transform;
			this.GetComponent<Animator>().SetBool ( "hitWall" , true );
		}
		else if( coll.gameObject.name.Equals ( "Ground" ) && pControl.GetOnGround() )
		{
			this.GetComponent<Animator>().SetBool ( "hitWall" , true );
		}
		StartCoroutine ( "resetHit" );		
	}
	
	IEnumerator resetHit()
	{
		yield return new WaitForSeconds ( .5f );
		this.GetComponent<Animator>().SetBool ( "hitWall" , false );
	}
	
}
