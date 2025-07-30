using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool isPaused;
    private bool wasPreviouslyFrozen;
    private InputAction escape;
    private bool menuLock;
    void Start()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
        escape = InputSystem.actions.FindAction("Player/Escape");
    }

    void Update()
    {
        
        if (escape.IsPressed() && !menuLock)
        {
            StartCoroutine(waitForSeconds(1f));
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
    IEnumerator waitForSeconds(float seconds)
    {
        menuLock = true;
        yield return new WaitForSeconds(seconds);
        menuLock = false;
    }
}
