using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool isDead = false;                      //Used to prevent the player from opening the pause menu when they are dead
    public static bool timerRunning = false;
    public static bool didRestart = false;
    public static Vector3 playerPosition = Vector3.zero;
    public static Vector3 restartPosition = Vector3.zero;

    public static string objectiveTextString = "";
    public static bool canInteract = false;

    public static void resetAll()
    {
        resetStaticBackgroundItems();
        resetFlags();
    }
    public static void resetStaticBackgroundItems()
    {
        //The timer
        timerRunning = false;
        //Reset the knowledge if the player is able to interact
        canInteract = false;
        objectiveTextString = "";

    }
    public static void resetFlags()
    {
        isDead = false;
        didRestart = false;
    }
}
