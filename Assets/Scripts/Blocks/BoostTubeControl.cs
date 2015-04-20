using UnityEngine;
using System.Collections;

public class BoostTubeControl : MonoBehaviour 
{
    public float boostForce = 1000f;
    public float moveTime = 0.5f;
    public BoostTubeCollision frontBoostCollision;
    public BoostTubeCollision backBoostCollision;
    public ParticleSystem particleSystem;

    private BoostTubeCollision entranceBoostCollision;

    void Start()
    {
        if(frontBoostCollision == null)
        {
            Debug.LogError("BoostTubeControl frontBoostCollision not set!");
        }

        if(backBoostCollision == null)
        {   
            Debug.LogError("BoostTubeControl backBoostCollision not set!");
        }

        entranceBoostCollision = null;
    }

    public void EnterTube( BoostTubeCollision boostTubeCollision )
    {
        entranceBoostCollision = boostTubeCollision;

        frontBoostCollision.SetIsActive( false );
        backBoostCollision.SetIsActive( false );
    }

    public void ExitTube()
    {
        entranceBoostCollision = null;

        frontBoostCollision.SetIsActive( true );
        backBoostCollision.SetIsActive( true );
    }
    
    public bool IsEntranceTubeCollision( BoostTubeCollision boostTubeCollision )
    {
        return (boostTubeCollision == entranceBoostCollision);
    }

    public Vector2 GetBoostDirection( BoostTubeCollision boostTubeCollision )
    {
        if( boostTubeCollision == frontBoostCollision )
        {
            return this.transform.right;
        }
        else
        {
            return -this.transform.right;
        }
    }
}
