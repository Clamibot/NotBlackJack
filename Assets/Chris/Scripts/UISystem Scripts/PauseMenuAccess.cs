using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenuAccess : MonoBehaviour
{
    //Object References
    /*public Button pauseResumeButton;
    public Sprite play;
    public Sprite pause;*/
    private PauseMenu pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu = this.transform.parent.GetComponent<PauseMenu>();
    }


    public void PauseButton()
    {
        pauseMenu.Pause();
    }

    /*public void PauseResumeButton()
    {
        if(PauseMenu.isPaused)
        {
            pauseMenu.Resume();
        }
        else// if(!PauseMenu.isPaused)
        {
            pauseMenu.Pause();
        }
    }

    public void loadResume()
    {
        pauseResumeButton.image.sprite = play;
    }
    public void loadPause()
    {
        pauseResumeButton.image.sprite = pause;
    }*/
}
