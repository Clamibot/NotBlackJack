using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detected : MonoBehaviour
{
    public List<WeaponObject> weapons;


    private void Start()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        //If the player is detected
        if (other.tag == "Player")
        {
            toggleWeaponFiring(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        //If the player left
        if (other.tag == "Player")
        {
            toggleWeaponFiring(false);
        }
    }

    public void toggleWeaponFiring(bool shouldFire)
    {
        if(weapons.Capacity > 0)
            foreach(WeaponObject weapon in weapons)
                weapon.fire = shouldFire;
    }
}
