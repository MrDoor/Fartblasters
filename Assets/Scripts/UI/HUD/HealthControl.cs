using UnityEngine;
using System.Collections;

public class HealthControl : MonoBehaviour {

	PlayerControl pControl;
	public Texture2D[] health = new Texture2D[5];
	public Texture2D fullHealth;
	public Texture2D halfHealth;
	public Texture2D emptyHealth;
	
	private float boxSize = 24f;
	private Rect backgroundRect;
	// Use this for initialization
	void Start () {
		pControl = Util.SafePlayerControlFind();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() 
    {	
			
		float left		= Screen.width / 30;
		float top		= Screen.height / 60;
		float width		= boxSize;
		float height	= boxSize;
		
		for( int i=0; i<health.Length; i++ )
		{
			backgroundRect = new Rect(left, top, width, height);
			GUI.DrawTexture(backgroundRect, health[i]);	
			left += boxSize;	
		}
	}
	
	//Really don't like this... but couldn't think of a better way at the moment.  There HAS to be a better way...
	public void updateHealth(float totalHealth)
	{
		switch((int)totalHealth)
		{
			case 100: 	health[4] = fullHealth;
						health[3] = fullHealth;
						health[2] = fullHealth;
						health[1] = fullHealth;
						health[0] = fullHealth;
						break;
			case 90: 	health[4] = halfHealth;
						health[3] = fullHealth;
						health[2] = fullHealth;
						health[1] = fullHealth;
						health[0] = fullHealth;
						break;
			case 80: 	health[4] = emptyHealth;
						health[3] = fullHealth;
						health[2] = fullHealth;
						health[1] = fullHealth;
						health[0] = fullHealth;
						break;
			case 70: 	health[4] = emptyHealth;
						health[3] = halfHealth;
						health[2] = fullHealth;
						health[1] = fullHealth;
						health[0] = fullHealth;
						break;
			case 60: 	health[4] = emptyHealth;
						health[3] = emptyHealth;
						health[2] = fullHealth;
						health[1] = fullHealth;
						health[0] = fullHealth;
						break;
			case 50: 	health[4] = emptyHealth;
						health[3] = emptyHealth;
						health[2] = halfHealth;
						health[1] = fullHealth;
						health[0] = fullHealth;
						break;
			case 40: 	health[4] = emptyHealth;
						health[3] = emptyHealth;
						health[2] = emptyHealth;
						health[1] = fullHealth;
						health[0] = fullHealth;
						break;
			case 30:	health[4] = emptyHealth;
						health[3] = emptyHealth;
						health[2] = emptyHealth;
						health[1] = halfHealth;
						health[0] = fullHealth;
						break;
			case 20: 	health[4] = emptyHealth;
						health[3] = emptyHealth;
						health[2] = emptyHealth;
						health[1] = emptyHealth;
						health[0] = fullHealth;
						break;
			case 10: 	health[4] = emptyHealth;
						health[3] = emptyHealth;
						health[2] = emptyHealth;
						health[1] = emptyHealth;
						health[0] = halfHealth;
						break;
			case 0: 	health[4] = emptyHealth;
						health[3] = emptyHealth;
						health[2] = emptyHealth;
						health[1] = emptyHealth;
						health[0] = emptyHealth;
						break;	
		}
	}
}
