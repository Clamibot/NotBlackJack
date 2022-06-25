using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverButton : MonoBehaviour
{

    public void Restart()
    {
        Time.timeScale = 1f;
        PauseMenu.isPaused = false;
        //Reset static variable counts (Train track nums and ground nums
        GameManager.resetStaticBackgroundItems();
        //State that the game restarted the level
        GameManager.didRestart = true;
        //State that player is not dead anymore
        GameManager.isDead = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMenu()
    {
        //Reset variables
        GameManager.resetAll();
        Time.timeScale = 1f;
        PauseMenu.isPaused = false;
        SceneManager.LoadScene("Prefab Workstation");
    }

    public void QuitToDesktop()
    {
        Debug.Log("Game is turning off");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
