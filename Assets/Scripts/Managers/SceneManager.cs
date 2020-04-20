using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    private AudioSource menuAudioSource = null;
    private AudioSource gameAudioSource = null;

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
        LoadScene("MainMenu", () =>
        {
            menuAudioSource = ObjectPooler.Instance.GetPooledObject("AudioSource").GetComponent<AudioSource>();

            menuAudioSource.gameObject.SetActive(true);

            menuAudioSource.clip = Resources.Load<AudioClip>("Music/MenuLoop");
            menuAudioSource.loop = true;
            menuAudioSource.Play();

            callback?.Invoke();
        });
    }

    public void LoadGame(Action callback = null)
    {
        menuAudioSource?.Stop();
        menuAudioSource?.gameObject.SetActive(false);

        menuAudioSource = null;

        LoadScene("Game", () =>
        {
            gameAudioSource = ObjectPooler.Instance.GetPooledObject("AudioSource").GetComponent<AudioSource>();

            gameAudioSource.gameObject.SetActive(true);

            gameAudioSource.clip = Resources.Load<AudioClip>("Music/GameMainLoop");
            gameAudioSource.loop = true;
            gameAudioSource.Play();

            callback?.Invoke();
        });
    }
}