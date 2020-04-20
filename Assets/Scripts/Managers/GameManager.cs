using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public bool godMode = false;
    public bool onlyRare = false;
    public bool onlySpawnFrontWhenMoving = false;

    public float baseRadius = 10f;

    public bool enableAsteroids = true;

    public static GameManager Instance = null;

    public CanvasGroup fader;
    public GameObject blackScreen;
    public VideoPlayer videoPlayer;

    public GameObject mainUI;

    public GameObject resumePanel;
    public TextMeshProUGUI resumePanelText;
    public Image resumePanelImage;

    public Sprite winImage;
    public Sprite lostImage;

    public GameObject playerPrefab;
    public GameObject basePrefab;
    
    public LineRenderer linePrefab;

    public float spawnRate = 2f;
    public int spawnAmount = 2;

    [HideInInspector]
    public PlayerController player;

    private AsteroidSpawner asteroidSpawner;
    private UIManager uiManager;
    private ModuleManager moduleManager;
    private SceneManager sceneManager;

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
        DOTween.Init();

        sceneManager = GetComponent<SceneManager>();
        EventManager.StartListening(Statics.Events.playGame, StartGame);

        sceneManager.LoadMainMenu();
    }

    public void StartGame()
    {
        Fade(true, StartVideo);
    }

    private void StartVideo()
    {
        blackScreen.SetActive(true);
        sceneManager.menuAudioSource.Stop();

        videoPlayer.loopPointReached -= VideoEndReached;

        videoPlayer.gameObject.SetActive(true);

        videoPlayer.loopPointReached += VideoEndReached;
        videoPlayer.Play();
    }

    void VideoEndReached(VideoPlayer vp)
    {
        videoPlayer.gameObject.SetActive(false);

        PrepareAndStartGame();
    }

    public void PrepareAndStartGame()
    {
        StopAllCoroutines();

        mainUI.SetActive(false);
        resumePanel.SetActive(false);

        Action onGameSceneLoaded = () =>
        {
            blackScreen.SetActive(false);

            mainUI.SetActive(true);

            baseList = new List<GameObject>();

            EventManager.StartListening(Statics.Events.gameOver, OnGameOver);
            EventManager.StartListening(Statics.Events.baseFound, OnEndGame);

            player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<PlayerController>();

            CameraFollow cam = Camera.main.GetComponent<CameraFollow>();
            cam.SetTarget(player.transform);

            asteroidSpawner = gameObject.GetComponent<AsteroidSpawner>();
            moduleManager = gameObject.GetComponent<ModuleManager>();

            uiManager = gameObject.GetComponent<UIManager>();

            asteroidSpawner.Init(Camera.main);
            moduleManager.Init(Camera.main);

            if (enableAsteroids) asteroidSpawner.StartSpawner();

            uiManager.Init(player.maxHull, player.minHull, player.maxOxygen, player.minOxygen);

            moduleManager.StartSpawner();

            CreateBases();

            StartCoroutine(CheckBasesAreTooFarAway());

            Fade(false);
        };

        sceneManager.LoadGame(onGameSceneLoaded);
    }

    public void SkipIntro()
    {
        videoPlayer.Stop();
        VideoEndReached(videoPlayer);
    }

    /// <summary>
    /// Function to finish and win the game
    /// </summary>
    private void OnEndGame()
    {
        StartCoroutine(ShowEndScreen("YOU SURVIVED!", winImage));
    }

    /// <summary>
    /// Function for losing the game
    /// </summary>
    private void OnGameOver()
    {
        StartCoroutine(ShowEndScreen("SHIP DESTROYED!", lostImage));
    }

    private IEnumerator ShowEndScreen(string message, Sprite winorlost)
    {
        Destroy(player.gameObject);

        asteroidSpawner.StopSpawner();
        moduleManager.StopSpawner();

        yield return new WaitForSeconds(2f);

        ObjectPooler.Instance.InitializePools();

        mainUI.SetActive(false);
        resumePanel.SetActive(true);
        resumePanelText.text = message;
        resumePanelImage.sprite = winorlost;
    }

    private void CreateBases()
    {
        CleanBases();

        Vector3 firstRandomBasePosition = RandomPointOnCircleEdge(baseRadius);
        Vector3 secondRandomBasePosition = Quaternion.Euler(0, 120, 0) * firstRandomBasePosition + player.transform.position;
        Vector3 thirdRandomBasePosition = Quaternion.Euler(0, -120, 0) * firstRandomBasePosition + player.transform.position;

        firstRandomBasePosition += player.transform.position;

        baseList.Add(Instantiate(basePrefab, firstRandomBasePosition, Quaternion.Euler(new Vector3(45, 0, UnityEngine.Random.Range(-25, 25)))));
        baseList.Add(Instantiate(basePrefab, secondRandomBasePosition, Quaternion.Euler(new Vector3(45, 0, UnityEngine.Random.Range(-25, 25)))));
        baseList.Add(Instantiate(basePrefab, thirdRandomBasePosition, Quaternion.Euler(new Vector3(45, 0, UnityEngine.Random.Range(-25, 25)))));
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
    public Vector3 NearestBasePosition()
    {
        float distance = baseRadius * 1.1f;
        GameObject nearestBase = null;

        for (int i = 0; i < baseList.Count; ++i)
        {
            float baseDistance = Vector3.Distance(baseList[i].transform.position, player.transform.position);
            if (distance > baseDistance)
            {
                distance = baseDistance;
                nearestBase = baseList[i];
            }
        }

        return nearestBase.transform.position;
    }

    private Vector3 RandomPointOnCircleEdge(float radius)
    {
        Vector2 vector = UnityEngine.Random.insideUnitCircle.normalized * radius;
        return new Vector3(vector.x, 0, vector.y);
    }

    public void RestartMainMenu()
    {
        StopAllCoroutines();

        mainUI.SetActive(false);
        resumePanel.SetActive(false);
        sceneManager.LoadMainMenu();
    }

    public void Fade(bool fadeOut, Action callback = null)
    {
        fader.DOFade(fadeOut ? 1 : 0, 1).OnComplete(() => callback?.Invoke());
        fader.blocksRaycasts = fadeOut;
    }

}
