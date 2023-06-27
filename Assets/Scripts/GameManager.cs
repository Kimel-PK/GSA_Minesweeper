using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public class HighScores
    {
        // High score for map with size AxB is also considered as high score for map with size BxA.
        public List<float> mapSizeHigher = new();
        public List<float> mapSizeLower = new ();
        public List<float> mapMinesCount = new();
        public List<double> highScore = new();
    }
    public HighScores highScores = new();


    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            LoadHighScores();
        }
        else
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
    }
    
    private void Update()
    {
        if (!gameOver && !paused)
            timer += Time.deltaTime;
    }


    /// <param name="explosionPosition"> Null if game is won, mine explosion position otherwise</param>
    public void GameOver(Vector3 ?explosionPosition)
    {
        if (explosionPosition == null)
        {
            onGameWon?.Invoke();
            GameUI.Singleton.ShowGameWonScreen();

            SaveHighScores();
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

    public void SaveHighScores()
    {
        double currentHighScore;
        int listIndex;
        (currentHighScore, listIndex) = GetHighScore(sizeX, sizeZ, minesCount);

        if (timer < currentHighScore)
        {
            if (listIndex == -1) // If there is no high score entry for this map configuration.
            {
                if (sizeX > sizeZ)
                {
                    highScores.mapSizeHigher.Add(sizeX);
                    highScores.mapSizeLower.Add(sizeZ);
                }
                else
                {
                    highScores.mapSizeHigher.Add(sizeZ);
                    highScores.mapSizeLower.Add(sizeX);
                }
                highScores.mapMinesCount.Add(minesCount);
                highScores.highScore.Add(timer);
            }
            else
            {
                highScores.highScore[listIndex] = timer;
            }

            string scoresJson = JsonUtility.ToJson(highScores);
            File.WriteAllText(Application.persistentDataPath + "/highScores.json", scoresJson);
            print($"saved to {Application.persistentDataPath + "/highScores.json"}");
        }
    }

    public void LoadHighScores()
    {
        string path = Application.persistentDataPath + "/highScores.json";
        if (File.Exists(path))
        {
            string scoresJson = File.ReadAllText(path);
            highScores = JsonUtility.FromJson<HighScores>(scoresJson);
        }
    }

    /// <returns>High score for given map configuration and its index in high scores list</returns>
    public (double, int) GetHighScore(float sizeX, float sizeZ, float minesCount)
    {
        float sizeHigher, sizeLower;
        if (sizeX > sizeZ)
        {
            sizeHigher = sizeX;
            sizeLower = sizeZ;
        }
        else
        {
            sizeHigher = sizeZ;
            sizeLower = sizeX;
        }

        for (int i = 0; i < highScores.mapSizeHigher.Count; i++)
        {
            if (highScores.mapSizeHigher[i] == sizeHigher
                && highScores.mapSizeLower[i] == sizeLower
                && highScores.mapMinesCount[i] == minesCount)
            {
                return (highScores.highScore[i], i);
            }
        }

        return (Mathf.Infinity, -1);
    }
}