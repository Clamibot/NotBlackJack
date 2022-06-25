using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the weaponizable vase object. It inherits from WeaponObject.
/// </summary>
public class Vase : WeaponObject, Saveable
{
    // Weapon constructor
    public Vase()
    {
        maxClipAmmo = 1;
        currentAmmo = 1;
        totalAmmo = 1;
        projectileVelocity = 5;
        projectileDamage = 5;
        fireRate = 1;
    }
}
