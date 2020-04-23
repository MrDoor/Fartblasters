using UnityEngine;
using System.Collections;

public class SpawnItem : MonoBehaviour {

	public GameObject item;
	public GameObject spawnPoint;
	public bool isMulti;
	public int spawnCount;
	public float delay;
	
	private float accumulatedDelay;
	private bool spawned;
	// Use this for initialization
	void Start () {
		accumulatedDelay = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter2D ( Collider2D obj )
	{
		if ( item && spawnPoint )
		{
			if ( obj.gameObject.name.Equals( "Player" ) )
			{
				if( !spawned )
				{
					if(!isMulti)
					{
						spawned = true;
						accumulatedDelay = delay;
						StartCoroutine ( "SpawnItems" );				
					}
					else
					{
						for( int i = 0; i < spawnCount; i++)
						{
							StartCoroutine( "SpawnItems_Random" );
							accumulatedDelay += delay;
						}
						spawned = true;
					}
				}
			}
		}
	}
	
	IEnumerator SpawnItems_Random()
	{	
		yield return new WaitForSeconds( accumulatedDelay );
		GameObject newItem = (GameObject)Instantiate( item, spawnPoint.transform.position, Quaternion.identity );
		float x = Random.Range(-200, 200);
		float y = Random.Range(-200, 200);
		newItem.transform.GetComponent<Rigidbody2D>().AddForce( new Vector2( x, y ) );
	}
	
	IEnumerator SpawnItems()
	{	
		yield return new WaitForSeconds( accumulatedDelay );
		GameObject newItem = (GameObject)Instantiate( item, spawnPoint.transform.position, Quaternion.identity );
	}
}
