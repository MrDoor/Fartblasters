using UnityEngine;
using System.Collections;

public class RetractClaw : MonoBehaviour {

	public GameObject block;
	public float retractSpeed;
	
	private ClawController claw;
	private Vector3 startPosition;
	
	// Use this for initialization
	void Start () {
		claw = block.GetComponent<ClawController>();
		startPosition = claw.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	void OnTriggerEnter2D ( Collider2D coll )
	{
		if( coll.tag.Equals( "Player" ) )
		{
			PlayerPrefs.SetInt("died", 0);
			StartCoroutine ( "Retract" );
		}
		else
		{
			Debug.Log ( coll.tag );
		}
	}
	
	IEnumerator Retract()
	{
		while ( claw.transform.position.y < startPosition.y + 10 )
		{
			Vector3 newPosition = claw.transform.position;
			newPosition.y += .01f;
			claw.transform.position = newPosition;			
			yield return new WaitForSeconds ( retractSpeed );
		}
		claw.enabled = false;
		Destroy( Util.SafeGameObjectFind ( "Claw_With_Player" ) , 3f );
	}
}
