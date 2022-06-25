using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPiece : MonoBehaviour
{
    //Objects
    [SerializeField] private Material complete;         //Holds the color for completion
    [SerializeField] private Material hallway;          //Holds the color for hallway
    //Variables
    private float heightSpawn = 35f;
    [HideInInspector] public bool isHallway = true;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(transform.position.x, heightSpawn, transform.position.z);
    }

    //Should only be called once in lifetime
    public void markAsComplete()
    {
        //Color as complete
        gameObject.GetComponent<MeshRenderer>().material = complete;
        //Show check mark
        gameObject.transform.Find("Checked").gameObject.SetActive(true);
    }
    
    //Should only be called once in lifetime
    public void markAsHallway()
    {
        //Color as hallway
        gameObject.GetComponent<MeshRenderer>().material = hallway;
    }
}
