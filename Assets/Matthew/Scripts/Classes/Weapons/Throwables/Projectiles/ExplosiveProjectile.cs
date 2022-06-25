using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectile : MonoBehaviour
{
    public AudioClip explosionSound; // The explosion sound
    public GameObject explosionEffect; // The grenade's explosion effect

    private void OnTriggerEnter(Collider other)
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);
        StartCoroutine(playAudio(explosionSound));
        Destroy(gameObject); // Destroy the projectile
    }

    IEnumerator playAudio(AudioClip soundEffect)
    {
        AudioSource.PlayClipAtPoint(soundEffect, transform.position);
        yield break;
    }
}