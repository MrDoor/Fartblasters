using UnityEngine;
using System.Collections;

public class AngledBouncy : MonoBehaviour
{
    public float bounceForce;
    public int baseForce;
    public float SecondsBetweenBounces;
    public ForceMode2D forceModeToUse;

    public bool useRelative = false;

    private float bounceTimer = 0f;

    // Use this for initialization
    void Start()
    {
        if (bounceForce <= 0)
        {
            bounceForce = 8f;
            Debug.Log("Bounce Setting bounceForce to: " + bounceForce);
        }
    }

    // Update is called once per frame
    void Update()
    {
        bounceTimer += Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        //Debug.Log("Bounce timer: " + bounceTimer);
        if (bounceTimer < SecondsBetweenBounces) { return; }

        if (coll.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Bounce coll: " + coll);

            PlayerControl pControl = coll.gameObject.GetComponent<PlayerControl>();
            Rigidbody2D rb = pControl.GetComponent<Rigidbody2D>();

            Vector2 bounceForceVector = new Vector2((pControl.playerAnimation.isFacingRight ? baseForce : -baseForce), baseForce) * bounceForce;
            Debug.Log("Bounce force: " + bounceForce + " vector: " + bounceForceVector);
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            if (useRelative)
            {
                rb.AddRelativeForce(bounceForceVector, forceModeToUse);
            }
            else
            {
                rb.AddForce(bounceForceVector, forceModeToUse);
            }

            //Debug.Log("Bounce rb mass: " + rb.mass + " velocity:" + rb.velocity + " position: " + rb.position);

            bounceTimer = 0f;

            //pControl.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //Debug.Log("Bounce rigidBody velocity before: " + pControl.transform.GetComponent<Rigidbody2D>().velocity);

            //pControl.transform.GetComponent<Rigidbody2D>().AddForce(bounceForceVector, forceModeToUse);
            //Debug.Log("Bounce rigidBody velocity after: " + pControl.transform.GetComponent<Rigidbody2D>().velocity);


            //if (pControl.playerAnimation.isFacingRight)
            //{
            //    pControl.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //    pControl.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(-500, 500) * bounceForce);
            //}
            //else
            //{
            //    pControl.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //    pControl.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(500, 500) * bounceForce);
            //}

            //Debug.Log("z: " + this.transform.eulerAngles.z);
            //switch ((int)this.transform.eulerAngles.z)
            //{
            //    case 0:
            //        pControl.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //        pControl.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(200, 200) * bounceForce);
            //        break;
            //    case 90:
            //        pControl.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //        pControl.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(-300, -500) * bounceForce);
            //        break;
            //    case 135:
            //        pControl.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //        pControl.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(-500, -500) * bounceForce);
            //        break;
            //    case 270:
            //        pControl.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //        pControl.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(300, -700) * bounceForce);
            //        break;
            //}

        }
    }
}
