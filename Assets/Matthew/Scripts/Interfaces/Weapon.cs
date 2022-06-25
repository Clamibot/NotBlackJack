using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This interface determines what objects are weapons.
/// The getAmmoMax and getAmmoCurrent methods are used to get the weapon stats regarding ammo.
/// </summary>
public interface Weapon
{
    int getAmmoMax();
    int getAmmoCurrent();
    int getAmmoTotal();

    // this might not be necessary, added for thought
    NS_Object getWeapon();
}
