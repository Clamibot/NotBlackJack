using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System;

/// <summary>
/// This is our base interactable object. It inherits from 
/// </summary>
/// 
public class InteractableObject : NS_Object, Interactable, Saveable
{
	public int triggeredObj = 0;
	public Door door;
	public Atm atm;
	public SecurityCamera securitycamera;
	public Terminal terminal;
	public Radio radio;
	public string objName;
	public Vector3 objLoc;
	public Movement_AI[] enemies;
	public AudioSource audioSource;

	// Start is called before the first frame update
	void Start()
	{
		door = gameObject.AddComponent(typeof(Door)) as Door;
		atm = gameObject.AddComponent(typeof(Atm)) as Atm;
		securitycamera = gameObject.AddComponent(typeof(SecurityCamera)) as SecurityCamera;
		terminal = gameObject.AddComponent(typeof(Terminal)) as Terminal;
		radio = gameObject.AddComponent(typeof(Radio)) as Radio;
		enemies = FindObjectsOfType<Movement_AI>();

		audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.clip = Resources.Load("siren") as AudioClip;

		GameObject[] gos = GameObject.FindGameObjectsWithTag("Enemy");

		if (gos != null)
		{
			enemies = new Movement_AI[gos.Length];

			for (int i = 0; i < gos.Length; i++)
			{
				Movement_AI tmai = gos[i].GetComponent<Movement_AI>();

				if (tmai != null)
				{
					enemies[i] = tmai;
				}
			}
		}
	}

	// Use fixed update so game logic isn't tied to framerate
	void FixedUpdate()
	{
		//Activates interactable once E is pressed and in trigger
		if (Input.GetKeyDown(KeyCode.E) && triggeredObj == 1)
		{
			switch (objName)
			{
				case "Door":
					door.interact(door);
					break;
				case "Radio":
					radio.interact(audioSource);
					for (int x = 0; x < enemies.Length; x++)
					{
						enemies[x].distract(gameObject.transform.position);
					}
					break;
				case "Terminal":
					terminal.interact(audioSource);
					for (int x = 0; x < enemies.Length; x++)
					{
						enemies[x].distract(gameObject.transform.position);
					}
					break;
				case "Atm":
					Debug.Log("interacted with atm");
					atm.interact(audioSource);
					for (int x = 0; x < enemies.Length; x++)
					{
                        Debug.Log("Distract Enemies");
						enemies[x].distract(gameObject.transform.position);
					}
					break;
				case "SecurityCamera":
					securitycamera.interact(audioSource);
					for (int x = 0; x < enemies.Length; x++)
					{
						enemies[x].distract(gameObject.transform.position);
					}
					break;
			}
		}
		//Only closes door once player has left trigger
		if (triggeredObj == 2)
		{
			switch (objName)
			{
				case "Door":
					if ((door.getActive()) == false)
					{
						door.setActive(true);
						door.interact(door);
					}
					break;
				case "Radio":
					if ((radio.getActive()) == false)
					{
						radio.setActive(true);
                        radio.interact(audioSource);
                    }
					break;
				case "Terminal":
					if ((terminal.getActive()) == false)
					{
						terminal.setActive(true);
                        terminal.interact(audioSource);
                    }
					break;
				case "Atm":
					if ((atm.getActive()) == false)
					{
						atm.setActive(true);
                        atm.interact(audioSource);
                    }
					break;
				case "SecurityCamera":
					if ((securitycamera.getActive()) == false)
					{
						securitycamera.setActive(true);
                        securitycamera.interact(audioSource);
                    }
					break;
			}
			triggeredObj = 0;
		}
	}
	//Allows interactable when entering trigger
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			try
			{
				door.setActive(false);
			}
			catch(Exception e) { }

			try
			{
				radio.setActive(false);
			}
			catch (Exception e) { }

			try
			{
				terminal.setActive(false);
			}
			catch (Exception e) { }

			try
			{
				atm.setActive(false);
			}
			catch (Exception e) { }

			try
			{
				securitycamera.setActive(false);
			}
			catch (Exception e) { }
		}
	}
	//When remaining in trigger, door can be moved
	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			triggeredObj = 1;
		}

	}
	//On exit of trigger, the door will close
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			triggeredObj = 2;
		}
	}

	void Saveable.save(string file)
	{

	}

	void Saveable.load(string file)
	{

	}

	void Interactable.interact()
	{
		//For interacting with objects.
	}

	// this might not be necessary, added for thought
	NS_Object Interactable.getInteractable()
	{
		return this;
	}
}