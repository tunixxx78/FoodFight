using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TryAgain();
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 0;
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
    }

    public void TryAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartTime()
    {
        Time.timeScale = 1;
        //Cursor.lockState = CursorLockMode.Locked;
    }

}

