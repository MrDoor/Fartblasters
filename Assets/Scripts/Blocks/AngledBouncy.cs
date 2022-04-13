using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Bouncable
{
    public string name;
    public Vector2 bounceVector;
}

[System.Serializable]
public class Bouncables
{
    public List<Bouncable> BouncableObjects = new List<Bouncable>();

    public Bouncable At(string name)
    {
        if (BouncableObjects == null)
        {
            return null;
        }
        if (BouncableObjects.Count <= 0)
        {
            return null;
        }
        foreach (Bouncable b in BouncableObjects)
        {
            if (b.name == name)
            {
                return b;
            }
        }
        return null;
    }

    public bool Contains(string name)
    {
        if (BouncableObjects == null)
        {
            return false;
        }
        if (BouncableObjects.Count <= 0)
        {
            return false;
        }
        foreach (Bouncable b in BouncableObjects)
        {
            if (b.name == name)
            {
                return true;
            }
        }
        return false;
    }
}

public class AngledBouncy : MonoBehaviour
{
    public float bounceForce;
    public int baseForceX;
    public int baseForceY;
    public float SecondsBetweenBounces;
    public ForceMode2D forceModeToUse;

    public bool useRelative = false;

    // public List<string> BouncableObjectNames = new List<string>();
    // public Dictionary<string, Vector2> BouncableObjects = new Dictionary<string, Vector2>();
    // public List<Bouncable> BouncableObjects = new List<Bouncable>();
    public Bouncables BouncableObjects = new Bouncables();

    private float bounceTimer = 0f;
    private Vector2 defaultBounceVector = new Vector2(4000.0f, 4000.0f);

    public void LoadDefaults()
    {
        BouncableObjects = new Bouncables();

        Bouncable bRock = new Bouncable();
        bRock.name = "Rock";
        bRock.bounceVector = new Vector2(11500f, 11500f);

        BouncableObjects.BouncableObjects.Add(bRock);
    }

    // Use this for initialization
    void Start()
    {
        if (bounceForce <= 0)
        {
            bounceForce = 8f;
            Debug.Log("Bounce Setting bounceForce to: " + bounceForce);
        }

        baseForceX = baseForceX <= 0 ? 500 : baseForceX;
        baseForceY = baseForceY <= 0 ? 500 : baseForceY;

        if (BouncableObjects != null && BouncableObjects.BouncableObjects.Count <= 0)
        {
            LoadDefaults();
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

        bool isPlayer = coll.gameObject.CompareTag("Player");
        bool isBouncable = BouncableObjects.Contains(coll.gameObject.name);

        bool shouldBounce = isPlayer || isBouncable;

        if (shouldBounce)
        {
            //Debug.Log("Bounce coll: " + coll);
            if (isPlayer)
            {

                PlayerControl pControl = coll.gameObject.GetComponent<PlayerControl>();
                Rigidbody2D rb = pControl.GetComponent<Rigidbody2D>();

                Vector2 bounceForceVector = new Vector2((pControl.playerAnimation.isFacingRight ? baseForceX : -baseForceX), baseForceY) * bounceForce;
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


                bounceTimer = 0f;
            }
            else
            {
                Debug.Log($"Launching {coll.gameObject.name} with bounceForce: {bounceForce}!");

                Bouncable b = BouncableObjects.At(coll.gameObject.name);
                Vector2 bounceForceVector = b != null ? b.bounceVector : defaultBounceVector;

                Rigidbody2D rb = coll.gameObject.GetComponent<Rigidbody2D>();
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
            }

        }
    }
}
