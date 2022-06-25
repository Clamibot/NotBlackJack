using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    //Required GameObjects
	[SerializeField] private GameObject player;
    private static Camera thisCamera;
    //Varialbles
	private float heightOffset;
	private float backOffset;
    //Global condiditons
    [HideInInspector] public static bool isFacingFromBack = false;
    [HideInInspector] public static bool isFacingFromTop = false;
    [HideInInspector] public static bool isRotating = false;
    [HideInInspector] public static bool isCentered = false;
    [SerializeField] private bool inBuilding = false;
    private static bool inBuildingStatic;
    // Start is called before the first frame update
    void Start()
    {
        //Get desirable camera offsets
        heightOffset = transform.position.y - player.transform.position.y;
		backOffset = transform.position.z - player.transform.position.z;

        //Set thisCamera to Main (which controls the player)
        thisCamera = Camera.main;
        isCentered = false;
        isRotating = false;
        isFacingFromBack = false;
        isFacingFromTop = false;

        inBuildingStatic = inBuilding;
    }


    // Update is called once per frame
    void Update()
    {
        //This now serves to adjust the viewing angles
        if (!isCentered)
        {
            //Normal Movement
            if (!isFacingFromBack && !isFacingFromTop && !isRotating)
                transform.position = new Vector3(
                    player.transform.position.x,
                    player.transform.position.y + heightOffset,
                    transform.position.z);
            //Movement when facing rear
            else if (isFacingFromBack && !isRotating)
                transform.position = new Vector3(
                    player.transform.position.x + backOffset / 1.5f,
                    player.transform.position.y + heightOffset,
                    player.transform.position.z);
            //Movement when facing top down on train
            else if (isFacingFromTop && !isRotating)
                transform.position = new Vector3(
                    player.transform.position.x + backOffset / 2.5f,
                    player.transform.position.y - backOffset / 1.5f,
                    player.transform.position.z);

        }
    }

    public static void CenterCamera(Vector3 position)
    {
        //Notify that the camera is centered
        isCentered = true;
        //Separate camera from player
        thisCamera.transform.parent = null;
        //Fix the position
        if(isFacingFromTop || inBuildingStatic)
            thisCamera.transform.position = new Vector3(position.x,
                                                    thisCamera.transform.position.y,
                                                    position.z);
        else
            thisCamera.transform.position = new Vector3(position.x,
                                                    thisCamera.transform.position.y,
                                                    thisCamera.transform.position.z);
    }

    public static void ReleaseCamera(GameObject player)
    {
        //Notify that the camera is released
        isCentered = false;
        //Put camera as child of player
        thisCamera.transform.parent = player.transform;

        //If in building
        if(inBuildingStatic)
            thisCamera.transform.position = new Vector3(player.transform.position.x,
                                                    thisCamera.transform.position.y,
                                                    player.transform.position.z);

    }
}
