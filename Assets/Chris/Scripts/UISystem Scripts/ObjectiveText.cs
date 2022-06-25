using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveText : MonoBehaviour
{
    [SerializeField] private Text objectiveTextObject;
    private enum LevelArea {Level1, Level2, Level3, Level4, Level5, Level6};
    [SerializeField] private LevelArea levelArea = LevelArea.Level1;

    // Start is called before the first frame update
    void Start()
    {
        //Quick set the text
        objectiveTextObject.text = getDefaultLevelText();
    }

    // Update is called once per frame
    void Update()
    {
        //Set the text field
        objectiveTextObject.text = (GameManager.canInteract) ? GameManager.objectiveTextString : getDefaultLevelText();
        //Disapeer the box
        objectiveTextObject.transform.parent.gameObject.SetActive(objectiveTextObject.text != "");
    }

    private string getDefaultLevelText()
    {
        string temp = "";
        //Sett he default text based on which level we are in
        switch(levelArea)
        {
            case LevelArea.Level1:
                temp = "Make your way to the Finals!";
                break;
            case LevelArea.Level2:
                temp = "Get through the Hitmen!";
                break;
            case LevelArea.Level3:
                temp = "Get through the Hitmen!";
                break;
            case LevelArea.Level4:
                temp = "Make your way to the Finals!";
                break;
            case LevelArea.Level5:
                temp = "Get To The Car!";
                break;
            case LevelArea.Level6:
                temp = "Defeat The Big Cheese!";
                break;
        }
        return temp;
    }
}
