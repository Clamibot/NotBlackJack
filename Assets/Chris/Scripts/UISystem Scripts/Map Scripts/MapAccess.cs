using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapAccess : MonoBehaviour
{
    //Variables
    [HideInInspector] public static bool inMap = false;

    //Object References
    public Camera mapCam;
    private MapCameraMove mapCamScript;
    public Button mapButton;
    public Sprite open;
    public Sprite closed;

    private void Start()
    {
        mapCamScript = mapCam.GetComponent<MapCameraMove>();
    }

    // Update is called once per frame
    void Update()
    {
        //Only allow access to the map if the game is not pausef
        if (!PauseMenu.isPaused)
        {
            //Check if the Objectives button was pressed
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                //Control map access
                if (inMap)
                {
                    LeaveMap();
                }
            }
            //Check if the map button was pressed
            if (Input.GetKeyDown(KeyCode.M))
            {
                //Control map access
                if (inMap)
                {
                    LeaveMap();
                }
                else if (!inMap)
                {
                    GoToMap();
                }
            }
        }
    }

    public void MapButon()
    {
        //Control map access
        if (inMap)
        {
            LeaveMap();
        }
        else if (!inMap)
        {
            GoToMap();
        }
    }

    public void GoToMap()
    {
        //Set status
        inMap = true;
        //Turn on map
        mapCam.gameObject.SetActive(true);
        //Change button sprite
        mapButton.image.sprite = open;
        //Clear highlight
        EventSystem.current.SetSelectedGameObject(null);
        //reset the marker and camera to the player's position
        mapCamScript.initPositions();
    }

    public void LeaveMap()
    {
        //Set status
        inMap = false;
        //Turn map off
        mapCam.gameObject.SetActive(false);
        //Change button sprite
        mapButton.image.sprite = closed;
        //Clear highlight
        EventSystem.current.SetSelectedGameObject(null);
    }
}
