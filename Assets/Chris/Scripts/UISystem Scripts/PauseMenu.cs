using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //Variables
	public static bool isPaused = false;
    //references public static bool MapAccess.inMap
    [HideInInspector] public static bool inSettings = false;

    //Object references
    public GameObject pauseScreen;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		//Check if the buttons to enable the pause menu is active
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
		{
			if(isPaused && !inSettings)
			{
				//Set the game back in motion
				Resume();
			}
			else if(!isPaused && !inSettings)
			{
				//Pause the events in game and pull up the pause screen
				Pause();
			}
            //Control settings access
            else if (inSettings)
            {
                inSettings = false;
            }
        }
    }

    //Resume the game, take the menu away, resume time (from pause menu access)
    public void Resume()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    //Pause the game, bring up the menu, and freeze time
    public void Pause()
	{
		pauseScreen.SetActive(true);
		Time.timeScale = 0f;
		isPaused = true;
    }
	
	public void Settings()
	{
		Debug.Log("This operation is not supported yet\n");
	}
	
	public void QuitToMenu()
	{
		Time.timeScale = 1f;
        isPaused = false;
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
