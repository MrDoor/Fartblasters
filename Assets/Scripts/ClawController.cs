using UnityEngine;
using System.Collections;

public class ClawController : MonoBehaviour
{
    public PlayerControl playerControl;

    public GameObject leftClaw;
    public GameObject rightClaw;
    public float movementZone;
    public float movementSpeed;

    private float maxDistance;
    private float minDistance;

    private bool leftPressed;
    private bool rightPressed;
    private bool upPressed;
    private bool downPressed;

    private bool aButtonDown;

    private float horizontal = 0f;
    private float vertical = 0f;

    // Use this for initialization
    void Start()
    {
        maxDistance = this.transform.position.x + (movementZone / 2);
        minDistance = this.transform.position.x - (movementZone / 2);
        playerControl = Util.SafeGameObjectFind("Player").GetComponent<PlayerControl>();
        playerControl.inClaw = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerControl.inClaw) { return; }

        // TODO: Replace with new controller support

        // horizontal = Input.GetAxis("HorizontalDpad");
        // vertical = Input.GetAxis("VerticalDpad");

        // if (Input.GetKey("left") || horizontal == -1) { leftPressed = true; } else { leftPressed = false; }
        // if (Input.GetKey("right") || horizontal == 1) { rightPressed = true; } else { rightPressed = false; }
        // if (Input.GetKey("up") || vertical == 1) { upPressed = true; } else { upPressed = false; }
        // if (Input.GetKey("down") || vertical == -1) { downPressed = true; } else { downPressed = false; }

        // if (leftPressed)
        // {
        //     if (this.transform.position.x >= minDistance)
        //     {
        //         Vector2 newPosition = this.transform.position;
        //         newPosition.x = newPosition.x - movementSpeed / 100;

        //         this.transform.position = newPosition;
        //     }
        // }
        // else if (rightPressed)
        // {
        //     if (this.transform.position.x <= maxDistance)
        //     {
        //         Vector2 newPosition = this.transform.position;
        //         newPosition.x = newPosition.x + movementSpeed / 100;

        //         this.transform.position = newPosition;
        //     }
        // }
        // else if (downPressed)
        // {
        //     if (this.transform.position.y >= Camera.main.orthographicSize + 1)
        //     {
        //         Vector2 newPosition = this.transform.position;
        //         newPosition.y = newPosition.y - movementSpeed / 100;

        //         this.transform.position = newPosition;
        //     }
        // }
        // else if (upPressed)
        // {
        //     Vector2 newPosition = this.transform.position;
        //     newPosition.y = newPosition.y + movementSpeed / 100;

        //     this.transform.position = newPosition;
        // }

        if (rightClaw && leftClaw)
        {
            JointMotor2D tempLeftJoint = leftClaw.GetComponent<HingeJoint2D>().motor;
            JointMotor2D tempRightJoint = rightClaw.GetComponent<HingeJoint2D>().motor;

            // bool shouldOpen = (Input.GetKey("right shift") || Input.GetButton("AButton"));

            // leftClaw.GetComponent<HingeJoint2D>().useMotor = shouldOpen;
            // rightClaw.GetComponent<HingeJoint2D>().useMotor = shouldOpen;

            //        if (shouldOpen)
            //        {
            //            /*
            //tempLeftJoint.motorSpeed = 100;
            //leftClaw.GetComponent<HingeJoint2D>().motor = tempLeftJoint;

            //tempRightJoint.motorSpeed = -100;
            //rightClaw.GetComponent<HingeJoint2D>().motor = tempRightJoint;
            //*/

            //            leftClaw.GetComponent<HingeJoint2D>().useMotor = true;
            //            rightClaw.GetComponent<HingeJoint2D>().useMotor = true;
            //        }
            //        else
            //        {
            //            /*
            //tempLeftJoint.motorSpeed = -100;
            //leftClaw.GetComponent<HingeJoint2D>().motor = tempLeftJoint;

            //tempRightJoint.motorSpeed = 100;
            //rightClaw.GetComponent<HingeJoint2D>().motor = tempRightJoint;
            //*/
            //            leftClaw.GetComponent<HingeJoint2D>().useMotor = false;
            //            rightClaw.GetComponent<HingeJoint2D>().useMotor = false;
            //        }
        }
    }
}
