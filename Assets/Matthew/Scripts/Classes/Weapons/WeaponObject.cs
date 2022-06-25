using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

public enum WeaponType
{
	ASSAULT_RIFLE,
	LAUNCHER,
	SNIPER,
	PISTOL,
	MELEE
}

/// <summary>
/// This is our base weapon object. It inherits from Weapon.
/// </summary>
public class WeaponObject : NS_Object, Saveable
{
    // Object attributes
    public int maxClipAmmo; // The maximum size of a weapon's clip
    public int currentAmmo; // The current number of bullets left in the clip
    public int totalAmmo; // The total numbber of bullets for the weapon the player has on hand
    public float fireRate; // The fire rate of this weapon
    public float reloadTime; // The time it takes to reload this weapon
    public string weaponName; // The weapon's name
    public AudioClip fireSound; // The weapon's firing sound
    public AudioClip reloadSound; // The weapon's reloading sound
    public AudioClip dryFireSound; // The sound the weapon should play if there are no bullets left
    public GameObject projectilePrefab; // The projectile the weapon launches. This can be either a bullet or the throwable object.
    public Transform projectileSpawn; // The projectile spawn point
    public float projectileVelocity; // The velocity of the projectile for this weapon
    public int projectileDamage; // The damage that the projectile from this weapon should deal
    public float projectileLife; // The lifetime of the projectile
    public float timer = 0;
    public bool timerReached = true;
    public float reloadTimer = 0;
    public bool reloading = false;
	public bool fire = false;

    public void Fire() // Spawns a bullet for firearms. Spawns a projectile for throwables.
    {
        if (totalAmmo == 0)
        {
            print("Out Of Ammunition!");
            StartCoroutine(playAudio(dryFireSound));
        }
        else
        {
            currentAmmo -= 1;
            totalAmmo -= 1;
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.transform.position = projectileSpawn.position;
            Vector3 rotation = projectile.transform.rotation.eulerAngles;
            projectile.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);
            Rigidbody bullet = projectile.GetComponent<Rigidbody>();
            bullet.AddForce(projectileSpawn.forward * projectileVelocity, ForceMode.Impulse);
            bullet.mass = projectileDamage;
            StartCoroutine(DestroyProjectileAfterTime(projectile, projectileLife));
            StartCoroutine(playAudio(fireSound));
            if (currentAmmo == 0 && totalAmmo != 0)
                Reload(); // Auto reload for the player if the clip is empty after the last shot
        }
    }

    public void Reload() // Used only for firearms
    {
        if (currentAmmo == maxClipAmmo) // If the clip is already full, do nothing
            print("Clip Already Full!");
        else if (totalAmmo < maxClipAmmo) // Prevents the game from giving the player phantom bullets when there are not enough bullets left for a full reload
        {
            if (currentAmmo < totalAmmo)
            {
                currentAmmo = totalAmmo;
                reloading = true;
                StartCoroutine(playAudio(reloadSound));
            }
            else
            {
                print("Reload Not Possible, Not Enough Bullets Left!");
            }
        }
        else // Allow players to reload a clip that's not fully empty
        {
            currentAmmo = maxClipAmmo;
            reloading = true;
            StartCoroutine(playAudio(reloadSound));
        }
    }

    public void Throw() // Used only for throwables
    {
        if (totalAmmo == 0)
        {
            print("No Objects Left To Throw!");
            StartCoroutine(playAudio(dryFireSound));
        }
        else
        {
            totalAmmo -= 1;
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.transform.position = projectileSpawn.position;
            Vector3 rotation = projectile.transform.rotation.eulerAngles;
            projectile.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);
            projectile.GetComponent<Rigidbody>().AddForce(projectileSpawn.forward * projectileVelocity, ForceMode.Impulse);
            StartCoroutine(playAudio(fireSound));
        }
    }

    IEnumerator playAudio(AudioClip soundEffect)
    {
        AudioSource.PlayClipAtPoint(soundEffect, transform.position);
        yield break;
    }

    private IEnumerator DestroyProjectileAfterTime(GameObject projectile, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(projectile);
    }

    void Saveable.save(string file)
    {

    }

    void Saveable.load(string file)
    {

    }

    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
        // sample code from MSDN, might need later

        // Use the AddValue method to specify serialized values.
        //info.AddValue("props", myProperty_value, typeof(string));
    }

    // The special constructor is used to deserialize values.
    void MyItemType(SerializationInfo info, StreamingContext context)
    {
        // sample code from MSDN, might need later

        // Reset the property value using the GetValue method.
        //myProperty_value = (string)info.GetValue("props", typeof(string));
    }

    // Returns the maximum amount of ammo in this weapon's clip
    public int getAmmoMax()
    {
        return maxClipAmmo;
    }

    // Returns the current ammo in this weapon's clip
    public int getAmmoCurrent()
    {
        return currentAmmo;
    }

    // Returns the total amount of ammo available for this weapon to use
    public int getAmmoTotal()
    {
        return totalAmmo;
    }

    // this might not be necessary, added for thought
    NS_Object getWeapon()
    {
        return this;
    }
}