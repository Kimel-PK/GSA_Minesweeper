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
    [SerializeField] private GameObject gameOverPanel;
    private int _placedFlags;
    public int PlacedFlags
    {
        get => _placedFlags;
        set
        {
            _placedFlags = value;
            placedFlagsText.text = $"Flags: {_placedFlags}/{GameManager.Singleton.minesCount}";
        }
    }

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        PlacedFlags = 0;
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
