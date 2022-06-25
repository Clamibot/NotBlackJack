using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script destroys explosion particles after a specified amount of time.
/// </summary>
public class Explosion : MonoBehaviour
{
    public float timeout;
    void Awake()
    {
        Invoke("DestroyNow", timeout);
    }

    void DestroyNow()
    {
        Destroy(gameObject);
    }
}
