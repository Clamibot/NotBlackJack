using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the weaponizable shoe object. It inherits from WeaponObject.
/// </summary>
public class Shoe : WeaponObject, Saveable
{
    // Weapon constructor
    public Shoe()
    {
        maxClipAmmo = 1;
        currentAmmo = 1;
        totalAmmo = 1;
        projectileVelocity = 5;
        projectileDamage = 5;
        fireRate = 1;
    }
}

