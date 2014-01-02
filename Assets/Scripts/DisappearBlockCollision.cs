using UnityEngine;
using System.Collections;

public class DisappearBlockCollision : MonoBehaviour {

	float disappearTime = 0.0f;
	bool visibleTrigger = false;
	bool visibleFlag = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (disappearTime);
		if(visibleTrigger)
		{	
			if(visibleFlag)
			{
				visibleFlag = false;
			}
			else
			{
				visibleFlag = true;
			}			
			this.renderer.enabled = visibleFlag;
		}
		if(disappearTime != 0.0f && disappearTime <= Time.time)
		{
			this.gameObject.SetActive(false);
		}
	}
	
	void OnCollisionEnter2D(Collision2D coll)
	{
		if(coll.gameObject.name.Equals("Player"))
		{
			PlayerControl pControl = (PlayerControl)coll.gameObject.GetComponent<PlayerControl>();
			if(pControl)
			{
				if(disappearTime == 0.0f)
				{
					disappearTime = Time.time + 3.0f;
					visibleTrigger = true;
				}
			}
		}
		
	}
}
