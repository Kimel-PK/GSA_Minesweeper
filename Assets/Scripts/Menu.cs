using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sizeXText;
    [SerializeField] private TextMeshProUGUI sizeZText;
    [SerializeField] private TextMeshProUGUI minesCountText;
    [SerializeField] private Slider sizeXSlider;
    [SerializeField] private Slider sizeZSlider;
    [SerializeField] private Slider minesCountSlider;
    [SerializeField] private GameObject howToPlayPanel;

    private void Start()
    {
        sizeXSlider.value = GameManager.Singleton.sizeX;
        sizeZSlider.value = GameManager.Singleton.sizeZ;
        minesCountSlider.value = GameManager.Singleton.minesCount;
        RefreshInterface();
    }

    public void RefreshInterface()
    {
        // when any slider value changes refresh UI text
        sizeXText.text = $"Minefield width: {sizeXSlider.value.ToString()}";
        sizeZText.text = $"Minefield length: {sizeZSlider.value.ToString()}";
        // max mines count should be dynamic depending on the minefield size
        minesCountSlider.maxValue = sizeXSlider.value * sizeZSlider.value * 0.8f;
        minesCountText.text = $"Mines count: {minesCountSlider.value.ToString()}";
    }

    public void Play()
    {
        // when button "Play" in UI is pressed, set up GameManager and call StartGame()
        GameManager.Singleton.sizeX = (int)sizeXSlider.value;
        GameManager.Singleton.sizeZ = (int)sizeZSlider.value;
        GameManager.Singleton.minesCount = (int)minesCountSlider.value;
        GameManager.Singleton.StartGame();
    }

    public void Quit()
    {
        // quit application and return to desktop, NOTE: this is not working in Editor, build only
        Application.Quit();
    }
}
