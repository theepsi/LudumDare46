using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private void AnimateOnEnter(BaseEventData eventData)
    {
        
    }

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
