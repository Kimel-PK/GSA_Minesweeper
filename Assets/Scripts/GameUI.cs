using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI Singleton { get; private set; }
    [SerializeField] private TextMeshProUGUI placedFlagsText;
    [SerializeField] private TextMeshProUGUI minesText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject gameOverPanel;
    private int _placedFlags;
    public int PlacedFlags
    {
        get => _placedFlags;
        set
        {
            _placedFlags = value;
            placedFlagsText.text = $"{_placedFlags}";
        }
    }

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        PlacedFlags = 0;
        minesText.text = GameManager.Singleton.minesCount.ToString();
    }
    
    private void Update()
    {
        SetTimer();
    }

    private void SetTimer()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds((int)GameManager.Singleton.timer);
        timerText.text = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
    }

    public void BackToMenu()
    {
        GameManager.Singleton.BackToMenu();
    }

    public void ShowGameOverScreen()
    {
        gameOverPanel.SetActive(true);
    }
}
