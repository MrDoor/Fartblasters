using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public float xMargin = 1f;      // Distance in the x axis the player can move before the camera follows.
    public float yMargin = 1f;      // Distance in the y axis the player can move before the camera follows.
    public float xSmooth = 4f;      // How smoothly the camera catches up with it's target movement in the x axis.
    public float ySmooth = 4f;      // How smoothly the camera catches up with it's target movement in the y axis.
    public Vector2 maxXAndY;        // The maximum x and y coordinates the camera can have.
    public Vector2 minXAndY;        // The minimum x and y coordinates the camera can have.

    private Transform player;		// Reference to the player's transform.
    public GameObject playerGO;     // Reference to the player's GameObject.

    public bool shouldRefindPlayer; // Way to trigger camera reset for debugging

    void Awake()
    {
        try
        {
            if (playerGO == null)
            {
                SetPlayerGameObject(GameObject.FindGameObjectWithTag("Player"));
            }

            if (!player)
            {
                playerGO = Util.SafeGameObjectFind("PlayerSpawn");
                player = playerGO.transform;
                Debug.Log("Player found at: " + player.position);
            }
        }
        catch (UnityException ue)
        {
            Debug.Log("Could not find player: " + ue.ToString());

        }

        if (minXAndY.x == 0 && minXAndY.y == 0)
        {
            minXAndY = new Vector2(-10, -10);
        }
        //TrackPlayer();

        //StartCoroutine ( MoveTo ( player.transform ) );
        //Camera.main.orthographicSize = 3;			
    }

    bool CheckXMargin()
    {
        // Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
        return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
    }


    bool CheckYMargin()
    {
        // Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
        return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
    }


    void FixedUpdate()
    {
        if (shouldRefindPlayer)
        {
            playerGO = null;
            player = null;
            shouldRefindPlayer = false;
        }

        EnsureTarget("Player");
        //Debug.Log(playerGO.tag);
        TrackPlayer();
    }

    public void EnsureTarget(string tag)
    {
        if (playerGO == null || playerGO.tag != tag)
        {
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            if (go != null)
            {
                SetPlayerGameObject(go);
            }
        }
        TrackPlayer();
    }

    public void SetPlayerGameObject(GameObject playerGameObject)
    {
        if (playerGameObject == null)
        {
            Debug.Log("playerGameObject was null when trying to set the player object on main camera.");
            return;
        }
        playerGO = playerGameObject;
        SetPlayerTransform(playerGO.transform);
    }

    public void SetPlayerTransform(Transform player_)
    {
        player = player_;
        Debug.Log("player = " + player.name);
    }

    void TrackPlayer()
    {
        // By default the target x and y coordinates of the camera are it's current x and y coordinates.
        float targetX = transform.position.x;
        float targetY = transform.position.y;

        // If the player has moved beyond the x margin...
        if (CheckXMargin())
        {
            // ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
            targetX = Mathf.Lerp(transform.position.x, player.position.x, xSmooth * Time.deltaTime);
        }

        // If the player has moved beyond the y margin...
        if (CheckYMargin())
        {
            // ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
            targetY = Mathf.Lerp(transform.position.y, player.position.y, ySmooth * Time.deltaTime);
        }

        // The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
        targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
        targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

        Debug.Log("Moving camera to: " + targetX + ", " + targetY);
        // Set the camera's position to the target position with the same z component.
        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }

    // Initial Player Camera Setup ********************************************************


    IEnumerator MoveTo(Transform trans)
    {
        yield return new WaitForSeconds(2);
        this.transform.position = trans.position;
        Debug.Log("Moved to: " + trans.position);
    }

}
