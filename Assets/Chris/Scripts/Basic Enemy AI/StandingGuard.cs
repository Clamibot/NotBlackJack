using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingGuard : MonoBehaviour
{
    public int health = 100;            //How much health the enemy has
    public float sightRange;            //How far enemy can see
    public float speed;                  //Speed to move
    private const float timeToAggro = 0.6f;      //How much time it takes to become aggro'd/ENGAGED
    private const float timeToEvade = 3f;      //How much time should be spent in the chasing state
    private const float timeOfSuspicion = 5f; //How much time to stay suspicious
    public WeaponObject gun;                        //Access to gun
    private EnemyProperties enemyProperties;        //Acceses the properties of the enemy, like their weapon
    private BossEnemyProperties bossEnemyProperties;        //Acceses the properties of the boss enemy, like their weapon

    private Vector3 originalPosition;           //The place where they once stood
    private Quaternion originalRotation;        //Where they were originally looking
    [SerializeField] private Transform enemyUpDownRotation; //Used to point the enemy at the player, up and down wise

    public enum Engagement { NEUTRAL, SUSPICIOUS, CHASING, ENGAGED}
    public Engagement enemyState;  //Used for checking what the guard should be doing

    private Transform player;   //Tracks where the player is at
    private Vector3 playerHead;
    private Vector3 enemyHead;
    public float aggroTimer = 0f;  //Used to track how much time we have spent alerted but un-aggro'd
    public float evasionTimer = 0f;    //Used to track how much time has been spent chasing the player
    public float suspicionTimer = 0f;  //Used to track how much time the enemy has spent investigating an unusual occurance

    private Vector3 targetDestination;
    public float angle;
    // Start is called before the first frame update
    void Start()
    {
        //Get the original positions of the guard
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        //Get the player's transform
        player = GameObject.FindGameObjectWithTag("Player").transform;

        //If this guard has properties, get their current gun
        enemyProperties = GetComponent<EnemyProperties>();
        bossEnemyProperties = GetComponent<BossEnemyProperties>();
        if (enemyProperties != null)
            gun = enemyProperties.currentGun;
        if (bossEnemyProperties != null)
            gun = bossEnemyProperties.currentGun;
    }

    
    void LateUpdate()
    {
        //Check if we should change our alert level based upon player detection
        //think on what to do next based on the alert level
        //Check health

        checkPlayerDetection();
        think();

        if (enemyProperties == null && health <= 0)
            Die();
    }

    void checkPlayerDetection()
    {
        //Used to see if the enemy can see the player
        RaycastHit playerDetection;

        //Get the player's direction
        playerHead = new Vector3(player.position.x, player.position.y + 4, player.position.z);
        enemyHead = new Vector3(transform.position.x, transform.position.y + 4, transform.position.z);

        Vector3 direction = playerHead - enemyHead;
        //direction.y = 0;

        //Shoot a ray from the enemy towards the player
        if(Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), direction, out playerDetection, sightRange))
        {
            //If we get a collision, we check to see if its tag is of the player
            if(playerDetection.collider.tag == "Player")
            {
                //Get the position of the last place player was seen
                targetDestination = playerDetection.collider.gameObject.transform.position;

                //If we were chasing, then get engaged
                if(enemyState == Engagement.CHASING)
                {
                    //Set the guard state to being engaged
                    enemyState = Engagement.ENGAGED;
                }

                //If we are not engaged then do not deal with aggro and suspicion
                if (enemyState != Engagement.ENGAGED)
                {
                    if (aggroTimer > timeToAggro)
                    {
                        //Set the guard state to being engaged
                        enemyState = Engagement.ENGAGED;

                        //Reset the timer
                        aggroTimer = 0f;
                    }
                    else
                    {
                        //Set the guard to a suspicious state
                        enemyState = Engagement.SUSPICIOUS;

                        //Increment time to aggro
                        aggroTimer += Time.deltaTime;
                    }
                }
                

            }
            //If we don't see the player and we were engaged, start chasing last known location
            else if(enemyState == Engagement.ENGAGED)
            {
                enemyState = Engagement.CHASING;
            }
            //If enemy doesn't see the player and are not chasing them
            else if(enemyState != Engagement.CHASING && enemyState != Engagement.SUSPICIOUS)
            {
                //Then reset the aggro timer
                aggroTimer = 0f;
            }
        }
        //If we don't see the player and we were engaged, start chasing last known location
        else if (enemyState == Engagement.ENGAGED)
        {
            enemyState = Engagement.CHASING;
        }

    }

    void think()
    {
        //For each state when applicable:
        //  Deal with timer
        //  Check gun
        //  Move to target


        switch(enemyState)
        {
            //If nothing to do then return to post if not there
            case (Engagement.NEUTRAL):
                //Go back to post at a slower speed
                returnToPost(0.1f, 0.001f);
            break;

            //If suspicious activity detected, go check it out
            case (Engagement.SUSPICIOUS):
                //Count time to evade
                suspicionTimer += Time.deltaTime;
                //If player has evaded long enough, set enemy to a suspicious state
                if (suspicionTimer > timeOfSuspicion)
                {
                    suspicionTimer = 0f;
                    aggroTimer = 0f;
                    enemyState = Engagement.NEUTRAL;
                }
                //Go to the suspicious area
                moveToTarget(targetDestination, .4f, 45f);

                //If we are there and nothing is happening, then go to neutral state
                if (Vector3.Distance(targetDestination, transform.position) <= 0.5f && aggroTimer == 0f)
                    enemyState = Engagement.NEUTRAL;
            break;

            //If we were engaged, chase after the player's last known location
            case (Engagement.CHASING):
                //Count time to evade
                evasionTimer += Time.deltaTime;
                //If player has evaded long enough, set enemy to a suspicious state
                if(evasionTimer > timeToEvade)
                {
                    evasionTimer = 0f;
                    enemyState = Engagement.SUSPICIOUS;
                }

                //If enemy has a gun, stop firing
                if (gun != null)
                    gun.fire = false;

                //Go after the player
                moveToTarget(targetDestination, .8f, 60f);
            break;

            //If the player has been spotted, attack
            case (Engagement.ENGAGED):
                //Reset evasion timer
                evasionTimer = 0f;

                //If enemy has a gun, aim and start firing
                if (gun != null)
                {
                    //aimUpDown();    //Aim the enemy towards the player up and down wise
                    //enemyUpDownRotation.localRotation = Quaternion.RotateTowards(enemyUpDownRotation.rotation, Quaternion.FromToRotation(-enemyUpDownRotation.up, (player.position - enemyUpDownRotation.position)), 360.0f);

                    gun.fire = true;
                }

                //If we are not too close to the player, move towards them
                if (Vector3.Distance(targetDestination, transform.position) > 5f)
                    moveToTarget(targetDestination, .8f, 45f);
                //If we are super close then turn around faster
                else
                    rotateToTarget(targetDestination, 2.25f);


            break;

        }
    }

    //*****Neutral state**********************************************************************
    void returnToPost(float relativeSpeed, float angleOfTurning)
    {
        //Check are we in our post geographically
        if(inPost())
        {
            //If we are, check if we are rotated to where we should be pointing
            if(!inPostRotation())
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, 0.9f * Time.deltaTime); //Turn over time
            }
        }
        //If we are not
        else
        {
            //Move to our post
            moveToTarget(originalPosition, relativeSpeed, angleOfTurning);
        }
    }

    bool inPost()
    {
        return (transform.position == originalPosition) || Vector3.Distance(transform.position,originalPosition) <= 3f;
    }

    bool inPostRotation()
    {
        return (transform.rotation == originalRotation) || Mathf.Abs(Quaternion.Angle(transform.rotation,originalRotation)) <= 3f;
    }
    
    //*****Aiming*************************************************************************

    //Aims the enemy up and down based on the player's location, relative to their horizon.
    private void aimUpDown()
    {
        //Rotate only if the enemy has a spine.
        if (enemyUpDownRotation != null)
        {
            Vector3 playerRotationCorrection = new Vector3(playerHead.x, playerHead.y + 5, playerHead.z);

            Vector3 enemyToPlayer = playerRotationCorrection - enemyHead;       //Get vector between enemy and player
            //if(enemyToPlayer.y > -0.01)
            
            Vector3 enemyToPlayerHorizon = new Vector3(enemyToPlayer.x, transform.position.y, enemyToPlayer.z); //Get vector between enemy and player, but staying on the enemy's horizon.
            angle = Vector3.Angle(enemyToPlayerHorizon, enemyToPlayer);

            //TODO begininning idea to fix the aiming by adding or subtracting some amount to the angle based off the player and enemy's total distance
            //angle += Mathf.Sqrt(Mathf.Pow(enemyToPlayer.x, 2) + Mathf.Pow(enemyToPlayer.y, 2) + Mathf.Pow(enemyToPlayer.z, 2));


            if (playerHead.y - 0.5 <= enemyHead.y)
                angle *= -1;
            if(Vector3.Distance(playerHead,enemyHead) < 6)
                angle = Mathf.Clamp(angle, -60, 90);    //Make sure the enemy doesn't snap their back
            else
                angle = Mathf.Clamp(angle, -30, 30);    //Make sure the enemy doesn't snap their back
            enemyUpDownRotation.localRotation = Quaternion.Euler(5, 0,  0 - angle);     //Aim the enemy towards the player up and down wise. (Do a slight angle adjustment
        }
    }

    //*****Movement***********************************************************************
    Quaternion rotateToTarget(Vector3 target, float relativeSpeed)
    {
        //Get the target's direction
        Vector3 directionPosition = target - transform.position;
        directionPosition.y = 0;

        //Get the direction change from current to target
        Quaternion directionRotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionPosition), 0.9f * relativeSpeed * Time.deltaTime); //Turn over time

        //Rotate to target
        transform.rotation = directionRotation;

        //Return our change in rotation
        return directionRotation;
    }

    void moveToTarget(Vector3 target, float relativeSpeed, float angleOfTurning)
    {
        //Rotate to target
        Quaternion directionRotation = rotateToTarget(target,relativeSpeed * 2f);

        //If we are not pointed faraway from target, start moving to target
        if (Mathf.Abs(Quaternion.Angle(transform.rotation, directionRotation)) <= angleOfTurning)
        {
            transform.Translate(0, 0, speed * relativeSpeed * Time.deltaTime, Space.Self);
        }
    }

    //*****Life & Death*******************************************************************************************************************************************************

    //Bullet damage
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Bullet(Clone)" || other.gameObject.name == "Explosive Plasma Bolt(Clone)" || other.gameObject.name == "Explosion(Clone)" || other.gameObject.name == "Melee(Clone)")
        {
            Rigidbody bullet = other.gameObject.GetComponent<Rigidbody>();
            health -= (int)bullet.mass;
            //Debug.Log("hit" + health);
        }
    }

    public void Die()
    {
        GameObject go = GameObject.Find("ExitBox");
        if (go != null)
        {
            ExitBox eb = go.GetComponent<ExitBox>();

            if (eb != null)
            {
                eb.canLoadLevel = true;
                eb.numTargets--;
            }
        }
        Destroy(gameObject);
    }
}
