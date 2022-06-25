using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleSlidingDoor : MonoBehaviour
{
    private Vector3 startPositionDoor1;
    private Vector3 startPositionDoor2;
    private Vector3 endPositionDoor1;
    private Vector3 endPositionDoor2;
    private Vector3 destinationPositionDoor1;
    private Vector3 destinationPositionDoor2;
    public GameObject door1;
    public GameObject door2;
    public float offsetDistanceScale = 1;
    public float speed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        startPositionDoor1 = door1.transform.position;
        startPositionDoor2 = door2.transform.position;
        endPositionDoor1 = door1.transform.position - new Vector3(offsetDistanceScale, 0, 0);
        endPositionDoor2 = door2.transform.position + new Vector3(offsetDistanceScale, 0, 0);
        destinationPositionDoor1 = startPositionDoor1;
        destinationPositionDoor2 = startPositionDoor2;
    }

    // Update is called once per frame
    void Update()
    {
        door1.transform.position = Vector3.MoveTowards(door1.transform.position, destinationPositionDoor1, Time.deltaTime * speed);
        door2.transform.position = Vector3.MoveTowards(door2.transform.position, destinationPositionDoor2, Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other) // Players or enemies that get close to the door should trigger it to open
    {
        if (other.gameObject.name == "ThirdPersonController")
        {
            destinationPositionDoor1 = endPositionDoor1;
            destinationPositionDoor2 = endPositionDoor2;
        }
    }

    private void OnTriggerExit(Collider other) // The door should close if an object exits the sensor area
    {
        destinationPositionDoor1 = startPositionDoor1;
        destinationPositionDoor2 = startPositionDoor2;
    }
}
