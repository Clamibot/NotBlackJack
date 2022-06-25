using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the weaponizable board object. It inherits from WeaponObject.
/// </summary>
public class Board : WeaponObject, Saveable
{
    // Weapon constructor
    public Board()
    {
        maxClipAmmo = 1;
        currentAmmo = 1;
        totalAmmo = 1;
        projectileVelocity = 5;
        projectileDamage = 5;
        fireRate = 1;
    }
}
