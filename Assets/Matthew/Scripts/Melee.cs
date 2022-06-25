using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    SphereCollider sc;
    Properties arms;
    public GameObject punch;
    public int meleePower = 20;

    private void Start()
    {
        sc = GetComponent<SphereCollider>();
        arms = GameObject.FindGameObjectWithTag("Player").GetComponent<Properties>();
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            sc.enabled = true;
            arms.SetArmsWeapon3();
            GameObject projectile = Instantiate(punch, transform.position, transform.rotation);
            Rigidbody fist = projectile.GetComponent<Rigidbody>();
            fist.AddForce(transform.forward * meleePower, ForceMode.Impulse);
        }
        else
        {
            sc.enabled = false;
        }
    }
}
