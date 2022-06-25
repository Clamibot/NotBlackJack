using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the sniper rifle object. It inherits from WeaponObject. This sniper rifle is semi-automatic.
/// </summary>
public class Sniper_Rifle : WeaponObject, Saveable
{
    public bool playerOwned = true;

    // Weapon constructor
    public Sniper_Rifle()
    {
        maxClipAmmo = 6;
        currentAmmo = 6;
        totalAmmo = 24;
        fireRate = 1;
        reloadTime = 4;
        weaponName = "Hydra";
        projectileVelocity = 100;
        projectileDamage = 200;
        projectileLife = 5;
    }

    // Use fixed update so game logic isn't tied to framerate
    void FixedUpdate()
    {
        if ((Input.GetKey(KeyCode.Mouse0) && playerOwned || fire) && timerReached && !reloading)
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
