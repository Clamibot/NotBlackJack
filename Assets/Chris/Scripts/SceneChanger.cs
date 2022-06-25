using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    //Variables
    public string nextRoom;
    [SerializeField] private string nextScene;
    [SerializeField] private bool isInteract = false;
    private bool inContact = false;

    //private DialogueTrigger blockDialouge;

    private void Start()
    {
    //    blockDialouge = GetComponent<DialogueTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inContact && Input.GetKeyDown(KeyCode.F))
        {
            ChangeScene();
        }
    }

    private void ChangeScene()
    {
        //Reset static variable counts (Train track nums and ground nums)
        GameManager.resetStaticBackgroundItems();
        if (Application.CanStreamedLevelBeLoaded(nextScene))
        {


            //Load the next scene
            SceneManager.LoadScene(nextScene);
            if (nextScene == "Prefab Workstation")
                GameManager.resetAll();
            Debug.Log("Scene Changed");

            
        }
        else
            Debug.Log("Next Scene either does not exist or is not in the list of levels to build!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !isInteract)
        {
            ChangeScene();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        inContact = (other.tag == "Player" && isInteract);

        //For setting the objective text
        GameManager.canInteract = inContact;
        GameManager.objectiveTextString = "Press F To Go To " + nextRoom;
    }
    private void OnTriggerExit(Collider other)
    {
        inContact = false;

        //For setting the objective text
        GameManager.canInteract = false;
    }
}
