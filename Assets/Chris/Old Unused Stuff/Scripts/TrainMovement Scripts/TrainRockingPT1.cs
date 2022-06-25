using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainRockingPT1 : MonoBehaviour
{
    private float rockDegree = 0.3125f;
    private float rockFrequency = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
		//Does not require to check if paused as it is affected by Time.timeScale
		
        //To the right
        InvokeRepeating("rockRight", 2.0f, rockFrequency);
        InvokeRepeating("rockRight", 2.1f, rockFrequency);
        InvokeRepeating("rockLeft" , 2.2f, rockFrequency);
        InvokeRepeating("rockLeft" , 2.3f, rockFrequency);

        //To the left
        InvokeRepeating("rockLeft" , 2.4f, rockFrequency);
        InvokeRepeating("rockLeft" , 2.5f, rockFrequency);
        InvokeRepeating("rockRight", 2.6f, rockFrequency);
        InvokeRepeating("rockRight", 2.7f, rockFrequency);



        //To the left
        InvokeRepeating("rockLeft" , 3.0f, rockFrequency);
        InvokeRepeating("rockLeft" , 3.1f, rockFrequency);
        InvokeRepeating("rockRight", 3.2f, rockFrequency);
        InvokeRepeating("rockRight", 3.3f, rockFrequency);

        //To the right
        InvokeRepeating("rockRight", 3.4f, rockFrequency);
        InvokeRepeating("rockRight", 3.5f, rockFrequency);
        InvokeRepeating("rockLeft" , 3.6f, rockFrequency);
        InvokeRepeating("rockLeft" , 3.7f, rockFrequency);
    }

    void rockRight()
    {
        this.transform.Rotate(-rockDegree,0,0);
    }
    void rockLeft()
    {
        this.transform.Rotate(rockDegree,0,0);
    }
}
