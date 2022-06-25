using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCameraPatrol : MonoBehaviour
{
    private enum PatrolDirection { Stationary, Left, Right };
    [SerializeField] private PatrolDirection patrolDirection = PatrolDirection.Stationary;
    [SerializeField] private float patrolDistance;
    private float speed = 4;

    private Vector3 direction;
    private float distanceTraveled = 0;

    // Start is called before the first frame update
    void Start()
    {
        //If the developer tries to put a distance in when the camera is set to stationary, set the distance to 0 (even though it wouldn't change anything)
        if (patrolDirection == PatrolDirection.Stationary)
            patrolDistance = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(patrolDirection != PatrolDirection.Stationary && !PauseMenu.isPaused)
        {
            Vector3 direction = (patrolDirection == PatrolDirection.Left) ? -transform.forward : transform.forward;
            //direction = direction * transform.rotation.y;

            transform.Translate(direction * Time.deltaTime * speed,Space.World);
            distanceTraveled += Time.deltaTime * speed;

            if(distanceTraveled > patrolDistance)
            {
                distanceTraveled = 0;
                patrolDirection = (patrolDirection == PatrolDirection.Left) ? PatrolDirection.Right : PatrolDirection.Left;
            }
        }
    }
}
