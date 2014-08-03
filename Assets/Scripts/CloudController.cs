using UnityEngine;
using System.Collections;

public class CloudController : MonoBehaviour {

	public bool goRight;
	public float speed;
	public bool randomSpeed;
	public bool slowClouds;
	// Use this for initialization
	void Start () {
		if ( randomSpeed )
		{
			if ( slowClouds )
			{
				speed = Random.Range ( .1f, 1f );
			}
			else
			{
				speed = Random.Range ( .5f, 2f );
			}
		}
		speed = speed / 100;		
	}
	
	// Update is called once per frame
	void Update () {
		if ( goRight )
		{
			Vector2 temp = this.transform.position;
			temp.x += speed;
			this.transform.position = temp;
		}
		else
		{
			Vector2 temp = this.transform.position;
			temp.x -= speed;
			this.transform.position = temp;		
		}
	}
}
