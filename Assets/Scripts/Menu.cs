using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sizeXtext;
    [SerializeField] private TextMeshProUGUI sizeZtext;
    [SerializeField] private TextMeshProUGUI minesCounttext;
    [SerializeField] private Slider sizeXslider;
    [SerializeField] private Slider sizeZslider;
    [SerializeField] private Slider minesCountslider;

    public void RefreshInterface()
    {
        // when any slider value changes refresh UI text
        sizeXtext.text = $"Minefield width: {sizeXslider.value.ToString()}";
        sizeZtext.text = $"Minefield length: {sizeZslider.value.ToString()}";
        minesCounttext.text = $"Mines count: {minesCountslider.value.ToString()}";
    }

    public void Play()
    {
        // when button "Play" in UI is pressed, set up GameManager and call StartGame()
        GameManager.Singleton.sizeX = (int)sizeXslider.value;
        GameManager.Singleton.sizeZ = (int)sizeZslider.value;
        GameManager.Singleton.minesCount = (int)minesCountslider.value;
        GameManager.Singleton.StartGame();
    }

    public void Quit()
    {
        // quit application and return to desktop, NOTE: this is not working in Editor, build only
        Application.Quit();
    }
}
