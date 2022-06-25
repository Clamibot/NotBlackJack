using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This interface determines what objects can be interacted with.
/// The interact method is meant to trigger the interaction with the object.
/// </summary>
public interface Interactable
{
	void interact();
	
	// this might not be necessary, added for thought
	NS_Object getInteractable();
}
