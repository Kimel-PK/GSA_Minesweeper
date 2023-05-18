using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton { get; private set; }
    
    public int sizeX = 9;
    public int sizeZ = 9;
    public int minesCount = 10;
    public bool gameOver;

    private void Awake()
    {
        if (Singleton == null)
            Singleton = this;
        else
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
    }

    public void GameOver()
    {
        GameUI.Singleton.ShowGameOverScreen();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameOver = true;
    }

    public void StartGame()
    {
        StartCoroutine(EStartGame());
    }

    IEnumerator EStartGame()
    {
        var task = SceneManager.LoadSceneAsync(1);
        yield return new WaitUntil(() => task.isDone);
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    public void BackToMenu()
    {
        gameOver = false;
        StartCoroutine(EBackToMenu());
    }

    IEnumerator EBackToMenu()
    {
        var task = SceneManager.LoadSceneAsync(0);
        yield return new WaitUntil(() => task.isDone);
        Cursor.lockState = CursorLockMode.None;
    }
}