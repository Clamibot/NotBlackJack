using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the weaponizable can object. It inherits from WeaponObject.
/// </summary>
public class Can : WeaponObject, Saveable
{
    // Weapon constructor
    public Can()
    {
        maxClipAmmo = 1;
        currentAmmo = 1;
        totalAmmo = 1;
        projectileVelocity = 5;
        projectileDamage = 5;
        fireRate = 1;
    }
}
