using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the Door object. It inherits from the InteractableOnject class.
/// </summary>
/// 
public class Door : InteractableObject, Saveable
{
    bool hacked;
    bool active;
    int hackLvlReq = 0;
	bool hasBeenActivated = false;
    string objName = "Door";
	float activeTime = 0.0f, maxActiveTime = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        hacked = false;
        active = false;
        hackLvlReq = 1;
    }

    // Use fixed update so game logic isn't tied to framerate
    void FixedUpdate()
    {
		//Possible if statement to check for hack level of character.
		//For now, if statement that will check if event has triggered a change hack value to true.
    }

	void Update()
	{
		if(hasBeenActivated)
		{
			activeTime += Time.deltaTime;

			if(activeTime >= maxActiveTime)
			{
				// shut the door
				transform.Translate(0, -3.5f, 0);
				hasBeenActivated = false;
				active = false;
				activeTime = 0.0f;
			}
		}
	}

    public void setActive(bool choice)
    {
        active = choice;
    }

    public bool getActive()
    {
        return active;
    }

    public string getObjName()
    {
        return objName;
    }
    public void interact(Door door)
    {
		//Debug.Log("interacted");
        if (door.getActive() == false)
        {
            //Debug.Log("Move up?");
            door.transform.Translate(0,3.5f,0);
			hasBeenActivated = true;
        }
		if (door.getActive() == true && hasBeenActivated == true && activeTime >= maxActiveTime)
        {
            //Debug.Log("Move down?");
            door.transform.Translate(0,-3.5f,0);
			hasBeenActivated = false;
        }
    }
}