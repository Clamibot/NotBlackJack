using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatCodes : MonoBehaviour
{
    //Records if the user is trying for a cheat code
    private string userInput;
    //Cheat Codes
    private const int cheatCodeLength = 6;
    private const string fullHealth = "HEALTH";
    private const string levelSkip = "ISANIC";

    private string cheatCodeName = "";

    [SerializeField] private Properties properties;

    // Start is called before the first frame update
    void Start()
    {
        userInput = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            userInput = "";
            Debug.Log("Code reset");
        }

        userInput += getImportantKeyCharactersOnPress();

        if(userInput.Length >= cheatCodeLength)
        {
            if(ActivateCheatCode())
            {
                //Update objective screen to let player know they have activated a cheat code
                GameManager.canInteract = true;
                GameManager.objectiveTextString = cheatCodeName + " Activated";
                Debug.Log("Cheat Code Activated");
            }
            else
            {
                userInput = "";
            }
        }
    }

    private bool ActivateCheatCode()
    {
        //Default cheat code name
        cheatCodeName = "Cheat Code";

        if (userInput == fullHealth)
        {
            properties.currentHealth = 1000;

            return true;
        }
        else if (userInput == levelSkip)
        {
            //Load the next scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Debug.Log("Scene Changed");

            return true;
        }

        return false;
    }

    private string getImportantKeyCharactersOnPress()
    {
        string temp = "";

        //Return key that is pressed if any
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                temp = "A";
            else if (Input.GetKeyDown(KeyCode.B))
                temp = "B";
            else if (Input.GetKeyDown(KeyCode.C))
                temp = "C";
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                temp = "D";
            else if (Input.GetKeyDown(KeyCode.E))
                temp = "E";
            else if (Input.GetKeyDown(KeyCode.F))
                temp = "F";
            else if (Input.GetKeyDown(KeyCode.H))
                temp = "H";
            else if (Input.GetKeyDown(KeyCode.I))
                temp = "I";
            else if (Input.GetKeyDown(KeyCode.K))
                temp = "K";
            else if (Input.GetKeyDown(KeyCode.L))
                temp = "L";
            else if (Input.GetKeyDown(KeyCode.N))
                temp = "N";
            else if (Input.GetKeyDown(KeyCode.O))
                temp = "O";
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                temp = "S";
            else if (Input.GetKeyDown(KeyCode.T))
                temp = "T";
            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                temp = "W";
        }
        

        return temp;
    }
}
