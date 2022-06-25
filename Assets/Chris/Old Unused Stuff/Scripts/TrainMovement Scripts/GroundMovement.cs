using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMovement : MonoBehaviour
{
    //Speed at which this object moves
    public float speed = 0.25f;

    //Gets the length to the left of this object
    private float xScale;
    //Where the current left boundary of the camera is in the world
    private float leftBoundary;
    //Counts number of ground pieces
    private static int groundNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        xScale = (this.GetComponent<Renderer>().bounds.max.x - this.GetComponent<Renderer>().bounds.min.x)/2;
        groundNum++;
    }

    // Update is called once per frame
    void Update()
    {
		//If the game is not paused
		if(!PauseMenu.isPaused)
		{
			//Get the left boundry of the camera
			leftBoundary = Camera.main.ScreenToWorldPoint(new Vector3(
				0, 
				Camera.main.transform.position.y, 
				Camera.main.transform.position.z)).x;


			//Start to move the object to the left
			this.transform.position = new Vector3(
				this.transform.position.x - speed,
				this.transform.position.y,
				this.transform.position.z);

			float rightPos = this.transform.position.x + xScale;
			//Debug.Log("Name: " + this.name + " Center: " + transform.position.x + " Right Side: " + xScale + " rightPos: " + rightPos + "\nLB: " + leftBoundary);
			//Check if this object has gone off screen
			if(this.transform.position.x + xScale < leftBoundary - xScale)
			{
				this.transform.position = new Vector3(
				this.transform.position.x + xScale*4*(0.5f* groundNum),
				this.transform.position.y,
				this.transform.position.z);
			}
		}
    }

    public static void resetObjectCount()
    {
        groundNum = 0;
    }
}
