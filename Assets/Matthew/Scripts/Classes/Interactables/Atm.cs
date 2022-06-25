using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the Atm object. It inherits from the InteractableObject class.
/// </summary>
/// 
public class Atm : InteractableObject, Saveable
{
    bool hacked;
    bool active;
    int hackLvlReq = 0;
    string objName = "Atm";
    public AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        hacked = false;
        active = true;
        hackLvlReq = 1;
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

    public void interact(AudioSource audio)
    {
        if (active == false)
        {
            audio.Play();
        }
        else if(active == true)
        {
            audio.Stop();
        }
    }
    // Use fixed update so game logic isn't tied to framerate
    void FixedUpdate()
    {
		//Possible if statement to check for hack level of character.
		//For now, if statement that will check if event has triggered a change hack value to true.
    }
}
