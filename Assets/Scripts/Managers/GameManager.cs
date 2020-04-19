using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool godMode = false;

    public float baseRadius = 10f;

    public bool enableAsteroids = true;

    public static GameManager Instance = null;

    public CameraFollow cameraFollow;

    public GameObject playerPrefab;
    public GameObject basePrefab;

    public float spawnRate = 2f;
    public int spawnAmount = 2;

    [HideInInspector]
    public PlayerController player;

    private AsteroidSpawner asteroidSpawner;
    private UIManager uiManager;
    private ModuleManager moduleManager;

    private List<GameObject> baseList;

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
        baseList = new List<GameObject>();

        EventManager.StartListening(Statics.Events.gameOver, OnGameOver);
        EventManager.StartListening(Statics.Events.baseFound, OnEndGame);

        asteroidSpawner = gameObject.GetComponent<AsteroidSpawner>();
        uiManager = gameObject.GetComponent<UIManager>();
        moduleManager = gameObject.GetComponent<ModuleManager>();

        player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<PlayerController>();
        cameraFollow.SetTarget(player.transform);

        if (enableAsteroids) asteroidSpawner.StartSpawner();

        uiManager.Init(player.maxHull, player.minHull, player.maxOxygen, player.minOxygen);

        moduleManager.StartSpawner();

        CreateBases();

        StartCoroutine(CheckBasesAreTooFarAway());
    }

    /// <summary>
    /// Function to finish and win the game
    /// </summary>
    private void OnEndGame()
    {
        Debug.Log("YOU WIN!");
        Destroy(player.gameObject);
        asteroidSpawner.StopSpawner();
        moduleManager.StopSpawner();

        StopAllCoroutines();
    }

    /// <summary>
    /// Function for losing the game
    /// </summary>
    private void OnGameOver()
    {
        Debug.Log("YOU LOST!");
        Destroy(player.gameObject);
        asteroidSpawner.StopSpawner();
        moduleManager.StopSpawner();

        StopAllCoroutines();
    }

    private void CreateBases()
    {
        CleanBases();

        Vector3 firstRandomBasePosition = RandomPointOnCircleEdge(baseRadius);
        Vector3 secondRandomBasePosition = Quaternion.Euler(0, 120, 0) * firstRandomBasePosition + player.transform.position;
        Vector3 thirdRandomBasePosition = Quaternion.Euler(0, -120, 0) * firstRandomBasePosition + player.transform.position;

        firstRandomBasePosition += player.transform.position;

        baseList.Add(Instantiate(basePrefab, firstRandomBasePosition, Quaternion.identity));
        baseList.Add(Instantiate(basePrefab, secondRandomBasePosition, Quaternion.identity));
        baseList.Add(Instantiate(basePrefab, thirdRandomBasePosition, Quaternion.identity));
    }

    private void CleanBases()
    {
        for (int i = 0; i < baseList.Count; ++i)
        {
            Destroy(baseList[i].gameObject);
        }

        baseList.Clear();
    }

    private IEnumerator CheckBasesAreTooFarAway()
    {
        List<float> distances;
        for (; ; )
        {
            yield return new WaitForSeconds(3f);
            if (player != null)
            {
                distances = new List<float>();

                for (int i = 0; i < baseList.Count; ++i)
                {
                    distances.Add(Vector3.Distance(baseList[i].transform.position, player.transform.position));
                }

                if (distances.Min() > baseRadius)
                {
                    CreateBases();
                }
            }
        }
    }

    private Vector3 RandomPointOnCircleEdge(float radius)
    {
        Vector2 vector = Random.insideUnitCircle.normalized * radius;
        return new Vector3(vector.x, 0, vector.y);
    }
}
