using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This is the grenade throwing object. It inherits from WeaponObject. You can throw this object as fast as you can click.
/// </summary>
public class Grenade : WeaponObject, Saveable
{
    // Weapon constructor
    public Grenade()
    {
        totalAmmo = 6;
        weaponName = "Grenade";
        projectileVelocity = 10;
    }

    // Use update instead of fixedupdate for responsiveness
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Throw();
        }
    }
}
