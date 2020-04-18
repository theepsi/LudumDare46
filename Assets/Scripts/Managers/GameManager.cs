using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public GameObject PlayerPrefab;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        
    }
}
