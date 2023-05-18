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
    public int correctlyPlacedFlags = 0;
    public int checkedTilesCount = 0;
    public int numberOfTilesToCheck;


    private void Awake()
    {
        if (Singleton == null)
            Singleton = this;
        else
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        numberOfTilesToCheck = (sizeX * sizeZ) - minesCount;
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
}