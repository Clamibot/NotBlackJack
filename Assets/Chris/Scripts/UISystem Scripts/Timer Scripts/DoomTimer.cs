using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.SceneManagement;

public class DoomTimer : MonoBehaviour
{
    private bool paused;           //Tells if the timer is paused or not
    public float startTime = 180f; //Starting time left (default 3min)
    [HideInInspector] public float timeLeft; //Will hold the decreasing time left

    [SerializeField] private Text display;  //References the time that has to tick down
    [SerializeField] private GameObject DeathScreen;    //References the GameOver UI

    // Start is called before the first frame update
    void Start()
    {
        //The timer will start paused, it will need to be changed with a mutator method
        resetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        //If the game is not paused
        if(!paused && !PauseMenu.isPaused)
        {
            //Decrease the time left
            timeLeft -= Time.deltaTime;
            display.text = timeLeft.ToString("F") + "s";
            //If the timer hits 0, then game over
            if (timeLeft < 0)
                timerFinished();
        }
    }

    //Starts the timer
    public void startTimer()
    {
        if (paused)
        {
            gameObject.SetActive(true);
            paused = false;
            //Set global timer var is running
            GameManager.timerRunning = true;
            Debug.Log("Timer has started");
        }
    }
    public void endTimer()
    {
        if(!paused)
        {
            paused = true;
            gameObject.SetActive(false);
            //Set global timer var is not running
            GameManager.timerRunning = false;
            Debug.Log("Timer has ended");
        }
    }
    public void pauseTimer()
    {
        paused = true;
    }
    public void resetTimer()
    {
        timeLeft = startTime;
        display.text = timeLeft.ToString("F") + "s";
        paused = true;
        gameObject.SetActive(false);
    }

    //Used when the timer finishes
    public void timerFinished()
    {
        Debug.Log("Game Over");
        display.text = "DOOM";
        paused = true;
        //SceneManager.LoadScene("Death");
        //For now use
        GameManager.isDead = true;
        DeathScreen.SetActive(true);
        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

}
