using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool isPaused;
    private bool wasPreviouslyFrozen;
    
    void Start()
    {
        pauseMenu.SetActive(false);
        isPaused = false;

    }

    void Update()
    {
        print(Input.GetKeyDown(KeyCode.Escape));
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                resumeGame();
            }
            else
            {
                pauseGame();
            }
        }
    }

    public void pauseGame()
    {
        pauseMenu.SetActive(true);
        if (Time.timeScale == 0f)
            wasPreviouslyFrozen = true;
        else
            Time.timeScale = 0f;
        isPaused = true;
    }

    public void resumeGame()
    {
        pauseMenu.SetActive(false);
        if(!wasPreviouslyFrozen)
            Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");

    }
}
