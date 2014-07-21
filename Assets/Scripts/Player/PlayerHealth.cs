using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour 
{
    public PlayerControl playerControl;
    public HealthControl healthControl;
    public SpriteRenderer bodySpriteRenderer;
    public float maxHealth = 100f;

    private float currentHealth     = 0.0f;
    private float lastHit           = 0.0f;
    private float invinvibilityTime = 3f;
    private int flashCount          = 20;
    private float hitForce          = 500f;
    
    private const float defaultDamageAmount = 10f;
    private readonly Vector2 hitDirection   = new Vector2(3f, 1f);

    
    public void Init()
    {
        currentHealth   = maxHealth;
        lastHit         = Time.time;
        healthControl   = (HealthControl)GameObject.Find( "Health" ).GetComponent<HealthControl>();
        healthControl.updateHealth( currentHealth );
    }
            
    public void DecHealth( float amount = defaultDamageAmount )
    {
        if( Time.time >= lastHit )
        {
            currentHealth = Mathf.Max( ( currentHealth - amount ), 0f );
            healthControl.updateHealth( currentHealth );
        }
    }
    
    public void IncHealth( float amount )
    {
        currentHealth = Mathf.Min( ( currentHealth + amount ), maxHealth );
        healthControl.updateHealth( currentHealth );
    }
    
    public void KillPlayer()
    {
        PlayerPrefs.SetInt( "died", 1 );
        currentHealth = 0;
        healthControl.updateHealth( 0 );
        playerControl.StartDying();
    }
        
    public void Hit( Transform hitter )
    {
        if( Time.time > lastHit )
        {
            DecHealth();
            StartCoroutine( DamageFlash() );         
            if( currentHealth <= 0 )
            {
                playerControl.StartDying();
            }
            else
            {
                lastHit = Time.time + invinvibilityTime;
                this.transform.rigidbody2D.velocity = Vector3.zero; 
                Vector2 dir = new Vector2( hitDirection.x, hitDirection.y );
                if(this.transform.position.x <= hitter.position.x)
                {
                    dir.x *= -1;
                }               
                this.transform.rigidbody2D.AddForce( dir * hitForce );
            }
        }
    }

    IEnumerator DamageFlash()
    {
        bool colorSwitch = false;
        for( int i = 0; i < flashCount; i++ )
        {
            if( colorSwitch )
            {               
                bodySpriteRenderer.material.color = Color.white;
            }
            else
            {               
                bodySpriteRenderer.material.color = Color.red;
            }
            colorSwitch = !colorSwitch;
            yield return new WaitForSeconds(.10f);
        }
    }
}
