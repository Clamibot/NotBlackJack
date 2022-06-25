using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    public float timer; // the grenade's countdown timer
    private bool hasExploded = false;
    public AudioClip explosionSound; // The explosion sound
    public GameObject explosionEffect; // The grenade's explosion effect
    public GameObject explosion; // The object containing the collider that does damage to objects in a specified radius
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Use fixed update so game logic isn't tied to framerate
    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    public void Explode()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Instantiate(explosion, transform.position, transform.rotation);
        StartCoroutine(playAudio(explosionSound));
        Destroy(gameObject); // Destroy the grenade
    }

    IEnumerator playAudio(AudioClip soundEffect)
    {
        AudioSource.PlayClipAtPoint(soundEffect, transform.position);
        yield break;
    }
}