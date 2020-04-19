using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    private IEnumerator InternalLoadScene(string sceneToload, Action callback = null)
    {
        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneToload, UnityEngine.SceneManagement.LoadSceneMode.Single);

        while (operation.progress < 1)
        {
            yield return new WaitForEndOfFrame();
        }

        callback?.Invoke();
    }

    public void LoadScene(string sceneToload, Action callback = null)
    {
        StartCoroutine(InternalLoadScene(sceneToload, callback));
    }

    public void LoadMainMenu(Action callback = null)
    {
        LoadScene("MainMenu", callback);
    }

    public void LoadGame(Action callback = null)
    {
        LoadScene("Game", callback);
    }
}