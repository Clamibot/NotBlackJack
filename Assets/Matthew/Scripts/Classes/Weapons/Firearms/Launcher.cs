using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the SMG object. Essentially, it's a high fire rate assault rifle in pistol form factor. It inherits from WeaponObject. This weapon is a full-auto weapon.
/// </summary>
public class Launcher : WeaponObject, Saveable
{
    public bool playerOwned = false;

    // Weapon constructor
    public Launcher()
    {
        maxClipAmmo = 4;
        currentAmmo = 4;
        totalAmmo = 16;
        fireRate = 2;
        reloadTime = 4;
        weaponName = "Brute Death";
        projectileVelocity = 20;
        projectileDamage = 400;
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
