using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    [SerializeField] private GameObject DeathScreen;

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            AppearDeathScreen();
        }
    }

    public void AppearDeathScreen()
    {
        Debug.Log("Game Over");

        //SceneManager.LoadScene("Death");
        //For now use
        GameManager.isDead = true;
        DeathScreen.SetActive(true);
        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

}
