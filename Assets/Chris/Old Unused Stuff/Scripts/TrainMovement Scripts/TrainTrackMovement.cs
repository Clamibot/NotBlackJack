using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainTrackMovement : MonoBehaviour
{
    //Speed at which this object moves
    public float speed = 0.25f;

    //Gets the length to the left of this object
    private float xScale;
    //Counts the number of track pieces
    private static int trackNum = 0;
    //Where the current left boundary of the camera is in the world
    private float leftBoundary;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(transform.Find("Close Rod").name);
        xScale = (this.transform.Find("Close Rod").gameObject.GetComponent<Renderer>().bounds.max.x 
            - this.transform.Find("Close Rod").gameObject.GetComponent<Renderer>().bounds.min.x) 
            / 2;
        trackNum++;

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
			if (this.transform.position.x + xScale < leftBoundary - xScale*1.5f)
			{
				this.transform.position = new Vector3(
				this.transform.position.x + xScale * 3.95f * (0.5f * trackNum),
				this.transform.position.y,
				this.transform.position.z);
			}
		}
    }

    public static void resetObjectCount()
    {
        trackNum = 0;
    }
}
