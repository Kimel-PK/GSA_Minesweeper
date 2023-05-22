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
    public bool gameOver = true;
    public double timer;
    public int correctlyPlacedFlags = 0;
    public int checkedTilesCount = 0;
    public int numberOfTilesToCheck;
    public bool paused;
    
    private void Awake()
    {
        if (Singleton == null)
            Singleton = this;
        else
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
    }
    
    private void Update()
    {
        if (!gameOver)
            timer += Time.deltaTime;
    }

    public void GameOver(bool isGameWon)
    {
        if (isGameWon)
        {
            GameUI.Singleton.ShowGameWonScreen();
        }
        else
        {
            GameUI.Singleton.ShowGameOverScreen();
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameOver = true;
    }

    public void StartGame()
    {
        correctlyPlacedFlags = 0;
        checkedTilesCount = 0;
        numberOfTilesToCheck = (sizeX * sizeZ) - minesCount;
        StartCoroutine(EStartGame());
    }

    IEnumerator EStartGame()
    {
        var task = SceneManager.LoadSceneAsync(1);
        yield return new WaitUntil(() => task.isDone);
        Cursor.lockState = CursorLockMode.Locked;
        timer = 0;
        gameOver = false;
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

    public void CheckIfGameIsWon()
    {
        if (correctlyPlacedFlags == minesCount && GameUI.Singleton.PlacedFlags == minesCount && checkedTilesCount == numberOfTilesToCheck)
        {
            GameOver(true);
        }
    }

    public void Pause()
    {
        paused = true;
        Cursor.lockState = CursorLockMode.None;
    }
    
    public void Resume()
    {
        paused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}