using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObject : MonoBehaviour
{
    //GameObjects
    [SerializeField] private GameObject hideInObject;
    private GameObject player;
    //private Renderer playerBody;
    private Collider hideInObjectBody;

    //private bool oldKeyDown = false;
    private bool inContact = false;
    //Variables
    private Vector3 originalScale;
    public string title;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //Get the original scale of the Object
        originalScale = hideInObject.transform.localScale;
        //Get player visuals
        //playerBody = player.GetComponent<SpriteRenderer>();
        //Get hiding object's collider
        hideInObjectBody = hideInObject.GetComponent<Collider>();
    }

    private void Update()
    {
        if (inContact && Input.GetKeyDown(KeyCode.F))
        {


            if (!Move.inHiding)
            {
                //Flip the player's hiding status
                Move.inHiding = true;

                Debug.Log("Player is now Hiding");
                //Make box not push player out
                hideInObjectBody.enabled = false;
                //Disapeer player
                //playerBody.enabled = false;
                //Expand object
                hideInObject.transform.transform.localScale *= 1.2f;
            }
            else if (Move.inHiding)
            {
                //Flip the player's hiding status
                Move.inHiding = false;

                Debug.Log("Player is now Visible");
                //Make box push player out
                hideInObjectBody.enabled = true;
                //Show player again
                //playerBody.enabled = true;
                //De-expand object
                hideInObject.transform.transform.localScale = originalScale;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        inContact = (other.tag == "Player");

        //For setting the objective text
        GameManager.canInteract = true;
        GameManager.objectiveTextString = "Press F To Hide In " + title;
    }
    private void OnTriggerExit(Collider other)
    {
        inContact = false;

        //For setting the objective text
        GameManager.canInteract = false;
    }
}
