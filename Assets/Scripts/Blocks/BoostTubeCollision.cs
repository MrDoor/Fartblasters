using UnityEngine;
using System.Collections;

public class BoostTubeCollision : MonoBehaviour 
{
    public BoostTubeControl boostTubeControl;
    public BoxCollider2D boxCollider2D;

    private float currMoveTime    = 0.0f;
    private bool isActive         = true;
    
    void OnTriggerEnter2D( Collider2D coll )
    {
        if( (boostTubeControl != null) && IsBoostable(coll.gameObject) )
        {
            if(isActive)
            {
                boostTubeControl.EnterTube( this );
                currMoveTime = 0.0f;
                StartCoroutine( Boost(coll) );
            }
        }
    }
    
    void OnTriggerExit2D( Collider2D coll )
    {
        if( (boostTubeControl != null) && IsBoostable(coll.gameObject) )
        {
            if( !isActive && !boostTubeControl.IsEntranceTubeCollision( this ) )
            {
                boostTubeControl.ExitTube();
            }
        }
    }

    public void SetIsActive( bool active )
    {
        isActive = active;
    }

    private IEnumerator Boost( Collider2D coll )
    {
        Vector3 collisionCenter = new Vector3(  this.gameObject.transform.position.x + boxCollider2D.offset.x,
                                                this.gameObject.transform.position.y + boxCollider2D.offset.y,
                                                this.gameObject.transform.position.z );
        
        float moveTime = boostTubeControl.moveTime;

        // Lerp the object to the center of the 2D collision box
        while( currMoveTime < moveTime )
        {
            currMoveTime += Time.deltaTime;

            if( currMoveTime > moveTime )
            {
                currMoveTime = moveTime;
            }

            float percentTime = currMoveTime / moveTime;
            coll.gameObject.transform.position = Vector3.Lerp(coll.gameObject.transform.position, collisionCenter, percentTime);

            yield return new WaitForEndOfFrame();
        }
        
        // Play sound effect
        GetComponent<AudioSource>().PlayOneShot( GetComponent<AudioSource>().clip );

        // Emit particles
        boostTubeControl.particleSystem.Play();

        // Add force
        coll.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        coll.GetComponent<Rigidbody2D>().AddForce( boostTubeControl.GetBoostDirection( this ) * boostTubeControl.boostForce * coll.GetComponent<Rigidbody2D>().mass );
    }

    private bool IsBoostable(GameObject obj)
    {
        return (obj.layer != Constants.LAYER_INDEX_TRAJECTORY_DOT);
    }
}
