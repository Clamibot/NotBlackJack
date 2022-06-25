using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum ButtonType
{
	PLAY,
    RESTARTLEVEL,
    MAINMENU,
    STARTOVER,
	QUIT,
    LOADCONTROLS,
    SHOWPAUSEMENU,
    HIDEPAUSEMENU,
    RESTARTFROMPAUSE
}

public class MainMenu : MonoBehaviour
{
	public ButtonType bt = ButtonType.PLAY;

	private Button theButton;

    public GameObject loadingText;
    public Slider loadingBar;
    public Text progressText;
    public GameObject textToBeDestroyed;
    public bool replaceButtonText = false;
    public GameObject pauseButton;
    public GameObject restartButton;
    public GameObject menubutton;
    public GameObject backToGameButton;

    private void Start()
	{
		theButton = GetComponent<Button>();

		if (theButton == null)
		{
			return;
		}

		switch (bt)
		{
			case ButtonType.PLAY:
				theButton.onClick.AddListener(Play);
				break;

            case ButtonType.RESTARTLEVEL:
                theButton.onClick.AddListener(RestartLevel);
                break;

            case ButtonType.MAINMENU:
                theButton.onClick.AddListener(BackToMainMenu);
                break;

            case ButtonType.STARTOVER:
                theButton.onClick.AddListener(StartOver);
                break;

            case ButtonType.QUIT:
				theButton.onClick.AddListener(Quit);
				break;

            case ButtonType.LOADCONTROLS:
                theButton.onClick.AddListener(LoadControls);
                break;

            case ButtonType.SHOWPAUSEMENU:
                theButton.onClick.AddListener(ShowPauseMenu);
                break;

            case ButtonType.HIDEPAUSEMENU:
                theButton.onClick.AddListener(HidePauseMenu);
                break;

            case ButtonType.RESTARTFROMPAUSE:
                theButton.onClick.AddListener(RestartFromPause);
                break;
        }
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.isDead == false)
        {
            pauseButton.SetActive(false);
            restartButton.SetActive(true);
            menubutton.SetActive(true);
            backToGameButton.SetActive(true);
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetKeyDown(KeyCode.LeftAlt)) // Just unlocks the cursor
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Play()
	{
        StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().buildIndex + 1, replaceButtonText)); // Load the next level.
	}

    public void RestartLevel()
    {
        StartCoroutine(LoadAsynchronously(PlayerPrefs.GetInt("LastLoadedScene"), replaceButtonText)); // Load the last loaded level again. This will be a saved index in PlayerPrefs.
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0); // Load the main menu again. This will be at index 0.
    }

    public void StartOver()
    {
        SceneManager.LoadScene(1); // Load the first scene again. This will be at index 1.
    }

    public void LoadControls()
    {
        SceneManager.LoadScene(9); // Load the controls scene. This will be at index 9.
    }

    public void Quit()
	{
		Debug.Log("User Exited Game.");
		Application.Quit();
	}

    public void ShowPauseMenu()
    {
        pauseButton.SetActive(false);
        restartButton.SetActive(true);
        menubutton.SetActive(true);
        backToGameButton.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void HidePauseMenu()
    {
        pauseButton.SetActive(true);
        restartButton.SetActive(false);
        menubutton.SetActive(false);
        backToGameButton.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void RestartFromPause()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().buildIndex, false)); // Reload the current scene
    }

    IEnumerator LoadAsynchronously(int levelIndex, bool replaceButtonText)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(levelIndex);
        if (replaceButtonText)
            textToBeDestroyed.SetActive(false);
        loadingText.SetActive(true);
        loadingBar.gameObject.SetActive(true);

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / .9f);
            loadingBar.value = progress;
            progressText.text = progress * 100 + "%";
            yield return null;
        }
    }
}
