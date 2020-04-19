using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        EventManager.TriggerEvent(Statics.Events.playGame);
    }

    public void Credits()
    {
        //Open credit page
    }

    public void Quit()
    {
        Application.Quit();
    }
}
