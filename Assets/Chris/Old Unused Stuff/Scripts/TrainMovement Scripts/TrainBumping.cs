using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainBumping : MonoBehaviour
{
    private float bumpHeight = 0.05f;
    private float bumpFrequency = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
		//Does not require to check if paused as it is affected by Time.timeScale
		
        //Burst 1
        InvokeRepeating("bumpDown", 2.0f, bumpFrequency);
        InvokeRepeating("bumpUp", 2.02f, bumpFrequency);

        //Burst 2
        InvokeRepeating("bumpDown", 2.5f, bumpFrequency);
        InvokeRepeating("bumpUp", 2.52f, bumpFrequency);

    }

    void bumpUp()
    {
        this.transform.position += Vector3.up * bumpHeight;
    }
    void bumpDown()
    {
        this.transform.position += Vector3.down * bumpHeight;
    }
}
