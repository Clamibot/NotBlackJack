using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the Camera object. It inherits from the InteractableObject class.
/// </summary>
/// 
public class NS_Camera : InteractableObject, Saveable
{
    bool hacked;
    bool active;
    int hackLvlReq = 0;
    string objName;

    // Start is called before the first frame update
    void Start()
    {
        hacked = false;
        active = true;
        hackLvlReq = 1;
    }

    // Use fixed update so game logic isn't tied to framerate
    void FixedUpdate()
    {
		//Possible if statement to check for hack level of character.
		//For now, if statement that will check if event has triggered a change hack value to true.
    }
}