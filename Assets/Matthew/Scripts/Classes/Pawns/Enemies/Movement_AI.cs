using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement_AI : MonoBehaviour
{
	private WaypointList route;
	private Vector3 currentTarget;
	public float speed = 2.5f;
	private NavMeshAgent nma;
	public float distanceTolerance = 2f;
	private bool canSeePlayer = false;
	private bool shouldMoveOnRoute = true;
	private bool distracted = false;
	public bool wasDistracted = false;
	public Vector3 currentDistractionLocation;
	private Vector3 preDistractionTarget;

	public bool isDistracted
	{
		get { return distracted; }
		set { distracted = value; }
	}

	public bool IsMoving
	{
		get
		{
			return !nma.isStopped;
		}
	}

	public Vector3 CurrentTarget
	{
		get
		{
			return currentTarget;
		}

		set
		{
			currentTarget = value;
			if(nma != null)
			{
				nma.SetDestination(currentTarget);
			}
		}
	}

	public bool CanSeePlayer
	{
		get
		{
			return canSeePlayer;
		}

		set
		{
			canSeePlayer = value;
		}
	}

	public bool ShouldFollowRoute
	{
		get
		{
			return shouldMoveOnRoute;
		}

		set
		{
			shouldMoveOnRoute = value;
		}
	}

	// Start is called before the first frame update
	private void Start()
	{
		route = GetComponent<WaypointList>();
		route.buildWaypoints();

		nma = GetComponent<NavMeshAgent>();
		nma.speed = speed;

		if(shouldMoveOnRoute == false || route.waypoints.Length == 0)
		{
			nma.destination = transform.position;
			nma.isStopped = true;
			Debug.Log("I told you i'm staying right here");
		}
		else
		{
			setNextTarget();
		}
	}

    // Update is called once per frame
    void Update()
    {
		if (nma.isStopped == true || shouldMoveOnRoute == false)
		{
			return;
		}

		if(wasDistracted)
		{
			currentTarget = preDistractionTarget;
			wasDistracted = false;
		}

		if(canSeePlayer == false && Vector3.Distance(transform.position, currentTarget) <= distanceTolerance)
		{
			if(!isDistracted)
			{
				setNextTarget();
			}
		}

		nma.SetDestination(currentTarget);
    }

	public void setNextTarget()
	{
		currentTarget = route.getNextWaypoint();
	}

	public void startMoving()
	{
		nma.isStopped = false;
	}

	public void stopMoving()
	{
		nma.isStopped = true;
	}

    public void distract(Vector3 objLoc)
    {
		if(isDistracted)
		{
			return;
		}

		preDistractionTarget = CurrentTarget;

		currentDistractionLocation = objLoc;

		if (Vector3.Distance(transform.position, currentDistractionLocation) < 25)
		{
			CurrentTarget = currentDistractionLocation;

			isDistracted = true;
		}
	}
}