using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Move : MonoBehaviour
{
    public static Move SingletonInstance = null;

    //User states
    public enum LadderState {Off, Climbing, GettingOff};
    public enum ViewSide { fromNormal, fromSide}

    //Variables
	public float speed = 2f;               //Controls Movementspeed
    private float sprintMultiplier = 1f;    //Controls the speed of the player when running
    [HideInInspector] public LadderState ladderState;   //Controls ladder movement
    [HideInInspector] public static ViewSide viewSide;   //Controls movement based on view
    private CharacterController physBody;   //References the bounding box of the player
    [HideInInspector] public static bool inHiding;  //Used to confirm player is out of a guard's eyesight
    //For updating the player's sprite
    Animator animator;
    public static bool isIdle;  //Used for other objects to check if the player is moving or not

    // Start is called before the first frame update
    void Start()
    {
		//Get the physical body component
        physBody = GetComponent<CharacterController>();
        ladderState = LadderState.Off;
        viewSide = ViewSide.fromNormal;
        inHiding = false;

        animator = GetComponent<Animator>();
        isIdle = true;

        //If we are awoken on a restart
        if (GameManager.didRestart)
        {
            //set position to the restart position
            transform.position = GameManager.restartPosition;
            GameManager.didRestart = false;
        }
        //If we are awoken as a scene change
        else if(SceneManager.GetActiveScene().name.Contains("Building"))
        {
            /*if(!GameManager.inBuilding)
            {
                GameManager.playerPosition = transform.position;
                GameManager.inBuilding = true;
                GameManager.initList();
            }*/
            transform.position = GameManager.playerPosition;
            GameManager.restartPosition = GameManager.playerPosition;
        }
        else
        {
            GameManager.restartPosition = transform.position;
        }
    }


    // Update is called once per frame
    void Update()
    {
        //For the action hero
        //if(!ActionHero.isAttacking)
        //    GameManager.playerPosition = gameObject.transform.position;

        if (!PauseMenu.isPaused && !inHiding && !MapAccess.inMap)
        {
            //Get x movement in normal, 'z' in other
            float moveLeftRight;

            //Get 'z' movement in normal, 'x' in other
            float moveForwardBack;

            if(viewSide == ViewSide.fromNormal)
            {
                moveLeftRight = Input.GetAxis("Horizontal");
                moveForwardBack = Input.GetAxis("Vertical");

            }
            else //viewSide == ViewSide.fromSide
            {
                moveLeftRight = Input.GetAxis("Vertical");
                moveForwardBack = -Input.GetAxis("Horizontal");
            }
            //If the player is sprinting (holding the button down), double the speed, else keep it at the base value
            sprintMultiplier = (Input.GetKey(KeyCode.LeftShift)) ? 2 : 1;
            animator.speed = (Input.GetKey(KeyCode.LeftShift)) ? 2 : 1;



            //Get the movement vector
            Vector3 move;
            //Movement if not on ladder
            if (ladderState == LadderState.Off)
            {
                move = new Vector3(moveLeftRight, 0, moveForwardBack);
                physBody.SimpleMove(move * speed * sprintMultiplier);

                //Update the sprite
                updateSprite(move);
            }
            //Movement if climbing a ladder
            else if(ladderState == LadderState.Climbing)
            {
                move = new Vector3(transform.position.x + Input.GetAxis("Horizontal") * speed / 128f, transform.position.y + Input.GetAxis("Vertical") * speed / 32f, transform.position.z);
                if(move.y < 3.2f && move.y > .8f && Input.GetAxis("Horizontal") < .1f)
                    transform.position = move;

            }
            //Movement if getting off a ladder
            else// if(ladderState == LadderState.GettingOff)
            {
                move = new Vector3(transform.position.x + 1 * speed / 32f, transform.position.y, transform.position.z);
                if (move.y < 3.5f && move.y > 3f)
                    transform.position = move;
            }

            
        }
    }

    void updateSprite(Vector3 direction)
    {
        if (viewSide == ViewSide.fromNormal)
        {
            if (direction.x > 0.1f)
            {
                animator.enabled = true;
                animator.SetFloat("X", 1);
                animator.SetFloat("Z", 0);
                isIdle = false;
            }
            else if (direction.x < -0.1f)
            {
                animator.enabled = true;
                animator.SetFloat("X", -1);
                animator.SetFloat("Z", 0);
                isIdle = false;
            }
            else if (direction.z < -0.1f)
            {
                animator.enabled = true;
                animator.SetFloat("X", 0);
                animator.SetFloat("Z", -1);
                isIdle = false;
            }
            else if (direction.z > 0.1f)
            {
                animator.enabled = true;
                animator.SetFloat("X", 0);
                animator.SetFloat("Z", 1);
                isIdle = false;
            }
            else
            {
                animator.enabled = false;
                isIdle = true;
            }
        }
        else //viewSide == ViewSide.fromSide
        {
            if (direction.x > 0.1f)
            {
                animator.enabled = true;
                animator.SetFloat("Z", 1);
                animator.SetFloat("X", 0);
                isIdle = false;
            }
            else if (direction.x < -0.1f)
            {
                animator.enabled = true;
                animator.SetFloat("Z", -1);
                animator.SetFloat("X", 0);
                isIdle = false;
            }
            else if (direction.z < -0.1f)
            {
                animator.enabled = true;
                animator.SetFloat("Z", 0);
                animator.SetFloat("X", 1);
                isIdle = false;
            }
            else if (direction.z > 0.1f)
            {
                animator.enabled = true;
                animator.SetFloat("Z", 0);
                animator.SetFloat("X", -1);
                isIdle = false;
            }
            else
            {
                animator.enabled = false;
                isIdle = true;
            }
        }
        
    }

    public void updateSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(inHiding && collision.gameObject.tag == "Cloud")
        {
            Physics.IgnoreCollision(physBody, collision.collider, true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Set to climbing
        if(other.tag == "Climbable" && ladderState == LadderState.Off)
        {
            ladderState = LadderState.Climbing;
            animator.SetFloat("Z", 1);
            animator.enabled = false;
        }
        //Set to getting off
        if(other.tag == "End Climb")
        {
            ladderState = LadderState.GettingOff;
            animator.SetFloat("Z", 1);
            animator.enabled = true;
            //transform.position = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Check for getting off
        if (other.tag == "End Climb")
            ladderState = LadderState.Off;
        //Check for leaving ladder
        if (other.tag == "Climbable" && ladderState != LadderState.GettingOff)
            ladderState = LadderState.Off;


    }
}
