using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProperties : MonoBehaviour
{
    public enum GunType { Pistols, AssualtRifle, Sniper, RPG}
    public GunType heldWeapon = GunType.AssualtRifle;

    public int health = 100;
    [HideInInspector] public WeaponObject currentGun;
    public GameObject gun1;
    public GameObject gun2;
    public GameObject gun3;
    public GameObject gun4;
    public GameObject secondPistol;
    private WeaponObject secondPistolWeaponObject;
    

    public GameObject lArmUp, lArmDown, rArmUp, rArmDown;
    //Hand positions for the spiker pistol
    private Quaternion lArmUpR1 = Quaternion.Euler(new Vector3(80f, 72f, 85f));
    private Quaternion lArmDownR1 = Quaternion.Euler(new Vector3(10f, 3f, 45f));
    private Quaternion rArmUpR1 = Quaternion.Euler(new Vector3(-80f, 18f, 5f));
    private Quaternion rArmDownR1 = Quaternion.Euler(new Vector3(-10f, 3f, 45f));
    //Hand positions for the ac-5 assault rifle
    private Quaternion lArmUpR2 = Quaternion.Euler(new Vector3(10f, 20f, 0f));
    private Quaternion lArmDownR2 = Quaternion.Euler(new Vector3(10f, 3f, 45f));
    private Quaternion rArmUpR2 = Quaternion.Euler(new Vector3(-10f, -20f, 0f));
    private Quaternion rArmDownR2 = Quaternion.Euler(new Vector3(-70f, 3f, 45f));
    //Hand positions for the hydra sniper rifle
    private Quaternion lArmUpR3 = Quaternion.Euler(new Vector3(10f, 20f, 0f));
    private Quaternion lArmDownR3 = Quaternion.Euler(new Vector3(10f, 3f, 45f));
    private Quaternion rArmUpR3 = Quaternion.Euler(new Vector3(-85f, -20f, 0f));
    private Quaternion rArmDownR3 = Quaternion.Euler(new Vector3(-5f, 3f, 45f));
    //Hand positions for the brute death rocket launcher
    private Quaternion lArmUpR4 = Quaternion.Euler(new Vector3(10f, 20f, 0f));
    private Quaternion lArmDownR4 = Quaternion.Euler(new Vector3(10f, 3f, 45f));
    private Quaternion rArmUpR4 = Quaternion.Euler(new Vector3(-60f, -20f, 0f));
    private Quaternion rArmDownR4 = Quaternion.Euler(new Vector3(-50f, 3f, 45f));
    

    // Start is called before the first frame update
    void Start()
    {
        if (heldWeapon == GunType.Pistols)
        {
            gun1.gameObject.SetActive(true);
            secondPistol.gameObject.SetActive(true);
            currentGun = gun1.GetComponent(typeof(WeaponObject)) as WeaponObject;
            secondPistolWeaponObject = secondPistol.GetComponent(typeof(WeaponObject)) as WeaponObject;
        }
        if (heldWeapon == GunType.AssualtRifle)
        {
            gun2.gameObject.SetActive(true);
            currentGun = gun2.GetComponent(typeof(WeaponObject)) as WeaponObject;

        }
        if (heldWeapon == GunType.Sniper)
        {
            gun3.gameObject.SetActive(true);
            currentGun = gun3.GetComponent(typeof(WeaponObject)) as WeaponObject;

        }
        if (heldWeapon == GunType.RPG)
        {
            gun4.gameObject.SetActive(true);
            currentGun = gun4.GetComponent(typeof(WeaponObject)) as WeaponObject;

        }


    }

    // Update is called once per frame
    void Update()
    {
        //Check health
        if (health <= 0)
            Die();
        
        //Check if one of the pistols is firing or not if the enemy has pistols
        if(heldWeapon == GunType.Pistols)
        {
            //Make sure both guns are firing or not at all
            secondPistolWeaponObject.fire = currentGun.fire;
        }
    }
    private void LateUpdate()
    {
        if (heldWeapon == GunType.Pistols)
        {
            SetArmsWeapon1();
        }
        if (heldWeapon == GunType.AssualtRifle)
        {
            SetArmsWeapon2();
        }
        if (heldWeapon == GunType.Sniper)
        {
            SetArmsWeapon3();
        }
        if (heldWeapon == GunType.RPG)
        {
            SetArmsWeapon4();
        }
    }
    void SetArmsWeapon1()
    {
        rArmUp.transform.localRotation = rArmUpR1;
        rArmDown.transform.localRotation = rArmDownR1;
        lArmDown.transform.localRotation = lArmDownR1;
        lArmUp.transform.localRotation = lArmUpR1;
    }
    void SetArmsWeapon2()
    {
        rArmUp.transform.localRotation = rArmUpR2;
        rArmDown.transform.localRotation = rArmDownR2;
        lArmDown.transform.localRotation = lArmDownR2;
        lArmUp.transform.localRotation = lArmUpR2;
    }
    void SetArmsWeapon3()
    {
        rArmUp.transform.localRotation = rArmUpR3;
        rArmDown.transform.localRotation = rArmDownR3;
        lArmDown.transform.localRotation = lArmDownR3;
        lArmUp.transform.localRotation = lArmUpR3;
    }
    void SetArmsWeapon4()
    {
        rArmUp.transform.localRotation = rArmUpR4;
        rArmDown.transform.localRotation = rArmDownR4;
        lArmDown.transform.localRotation = lArmDownR4;
        lArmUp.transform.localRotation = lArmUpR4;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Bullet(Clone)")
        {
            //Debug.Log("hit"+health);
            //health -= 1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Bullet(Clone)" || other.gameObject.name == "Explosive Plasma Bolt(Clone)" || other.gameObject.name == "Explosion(Clone)" || other.gameObject.name == "Melee(Clone)")
        {
            Rigidbody bullet = other.gameObject.GetComponent<Rigidbody>();
            health -= (int)bullet.mass;
            //Debug.Log("hit" + health);
        }
    }

    private void Die()
    {
        
        Destroy(gameObject);
    }
}
