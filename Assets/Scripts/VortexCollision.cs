using UnityEngine;
using System.Collections;

public class VortexCollision : MonoBehaviour {
	
	public Transform startMarker;
	public Vector3 endMark;
	public float speed = 3.0f;
	private float startTime;
	private float journeyLength;
	public Transform target;
	public float smooth = 5.0F;
	private float timeToLaunch = 0.0f;
	private float timeToReLaunch = 0.0f;
	private bool canLaunch = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		PlayerControl pControl = (PlayerControl)GameObject.Find ("Player").GetComponent<PlayerControl>();
		if(timeToLaunch <= Time.time && timeToReLaunch <= Time.time && canLaunch)
		{				
			Debug.Log("Gravity Scale before: " + GameObject.Find ("Player").transform.rigidbody2D.gravityScale);
			canLaunch = false;
			timeToReLaunch = Time.time + 3.0f;
			GameObject.Find ("Player").transform.rigidbody2D.gravityScale = 1;			
			GameObject.Find("Player").SendMessage("SetInVortex", false);
			EjectFromVortex();
			Debug.Log("Gravity Scale after: " + GameObject.Find ("Player").transform.rigidbody2D.gravityScale);			
		}
		else if(pControl && pControl.GetInVortex())
		{
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			target.transform.position = Vector3.Lerp(startMarker.position, endMark, fracJourney);
		}
			
	}
	
	void OnTriggerEnter2D(Collider2D obj)
	{
		//WIP -- Don't much like the animation... 
		if(timeToReLaunch <= Time.time)
		{
			if(obj.name.Equals("Player"))
			{
				PlayerControl pControl = obj.GetComponent<PlayerControl>();
				if(pControl)
				{
					try
					{
						Vector3 tempEnd = this.transform.position;
						tempEnd = this.collider2D.transform.position;
						endMark = tempEnd;						
						target = pControl.transform;
						startMarker = pControl.transform;				
						startTime = Time.time;				
						pControl.transform.rigidbody2D.gravityScale = 0;
						pControl.transform.rigidbody2D.velocity = Vector2.zero;
						journeyLength = Vector3.Distance(startMarker.position, endMark);
						pControl.SendMessage("SetInVortex", true);
						timeToLaunch = Time.time + 3.0f;
						canLaunch = true;
						Debug.Log ("Animation Played");
					}
					catch(UnityException ue)
					{
						Debug.Log("Error: " + ue.ToString());
					}
				}
			}
			else
			{
				Debug.Log ("obj not player. obj: " + obj.ToString());
			}
		}
	}
	
	void EjectFromVortex()
	{
		Vector2 direction = new Vector2();
		direction.x = Random.Range(0, 180) * .01f;
		direction.y = Random.Range(0, 180) * .01f;
		float force = Random.Range(5, 10) * 1000f;
		float sign = Random.Range (0, 11);
		Debug.Log ("sign: " + sign);
		if(sign >= 5)
		{
			direction *= -1;
		}
		Debug.Log("direction: " + direction.ToString() + " force: " + force + " direction*force = " + (direction * force));
		GameObject.Find ("Player").transform.rigidbody2D.AddForce(direction * force);
	}
}
