using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the LMG object. It inherits from WeaponObject. This weapon is a full-auto weapon.
/// </summary>
public class Assault_Rifle : WeaponObject, Saveable
{
    // Weapon constructor
    public Assault_Rifle()
    {
        maxClipAmmo = 100;
        currentAmmo = 100;
        totalAmmo = 400;
        fireRate = 0.1f;
        reloadTime = 2;
        weaponName = "MAC-5";
        projectileVelocity = 20;
        projectileDamage = 20;
        projectileLife = 5;
    }

    // Use fixed update so game logic isn't tied to framerate
    void FixedUpdate()
    {
        if (fire && timerReached && !reloading)
        {
            // Fire the weapon upon left click or throw the held object
            Fire();
            timerReached = false;
        }
        if (Input.GetKeyDown(KeyCode.R) && !reloading)
        {
            // Reload the weapon
            Reload();
        }
        if (!timerReached)
            timer += Time.deltaTime;

        if (!timerReached && timer > fireRate)
        {
            //Set to false so that We don't run this again
            timerReached = true;
            timer = 0;
        }
        if (reloading)
            reloadTimer += Time.deltaTime;

        if (reloading && reloadTimer > reloadTime)
        {
            //Set to false so that We don't run this again
            reloading = false;
            reloadTimer = 0;
        }
    }
}
