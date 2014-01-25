using UnityEngine;
using System.Collections;

public class FartometerControl : MonoBehaviour 
{
	public Texture backgroundTexture; 
	public Texture fillTexture;
	public Texture backFillTexture;
	public Texture capTexture;
	
	private PlayerControl playerControlRef;
	private Rect backgroundRect;
	private Rect fillRect;
	private Rect backFillRect;
	private Rect capRect;
	private Rect cropRect;
	private Rect backCropRect;
	private float cropTopOffset = 12;	// TODO: Come up with a better way to do the fartometer that doesn't involve magic numbers

	private float originalWidth = 1280;
	private float originalHeight = 720;
	private Vector3 scale;
	private float fillScale = 0.95f;

	void Awake() 
	{
        playerControlRef = Util.SafePlayerControlFind();
		if( !playerControlRef )
		{
			Debug.LogError( "Fartometer Control: Could not find Player Controls." );
		}

		scale.x = Screen.width / originalWidth;
		scale.y = Screen.width / originalHeight;
		scale.z = 1.0f;

		float left		= Screen.width / 50;
		float top		= Screen.height / 50;
		float width		= Screen.width / 30;
		float height	= width * ( 256 / 96.0f );

		backgroundRect	= new Rect( left, top, width, height );
		cropRect		= new Rect( left, top + cropTopOffset, width, height * fillScale );//z
		backCropRect	= new Rect( left, top + cropTopOffset, width, height * fillScale);//z
		fillRect		= new Rect( 0, 0, width, height * fillScale );
		backFillRect	= new Rect( 0, 0, width, height * fillScale );
		capRect			= new Rect( 0, 0, width, ( width / 6.0f ) );
	}

	void Update () 
	{
		float currentLaunchJuice	= playerControlRef.Launch_GetCurrentJuice();
		float fillPercent			= ( currentLaunchJuice - playerControlRef.Launch_GetPotentialJuice() ) / playerControlRef.Launch_GetMaxJuice();
		float backFillPercent		= currentLaunchJuice / playerControlRef.Launch_GetMaxJuice();

		if( currentLaunchJuice <= 0.0f )
		{
			fillPercent		= 0.0f;
			backFillPercent	= 0.0f;
		}

		// set default fills
		cropRect		= new Rect( backgroundRect.x, backgroundRect.y + cropTopOffset, backgroundRect.width, backgroundRect.height * fillScale);//
		backCropRect	= cropRect;
		fillRect		= new Rect( 0, -cropTopOffset, backgroundRect.width, backgroundRect.height * fillScale );
		backFillRect	= fillRect;
		capRect			= new Rect( 0, 0, backgroundRect.width, backgroundRect.width / 6.0f );

		if( backFillPercent < 1.0f )
		{	
			float backCropPercent = ( 1.0f - backFillPercent );
			float backCropAmount = backgroundRect.height * fillScale * backCropPercent;
			backCropRect = new Rect(	backgroundRect.x, 
			                       		backgroundRect.y + cropTopOffset + backCropAmount,
					                    backgroundRect.width,
			                        	Mathf.Max( backgroundRect.height - cropTopOffset - backCropAmount, cropTopOffset ) );
			
			backFillRect = new Rect(	0, 
			                        	-cropTopOffset - backCropAmount, 
					                    backgroundRect.width, 
			                        	backgroundRect.height * fillScale );
		}

		if( fillPercent < 1.0f )
		{
			float cropPercent = ( 1.0f - fillPercent );
			float cropAmount = backgroundRect.height * fillScale * cropPercent;

			cropRect = new Rect(	backgroundRect.x, 
			                    	backgroundRect.y + cropTopOffset + cropAmount,
			                    	backgroundRect.width,
			                    	Mathf.Max( backgroundRect.height - cropTopOffset - cropAmount, cropTopOffset ) );

			fillRect = new Rect(	0, 
			                    	-cropTopOffset - cropAmount, 
			                    	backgroundRect.width, 
			                    	backgroundRect.height * fillScale );
			capRect.y = 0;
		}
	}

	void OnGUI()
	{
		Matrix4x4 originalMatrix = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS( Vector3.zero, Quaternion.identity, scale );

		GUI.DrawTexture( backgroundRect, backgroundTexture ); 

		GUI.BeginGroup( backCropRect );
		GUI.DrawTexture( backFillRect, backFillTexture ); 	
		GUI.EndGroup();

		GUI.BeginGroup( cropRect );
		GUI.DrawTexture( fillRect, fillTexture ); 
		GUI.DrawTexture( capRect, capTexture ); 	
		GUI.EndGroup();

		GUI.matrix = originalMatrix;
	}
}
