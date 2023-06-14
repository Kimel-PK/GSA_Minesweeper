using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton { get; private set; }
    
    public int sizeX = 9;
    public int sizeZ = 9;
    public int minesCount = 10;
    public bool gameOver = true;
    public double timer;
    public int checkedTilesCount = 0;
    public int numberOfTilesToCheck;
    public Vector3 mineExplosionPosition;
    public event Action onMineExplosion;
    public event Action onGameWon;
    public bool paused;
    public AudioMixer mixer;
    public float mouseSensitivity = 0.1f;
    public float MasterVolume
    {
        get
        {
            float volume;
            mixer.GetFloat("masterVolume", out volume);
            return volume;
        }
        set
        {
            mixer.SetFloat("masterVolume", value);
        }
    }


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


    /// <param name="explosionPosition"> Null if game is won, mine explosion position otherwise</param>
    public void GameOver(Vector3 ?explosionPosition)
    {
        if (explosionPosition == null)
        {
            onGameWon?.Invoke();
            GameUI.Singleton.ShowGameWonScreen();
        }
        else
        {
            mineExplosionPosition = (Vector3)explosionPosition;
            //mineExplosionPosition = new Vector3(mineExplosionPosition.x, -1, mineExplosionPosition.z);
            
            onMineExplosion?.Invoke();
            GameUI.Singleton.ShowGameOverScreen();
        }

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
        timer = 0;
        gameOver = false;
        checkedTilesCount = 0;
        numberOfTilesToCheck = (sizeX * sizeZ) - minesCount;
    }

    public void BackToMenu()
    {
        gameOver = false;
        paused = false;
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
        if (checkedTilesCount == numberOfTilesToCheck)
        {
            GameOver(null);
        }
    }
}