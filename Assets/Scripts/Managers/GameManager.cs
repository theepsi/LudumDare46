using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool godMode = false;

    public static GameManager Instance = null;

    public CameraFollow cameraFollow;

    public GameObject playerPrefab;

    public float spawnRate = 2f;
    public int spawnAmount = 2;

    [HideInInspector]
    public PlayerController player;

    private AsteroidSpawner asteroidSpawner;
    private UIManager uiManager;
    private ModuleManager moduleManager;

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
        EventManager.StartListening(Statics.Events.gameOver, OnGameOver);
        asteroidSpawner = gameObject.GetComponent<AsteroidSpawner>();
        uiManager = gameObject.GetComponent<UIManager>();
        moduleManager = gameObject.GetComponent<ModuleManager>();

        player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<PlayerController>();
        cameraFollow.SetTarget(player.transform);

        asteroidSpawner.StartSpawner();

        uiManager.Init(player.maxHull, player.minHull, player.maxOxygen, player.minOxygen);

        moduleManager.StartSpawner();
    }

    private void OnGameOver()
    {
        Debug.Log("he perdio");
        Destroy(player.gameObject);
        asteroidSpawner.StopSpawner();
    }
}
