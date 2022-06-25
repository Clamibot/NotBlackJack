using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //Object References

    //Holds info about the room as well as rooms nearby
    [SerializeField] private List<GameObject> adjacentRooms;
    [SerializeField] private List<GameObject> tasksToComplete;
    //Holds the components that we need to access (getting the components ahead of time)
    private List<Room> adjacentRoomScripts;

    //References the part that is shown on the map screen
    private GameObject mapPiece;
    //References the script of the part that is shown on the map screen
    private MapPiece mapPieceScript;

    //Variables

    //Used for the name of the room
    public String roomName;
    //Used to check if this room has been discovered yet
    public bool isVisible = false;
    //Used to hard define the room as being complete. Note, isVisible should be set to true if this is set to true
    public bool isCompleteRoom = false;
    //Used to check if we have visited this room before
    private bool visited = false;
    //Used to check if this is a room we need to center the camera on
    public bool shouldCenterCamera = false;

    // Start is called before the first frame update
    void Start()
    {
        //Get the map pieces related to this room
        mapPiece = gameObject.transform.Find("Map Piece").gameObject;
        mapPieceScript = mapPiece.GetComponent<MapPiece>();


        //Update the room as already being complete
        if (isCompleteRoom)
        {
            mapPieceScript.markAsComplete();
        }
        //If the room needs to be visible
        if (!isVisible)
            mapPiece.SetActive(false);

        


        //Transfer adjacentRoom's Room.cs script to the array
        if (adjacentRooms != null && adjacentRooms.Count > 0)
        {
            adjacentRoomScripts = new List<Room>();
            foreach (GameObject room in adjacentRooms)
                adjacentRoomScripts.Add(room.GetComponent<Room>());
        }

        if (tasksToComplete != null && tasksToComplete.Count > 0)
            mapPieceScript.isHallway = false;
        //If the objects are complete upon level reload
        checkTasksCompleted();
    }


    private void OnTriggerEnter(Collider other)
    {
        //Show adjacent rooms and/or update the room color
        if (!visited && other.tag == "Player")
        {
            revealMap();
            if(gameObject.tag == "Building")
                lookFurther();
        }
    }

    private void checkTasksCompleted()
    {
        //for (int i = tasksToComplete.Count; i > 0; i--)
        //    if (tasksToComplete[i-1].tag == "Searchable" && tasksToComplete[i-1].GetComponent<SearchObject>().isSearched)    //Rename the component to the proper script name and get the boolean variable
        //        tasksToComplete.RemoveAt(i-1);
        //do a smilar one for puzzles
    }

    private void revealMap()
    {
        //Update that the room is now visited
        visited = true;
        //Set this room as being visible
        isVisible = true;
        mapPiece.SetActive(true);

        //Check if this rooms is hallway, where there are no tasks to complete
        if (tasksToComplete == null || tasksToComplete.Count == 0)
            //If this room wasn't forced to be complete, then mark it as a hallway
            if (!isCompleteRoom)
                mapPieceScript.markAsHallway();

        //Check if this room has any adjacent rooms
        if (adjacentRooms != null && adjacentRooms.Count > 0)
            //If there are, then for each room script
            foreach (Room room in adjacentRoomScripts)
                //If the room isn't already visible
                if (!room.isVisible)
                {
                    //Make the room visible
                    room.isVisible = true;
                    room.mapPiece.SetActive(true);
                }

    }

    private void lookFurther()
    {
        //Check if this room has any adjacent rooms via raycast while in the building
        RaycastHit adjacencyCheck;
        Vector3[] dirs = { transform.right, -transform.right, transform.forward, -transform.forward };

        Room focusRoom;
        float distanceToCheck;
        float nextDistance;
        for (int i = 0; i < dirs.Length; i++)
        {
            distanceToCheck = (i < 2) ? 30f : 20f;
            if (Physics.Raycast(new Vector3(transform.position.x, 1, transform.position.z), dirs[i], out adjacencyCheck, distanceToCheck))
            {
                if (adjacencyCheck.collider.tag == "Building")
                {
                    //Debug.Log(adjacencyCheck.collider.name);

                    focusRoom = adjacencyCheck.collider.gameObject.GetComponent<Room>();
                    nextDistance = distanceToCheck - Vector3.Distance(gameObject.transform.position, adjacencyCheck.collider.gameObject.transform.position);
                    //If the room isn't already visible
                    if (!focusRoom.isVisible)
                    {
                        //Make the room visible
                        focusRoom.isVisible = true;
                        focusRoom.mapPiece.SetActive(true);
                    }
                    //Look further in this direction to the next room
                    focusRoom.lookBeyond(nextDistance, dirs[i]);
                }
            }
        }

    }
    private void lookBeyond(float distanceLeft, Vector3 direction)
    {
        //Check if this room has any adjacent rooms via raycast while in the building
        RaycastHit adjacencyCheck;
        Room focusRoom;

        if (Physics.Raycast(new Vector3(transform.position.x, 1, transform.position.z), direction, out adjacencyCheck, distanceLeft))
        {
            if (adjacencyCheck.collider.tag == "Building")
            {
                //Debug.Log(adjacencyCheck.collider.name);

                focusRoom = adjacencyCheck.collider.gameObject.GetComponent<Room>();
                //If the room isn't already visible
                if (!focusRoom.isVisible)
                {
                    //Make the room visible
                    focusRoom.isVisible = true;
                    focusRoom.mapPiece.SetActive(true);
                }
                //Look further in this direction to the next room
                focusRoom.lookBeyond(distanceLeft
                                        - Vector3.Distance(gameObject.transform.position,
                                                            adjacencyCheck.collider.gameObject.transform.position)
                                    , direction);
            }
        }
    }

    //Used to mark this task as complete in the room and then check if it is truly complete
    //Should be called by a puzzle being completed or an object search has completed
    public void updateRoom(GameObject task)
    {
        tasksToComplete.Remove(task);

        if (tasksToComplete.Count == 0)
        {
            mapPieceScript.markAsComplete();
            isCompleteRoom = true;
        }
    }

    //Controls map centralization
    private void OnTriggerStay(Collider other)
    {
        //Lock the camera
        if (shouldCenterCamera)
        {
            //Center camera
            CameraMove.CenterCamera(gameObject.transform.position);
        }
        //Check if the room is complete
        if (other.tag == "Player" && !mapPieceScript.isHallway)
        {
            checkTasksCompleted();
            if (tasksToComplete.Count == 0)
                mapPieceScript.markAsComplete();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Release the camera
        CameraMove.ReleaseCamera(other.gameObject);
    }

}
