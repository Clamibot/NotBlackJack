using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    public float rotationSpeed = 1f;
    public Transform player, target, centerHeadPosition, zoomedInPosition;
    public Transform spineBase;             //Used to keep the player's upper body steady while aiming and moving
    public Transform playerUpDownRotation;  //Gets rotated to aim where the user points
    public GameObject crosshair;

    public float aimOffset = 0.5f;          //Publicly gets how far off the center we should look at when aiming (right mouse)
    public float zoomDistance = 1.75f;
    [HideInInspector] public float zoomSpeed = 5f;

    private float mouseX, mouseY;

    //Used for ignoring objects between Ethan and the camera
    private Transform obstruction;

    // Start is called before the first frame update
    void Start()
    {
        obstruction = target;

        //Turn the crosshair off at the start
        crosshair.SetActive(false);

        zoomedInPosition.Translate(new Vector3(aimOffset, 0, zoomDistance));    //Sets our zoom position based off our inputs

        //Locks the mouse and makes it invisible
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CamControl();
        //ViewObstructed();
    }

    void CamControl()
    {
        //Get how far we are moving the camera
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        //Clamp the y direction from moving too far up or down
        mouseY = Mathf.Clamp(mouseY, -90, 60);

        //move the camera to look at the target
        transform.LookAt(target);


        //If right mouse click is held, point player towards camera direction
        if (Input.GetMouseButton(1))
        {
            //Lock the lower spine so the player can aim
            spineBase.localRotation = Quaternion.Euler(-2, 0, 0);
            //spineBase.rotation = Quaternion.Euler(-2, 0, 0);

            //Rotate the player
            player.rotation = Quaternion.Euler(0, mouseX, 0);
            playerUpDownRotation.localRotation = Quaternion.Euler(0, 0, mouseY+10);    //Adding 10 degrees to keep the gun pointed right on the crosshair

            //Rotate the camera rig (updates the aim location as well)
            centerHeadPosition.rotation = Quaternion.Euler(mouseY, mouseX, 0);

            //If we aren't in the zoomed in location
            if(target.localPosition.x < zoomedInPosition.localPosition.x)
            {
                //Turn the crosshair on
                crosshair.SetActive(true);
                //Move the camera's aim location to the zoomed in position
                target.position = Vector3.MoveTowards(target.position, zoomedInPosition.position, zoomSpeed * Time.deltaTime);
            }
        }
        //If middle mouse click, point camera direction player is facing (Doesn't Work)
        /*else if(Input.GetMouseButtonDown(2))
        {
            mouseX = player.rotation.y;
            mouseY = player.rotation.x;
            //target.rotation = Quaternion.Euler(player.rotation.x, player.rotation.y, 0);
        }*/
        //When neither is done, player can move idenpendantly from camera's rotation
        else
        {
            //rotate the camera rig (updates the aim location as well)
            centerHeadPosition.rotation = Quaternion.Euler(mouseY, mouseX, 0);

            //If we aren't at the center of head
            if (target.localPosition.x > centerHeadPosition.localPosition.x)
            {
                //Turn the crosshair off
                crosshair.SetActive(false);
                //Move the camera's aim location to the center of head
                target.position = Vector3.MoveTowards(target.position, centerHeadPosition.position, zoomSpeed * Time.deltaTime);
            }
        }

        
    }

    //Doesn't work well (From a tutorial)
    /*void ViewObstructed()
    {
        RaycastHit hit;

        //Check if there are any objects between the camera and our viewing center
        if(Physics.Raycast(transform.position, target.position - transform.position, out hit, 4.5f))
        {
            //If it is an object that is not the player
            if(hit.collider.gameObject.tag != "Player")
            {
                //Get the obstruction and turn its render off except for its shadows
                obstruction = hit.transform;
                obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

                //zoom in
                if(Vector3.Distance(obstruction.position, transform.position) >= 3f && Vector3.Distance(transform.position, target.position) >= 1.5f)
                {
                    transform.Translate(Vector3.forward * zoomSpeed * Time.deltaTime);
                }
            }
            //Else there is no obstruction and we hit the player
            else
            {
                //Turn the previous obstruction's render back on
                obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

                //zoom back out
                if (Vector3.Distance(transform.position, target.position) < 4.5f)
                {
                    transform.Translate(Vector3.back * zoomSpeed * Time.deltaTime);
                }
            }
        }
    }*/
}
