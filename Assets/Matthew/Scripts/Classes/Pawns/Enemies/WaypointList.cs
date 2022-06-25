using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointList : MonoBehaviour
{
	public Waypoint[] waypoints;
	private MyWayPoint root, current;
	float time = 0.0f;
	public bool reverseAtEndpoint = false;
	// direction indicates the traversal direction of the waypoints.
	// forward is true, backwards is false
	private bool direction = true;
	private bool firstPass = true;
	private bool beenBuilt = false;

	// Start is called before the first frame update
	void Start()
	{
		buildWaypoints();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void buildWaypoints()
	{
		if (beenBuilt || waypoints.Length < 1)
		{
			return;
		}

		for (int i = 0; i < waypoints.Length - 1; i++)
		{
			addWaypoint(waypoints[i]);
		}

		addWaypoint(waypoints[waypoints.Length - 1], true);

		if (root != null)
		{
			current = root;

			while (current.isLast == false && current.next != null)
			{
				current = current.next;
			}

			root.prev = current;

			current = root;
		}

		beenBuilt = true;
	}

	public void addWaypoint(Waypoint w, bool isLastNode = false)
	{
		if(w == null)
		{
			return;
		}

		if (root == null)
		{
			root = new MyWayPoint(w.transform.position);
			return;
		}

		MyWayPoint current = root;

		while (current.next != null)
		{
			current = current.next;
		}

		current.next = new MyWayPoint(w.transform.position, current, (isLastNode ? root : null), isLastNode);
	}

	public Vector3 getNextWaypoint()
	{
		if (root == null || current == null)
		{
			return new Vector3(0.0f, 0.0f, 0.0f);
		}

		if (current == root || current.isLast)
		{
			if (firstPass)
			{
				firstPass = false;
			}
			else if (reverseAtEndpoint)
			{
				direction = !direction;
			}
		}

		Vector3 res = current.location;

		if (direction)
		{
			current = current.next;
		}
		else
		{
			current = current.prev;
		}

		return res;
	}

	internal class MyWayPoint
	{
		public Vector3 location;
		public MyWayPoint prev, next;
		public bool isLast;

		public MyWayPoint(Vector3 location) : this(location, null, null, false) { }

		public MyWayPoint(Vector3 location, MyWayPoint prev) : this(location, prev, null, false) { }

		// Complete Waypoint constructor
		public MyWayPoint(Vector3 location, MyWayPoint prev, MyWayPoint next, bool isLast)
		{
			this.location = location;
			this.prev = prev;
			this.next = next;
			this.isLast = isLast;
		}
	}
}
