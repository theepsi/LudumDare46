using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject creditsPanel;
    public GameObject howToPlayPanel;

    public void Play()
    {
        EventManager.TriggerEvent(Statics.Events.playGame);
    }

    public void Credits()
    {
        howToPlayPanel.SetActive(false);
        creditsPanel.SetActive(!creditsPanel.activeSelf);
    }

    public void HowToPlay()
    {
        creditsPanel.SetActive(false);
        howToPlayPanel.SetActive(!howToPlayPanel.activeSelf);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
