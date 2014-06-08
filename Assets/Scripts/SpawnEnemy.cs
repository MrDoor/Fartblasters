using UnityEngine;
using System.Collections;

public class SpawnEnemy : MonoBehaviour {

	public GameObject enemy;
	public GameObject spawnPoint;
	private bool spawned;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter2D ( Collider2D obj )
	{
		if ( enemy && spawnPoint )
		{
			if ( obj.gameObject.name.Equals( "Player" ) )
			{
				if( !spawned )
				{
					spawned = true;
					GameObject newEnemy = (GameObject)Instantiate( enemy, spawnPoint.transform.position, Quaternion.identity );
				}
			}
		}
	}
}
