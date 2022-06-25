using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
	RANGED,
	MELEE,
	TARGET
}

/// <summary>
/// This is the "brain" of the AI. It requires the owning object to have the following scripts:<para></para>
/// Movement_AI, 
/// PlayerDetection, 
/// WaypointList, 
/// NavMeshAgent
/// </summary>
public class Enemy_AI : MonoBehaviour
{
	private Movement_AI mai;
	private PlayerDetection pd;
	private WaypointList wl;
	private Enemy_AI[] guards;
	public WeaponType weaponType = WeaponType.ASSAULT_RIFLE;
    public WeaponObject weapon;
	public EnemyType type;
	private AlertLevel alertness = AlertLevel.NEUTRAL;
	private Vector3 lastSightedLocation;
	public float meleeAttackDistance = 3.5f;
	public bool isStationaryGuard = false;
    public int health = 100;
	private float currentWanderTime = 0.0f, maxWanderTime = 3.5f;
	private Vector3 wanderTarget;
	private bool isWandering = false;
	private bool isFirstWander = true;
	private float timeSinceLastMelee = 1.5f, meleeAttackTime = 1.5f;
	private Properties playerProperties;
	private bool hasSeenPlayer = false;
	private bool isDistracted = false;
	private float currentDistractionTime = 0.0f, maxDistractTime = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
		mai = GetComponent<Movement_AI>();
		pd = GetComponent<PlayerDetection>();
		wl = GetComponent<WaypointList>();
		playerProperties = GameObject.FindGameObjectWithTag("Player").GetComponent<Properties>();

		GameObject[] gos = GameObject.FindGameObjectsWithTag("Enemy");

		bool shouldGetGuards = true;

		for(int i = 0; i < gos.Length; i++)
		{
			if(gos[i].GetComponent<Enemy_AI>() == null)
			{
				shouldGetGuards = false;
				break;
			}
		}

		if(shouldGetGuards)
		{
			guards = new Enemy_AI[gos.Length];
			for (int i = 0; i < gos.Length; i++)
			{
				guards[i] = gos[i].GetComponent<Enemy_AI>();
			}
		}

		if(mai == null || pd == null || wl == null)
		{
			Debug.Log("Error gathering components for Enemy_AI");
			this.enabled = false;
			Die();
		}

		if(isStationaryGuard)
		{
			mai.ShouldFollowRoute = false;
		}

		
    }

    // Update is called once per frame
    void Update()
    {
		think();

        if (health <= 0)
            Die();
    }

    //Bullet damage
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Bullet(Clone)")
        {
            Destroy(other.gameObject);
            //Debug.Log("hit" + health);
            health -= 10;
        }
    }
    private void think()
	{
		if(mai.isDistracted)
		{
			alertness = AlertLevel.SUSPICIOUS;
			isDistracted = true;
		}

		if(pd.CanSeePlayer)
		{
			alertness = AlertLevel.ALARMED;
			mai.CanSeePlayer = true;
			mai.ShouldFollowRoute = true;
			lastSightedLocation = pd.TargetLocation;
			isWandering = false;
			isFirstWander = true;
			hasSeenPlayer = true;
			soundTheAlarm();
		}
		else
		{
			mai.CanSeePlayer = false;
		}

		switch(alertness)
		{
			case AlertLevel.NEUTRAL:

				if(!mai.IsMoving)
				{
					mai.startMoving();
				}

				break;
			case AlertLevel.SUSPICIOUS:

				pd.FOVMultiplier = PlayerDetection.PartiallyAlertFOVMultiplier;

				handleSuspicions();

				break;
			case AlertLevel.ALARMED:

				mai.CurrentTarget = lastSightedLocation;
				pd.FOVMultiplier = PlayerDetection.FullyAlertFOVMultiplier;

				if(pd.CanSeePlayer == false && type != EnemyType.TARGET)
				{
					mai.startMoving();

					if(isFirstWander)
					{
						wanderTarget = lastSightedLocation;
					}

					mai.CurrentTarget = wanderTarget;

					if (Vector3.Distance(transform.position, wanderTarget) <= 2f || isWandering)
					{
						wander();
						isWandering = true;
					}
				}

				switch(type)
				{
					case EnemyType.RANGED:

						if(pd.CanSeePlayer)
						{
							mai.stopMoving();
							weapon.fire = true;
							rotateToPlayer();

							//Debug.Log("need more daka");
						}
						else
						{
							weapon.fire = false;
							//mai.startMoving();
						}

						break;

					case EnemyType.MELEE:

						if(Vector3.Distance(transform.position, mai.CurrentTarget) < meleeAttackDistance && !isWandering)
						{
							timeSinceLastMelee += Time.deltaTime;

							if(timeSinceLastMelee >= meleeAttackTime)
							{
								if(playerProperties != null)
								{
									//Debug.Log("wack.");
									playerProperties.health -= 2;
								}

								timeSinceLastMelee = 0.0f;
							}
							mai.stopMoving();
						}
						else
						{
							mai.startMoving();
						}

						break;

					case EnemyType.TARGET:

						soundTheAlarm();

						// R U N
						mai.startMoving();

						if (hasSeenPlayer)
						{
							isFirstWander = false;
							wander();
							mai.CurrentTarget = wanderTarget;
							isWandering = true;
						}

						break;
				}

				break;
		}
	}

	public void soundTheAlarm()
	{
		for(int i = 0; i < guards.Length; i++)
		{
			if(guards[i] == null)
			{
				continue;
			}

			if(Vector3.Distance(transform.position, guards[i].transform.position) < 10)
			{
				guards[i].alertness = AlertLevel.ALARMED;
				//Debug.Log("guard no " + (i + 1) + " has been alerted");
				guards[i].lastSightedLocation = lastSightedLocation;
			}
		}
	}

	public void dropWeapon()
	{

	}
    
    public void Die()
    {
		if(type == EnemyType.TARGET)
		{
			GameObject go = GameObject.Find("ExitBox");

			if(go != null)
			{
				ExitBox eb = go.GetComponent<ExitBox>();

				if(eb != null)
				{
					eb.canLoadLevel = true;
					eb.numTargets--;
				}
			}
		}

        Destroy(gameObject);
    }

	void rotateToPlayer()
	{
		if (type == EnemyType.TARGET)
		{
			return;
		}

		Vector3 target = pd.TargetLocation;
		target.y = transform.position.y;
		transform.LookAt(target);
	}

	public void setWanderPoint()
	{
		if(mai == null)
		{
			return;
		}

		NavMeshHit hit;

		Vector3 center = transform.position;

		do
		{
			wanderTarget = center + Random.insideUnitSphere * 100;
		} while (!NavMesh.SamplePosition(wanderTarget, out hit, 1.0f, NavMesh.AllAreas));

		mai.CurrentTarget = wanderTarget;

		Debug.DrawLine(transform.position, wanderTarget, Color.blue);
	}

	public virtual void wander()
	{
		currentWanderTime += Time.deltaTime;

		if (Vector3.Distance(transform.position, wanderTarget) <= 2.0f || currentWanderTime >= maxWanderTime || isFirstWander)
		{
			setWanderPoint();
			currentWanderTime = 0.0f;
			isFirstWander = false;
		}
	}

	public void handleSuspicions()
	{
		if(Vector3.Distance(transform.position, mai.CurrentTarget) < 3)
		{
			currentDistractionTime += Time.deltaTime;

			if (currentDistractionTime >= maxDistractTime)
			{
				isDistracted = false;
				currentDistractionTime = 0.0f;
				alertness = AlertLevel.NEUTRAL;
				pd.FOVMultiplier = PlayerDetection.DefaultPOVMultiplier;
				mai.isDistracted = false;
				mai.wasDistracted = true;
			}
		}
	}
}
