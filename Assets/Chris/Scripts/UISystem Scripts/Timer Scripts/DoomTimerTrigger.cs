using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoomTimerTrigger : MonoBehaviour
{
    private enum State { Start, End, Pause, Reset };
    [SerializeField] private State state;
    private GameObject Doom_Timer; //Set equal to the DoomTimer UI
    private DoomTimer timer;
    //Used if we need to hide in an object to stop the timer
    [SerializeField] private bool isInteract = false;
    private bool inContact = false;

    // Start is called before the first frame update
    void Start()
    {
        Doom_Timer = GameObject.Find("DoomTimer");
        timer = Doom_Timer.GetComponent<DoomTimer>();   //Get the script
    }

    // Update is called once per frame
    void Update()
    {
        if (inContact && Input.GetKeyDown(KeyCode.F))
        {
            AffectTimer();
        }
    }
    private void AffectTimer()
    {
        switch (state)
        {
            case (State.Start):
                timer.startTimer(); //To start it 
                break;
            case (State.End):
                timer.endTimer();   //To  end  it 
                break;
            case (State.Pause):
                timer.pauseTimer(); //To pause it
                break;
            case (State.Reset):
                timer.resetTimer(); //To reset it
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !isInteract)
        {
            AffectTimer();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        inContact = (other.tag == "Player" && isInteract);
    }
    private void OnTriggerExit(Collider other)
    {
        inContact = false;
    }
}
