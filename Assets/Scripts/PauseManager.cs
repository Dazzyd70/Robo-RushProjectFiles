using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject gameUI;
    public GameObject WinScreen;
    public GameObject LoseScreenStandard;
    public GameObject LoseScreenEndless;
    public static bool isPaused;
    public string menuScene;
    public static bool isWon;
    public static bool isLostStandard;
    public static bool isLostEndless;

    void Start()
    {
        pauseMenu.SetActive(false);
        WinScreen.SetActive(false);
        LoseScreenStandard.SetActive(false);
        LoseScreenEndless.SetActive(false);
        gameUI.SetActive(true);
        isWon = false;
        isLostStandard = false;
        isLostEndless = false;
        isPaused = false;
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        if (isWon)
        {
            winScreen();
        }

        if (isLostStandard)
        {
            loseScreenStandard();
        }
        if (isLostEndless)
        {
            loseScreenEndless();
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        gameUI.SetActive(false);
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        gameUI.SetActive(true);
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuScene);
    }

    public void winScreen()
    {
        WinScreen.SetActive(true);
        gameUI.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void loseScreenStandard()
    {
        LoseScreenStandard.SetActive(true);
        gameUI.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void loseScreenEndless()
    {
        LoseScreenEndless.SetActive(true);
        gameUI.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isPaused = true;
        Time.timeScale = 0f;
    }
}
