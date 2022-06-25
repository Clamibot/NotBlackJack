using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCameraMove : MonoBehaviour
{
    [SerializeField] private GameObject playerMarker;

    // Update is called once per frame
    void Update()
    {
        if (MapAccess.inMap)
        {
            //Get x movement
            float moveX = Input.GetAxis("Horizontal");

            //Get 'z' movement
            float moveZ = Input.GetAxis("Vertical");

            //Move the map
            this.transform.position = new Vector3(this.transform.position.x + moveX,
                this.transform.position.y,
                this.transform.position.z + moveZ);

            //Insert check to see if player has gone out of bounds; if so then reset position to the player position
            //if(transform.position > player.transform.position)
        }
    }

    public void initPositions()
    {
        initPosition();
        initMarker();
    }

    public void initPosition()
    {
        //Start the camera where the player is
        this.transform.position = new Vector3(GameManager.playerPosition.x,
            this.transform.position.y,
            GameManager.playerPosition.z);
    }
    public void initMarker()
    {
        //Put the player marker where the player is
        playerMarker.transform.position = new Vector3(GameManager.playerPosition.x,
            this.transform.position.y - 1f,
            GameManager.playerPosition.z);
        //Set the player marker as independant so it doesn't move
        playerMarker.transform.parent = null;
    }
}
