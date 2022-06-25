using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//FOV for field of vision angle
//agroTime for timer for activation of "noticing" player
//headHeight for the player's head (enemies will notice player's head)
public class PlayerDetection : MonoBehaviour
{
    public float FOV=90;
	public const float DefaultPOVMultiplier = 1.0f;
	public const float PartiallyAlertFOVMultiplier = 2.0f;
	public const float FullyAlertFOVMultiplier = 4.0f;
	private float fovmulti = 1.0f;

	private float sightRange = 63.6815f;
	private float sightRangeModifier = .25f;

	public float FOVMultiplier
	{
		get
		{
			return fovmulti;
		}

		set
		{
			fovmulti = value;
		}
	}

    public float headHeight = 1;
    //this is in seconds
    private float agroTime = 1.25f;
    private GameObject target;

	/// <summary>
	/// Lets other components determine where the current target is
	/// </summary>
	public Vector3 TargetLocation
	{
		get
		{
			if(target != null)
			{
				return target.transform.position;
			}

			return transform.position;
		}
	}

    private float seenTime;
    private bool seen;

	///<summary>
	/// Lets other components determine if the guard can see the player
	///</summary>
	public bool CanSeePlayer
	{
		get
		{
			return seen && isAgro;
		}
	}

    //You can set agro if spawning new enemy
    private bool isAgro { get; set; } = false;


    private void Start()
    {
        target = GameObject.FindWithTag("Player");
        FOV /= 2;
    }


    void Update()
    {
		if (isAgro)
			Debug.Log("i am angry >:c");

        // This is to keep raycasts from interacting with sprites (they will interact with player collider for vision)
        int layerMask = 1 << 5;
        layerMask = ~layerMask;


        //Setting position for raycast to head so enemies will see head if visible (instead of middle of body)
        Vector3 headPos = new Vector3 (target.transform.position.x, target.transform.position.y+headHeight, target.transform.position.z);

        //Getting direction and angle of vector to player
        var heading = headPos- transform.position;
        var direction = heading / heading.magnitude;
        float angle = Vector3.Angle(direction, transform.forward);

        RaycastHit hit;
        // Does the ray intersect any objects excluding the sprite layer
        if (angle > FOV * fovmulti)
        {
            Debug.DrawRay(transform.position, direction*10, Color.red);
			seen = false;
            //Debug.Log("Out of FOV");
        }
        else if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, layerMask) && hit.transform.tag == "Player")
        {
			if(angle > FOV)
			{
				transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, target.transform.position - transform.position, 5.0f * Time.deltaTime, 0.0f));
			}

			Debug.DrawRay(transform.position, direction * hit.distance, Color.green);

			if(Vector3.Distance(transform.position, hit.point) > sightRange * sightRangeModifier)
			{
				return;
			}

			//if was previously not seen set seen time (starts timer)
			if (!seen)
                seenTime = Time.time;

            seen = true;

            //Debug.Log("Did Hit");
            //if has been seen for agroTime interval
            if (Time.time-seenTime > agroTime)
            {
                isAgro = true;
            }
        }
        else
        {
            seen = false;
            seenTime = Time.time;
            Debug.DrawRay(transform.position, direction * 1000, Color.yellow);
            //Debug.Log("Did not Hit");
        }
    }
}
