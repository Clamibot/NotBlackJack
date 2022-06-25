using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator : MonoBehaviour
{
    //Getting the generator
    private Collider body;
    private float zScale;



    //Working with clouds
    [SerializeField] private GameObject cloud;
    [HideInInspector] public static int numClouds;
    [SerializeField] private int maxClouds = 10;
    [SerializeField] private float cloudFrequency = 1f;
    [SerializeField] private bool isSolidCloud = false;
    private int spot = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Set scale
        body = gameObject.GetComponent<Collider>();
        zScale = body.bounds.max.z - body.bounds.min.z;

        //Set cloud size
        cloud.transform.localScale = new Vector3(cloud.transform.localScale.x,
                                                cloud.transform.localScale.y,
                                                zScale/3);

        //Check if the clouds should be solid or not
        cloud.GetComponent<Collider>().enabled = isSolidCloud;

        //Set num of clouds
        numClouds = 0;

        InvokeRepeating("makeCloud",0f,cloudFrequency);
    }

    // Update is called once per frame
    private void makeCloud()
    {
        //Make this less frequent
        if(numClouds < maxClouds)
        {   
            //Create a cloud at a position within the generator
            Instantiate(cloud,
                gameObject.transform.position +
                    new Vector3(0,-cloud.transform.localScale.y,(spot = (spot + Random.Range(-1,2))%2)*zScale/3), 
                Quaternion.identity);
            //Increase num of clouds
            numClouds++;
        }
    }
    

}
