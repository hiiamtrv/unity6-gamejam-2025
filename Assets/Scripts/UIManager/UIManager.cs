using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using GameManager;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI Screens")]
    [SerializeField] private GameObject pauseMenuPrefab;
    private GameObject pauseMenuInstance;
    [SerializeField] private GameObject gameOverPanelPrefab;
    private GameObject gameOverPanelInstance;
    [SerializeField] private GameObject loadingPanelPrefab;
    private GameObject loadingPanelInstance;
    [SerializeField] private RectTransform reputationPanel;
    private GameObject reputationPanelInstance;

    [SerializeField]
    private Canvas canvas;


   [SerializeField] private TMP_Text gameName;
    [SerializeField] private Slider loadingBar;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private TMP_Text pausingText;
    [SerializeField] private UnityEngine.UI.Button btnPlay;
    [SerializeField] private UnityEngine.UI.Button btnResume;
    [SerializeField] private UnityEngine.UI.Button btnPlayAgain;
    [SerializeField] private UnityEngine.UI.Button btnBackToMainMenu;

    [SerializeField] private Stack<GameObject> visibleReputationScores = new Stack<GameObject>();
    [SerializeField] private Stack<GameObject> invisibleReputationScores = new Stack<GameObject>();
    [SerializeField] private GameObject starPrefab;

    private int initialReputation;
    private int maxReputation;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);

            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene {scene.name} loaded!");
        PerformActionOnSceneLoad(scene.name);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialReputation = GameManager.GameManager.Instance.initialReputation;
        maxReputation = GameManager.GameManager.Instance.maxReputation;


        if (btnPlay != null)
        {
            btnPlay.transform.localScale = Vector3.zero;
            btnPlay.transform.DOScale(1.0f, 3.0f).SetEase(Ease.OutBack);
        }

        gameName.transform.localScale = Vector3.zero;
        gameName.transform.DOScale(1.0f, 2.0f).SetEase(Ease.OutBack).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Example: Pause the game.
    /// </summary>
    public void PauseGame()
    {
        if (pauseMenuInstance == null)
        {
            pauseMenuInstance = Instantiate(pauseMenuPrefab);
            CenterThePanel(pauseMenuInstance);
        }
        TogglePauseMenu();
        Time.timeScale = pauseMenuInstance.activeSelf ? 0 : 1; // Pause/Resume game time

        GameManager.GameManager.isPlaying = pauseMenuInstance.activeSelf ? false : true;
    }


    void CenterThePanel(GameObject panel)
    {
        if (panel != null)
        {
            panel.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            panel.transform.localScale = Vector3.one;
            panel.transform.SetParent(canvas.transform, worldPositionStays: true);
        }
    }

    /// <summary>
    /// Toggles the visibility of the pause menu.
    /// </summary>
    public void TogglePauseMenu()
    {
        if (pauseMenuInstance != null)
        {
            bool isActive = pauseMenuInstance.activeSelf;
            pauseMenuInstance.SetActive(!isActive);
            if (pauseMenuInstance.activeSelf)
            {
                pausingText.transform.localScale = Vector3.zero;
                pausingText.transform.DOScale(1.0f, 2.0f).SetEase(Ease.OutBack);

                btnResume.transform.localScale = Vector3.zero;
                btnResume.transform.DOScale(1.0f, 3.0f).SetEase(Ease.OutBack);

                btnBackToMainMenu.transform.localScale = Vector3.zero;
                btnBackToMainMenu.transform.DOScale(1.0f, 3.0f).SetEase(Ease.OutBack);
            }
        }
    }

    public void CreateReputationScores(int initialReputation)
    {
        for (int i = 0; i < initialReputation; i++)
        {
            Vector3 spawnStarPostition = GetSpawnStarPostition(canvas.GetComponent<RectTransform>());
            GameObject star = Instantiate(starPrefab);
            star.gameObject.transform.SetParent(canvas.transform, true);
            star.transform.position = spawnStarPostition + new Vector3(50 + 30 * i, -20, 0);
            star.transform.localScale = Vector3.zero;
            StartStarAppearAnimation(star);
            visibleReputationScores.Push(star);
        }

        foreach (var star in visibleReputationScores)
        {
            star.transform.DORotate(new Vector3(0, 0, 30), 0.5f)
                     .SetEase(Ease.InOutSine)
                     .SetLoops(-1, LoopType.Yoyo);
        }
    }

    Vector3 GetSpawnStarPostition(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        // corners[0]: Bottom Left
        // corners[1]: Top Left
        // corners[2]: Top Right
        // corners[3]: Bottom Right

        return corners[1];
    }

    public void UpdateReputationScores(int score)
    {
        if(GameManager.GameManager.Instance.reputation > 0)
        {
            if (invisibleReputationScores.Count > 0 && score > 0 && GameManager.GameManager.Instance.reputation < maxReputation)
            {
                for (int i = 0; i < score; i++)
                {
                    GameObject star = invisibleReputationScores.Pop();
                    star.SetActive(true);
                    StartStarAppearAnimation(star);
                    visibleReputationScores.Push(star);
                }
            }
            else if (visibleReputationScores.Count > 0 && score < 0)
            {
                for (int i = 0; i < Mathf.Abs(score); i++)
                {
                    GameObject star = visibleReputationScores.Pop();
                    StartStarDisppearAnimation(star);
                }
            }
        }
    }

    public void ShowGameOverPanel()
    {
        if(gameOverPanelInstance == null)
        {
            gameOverPanelInstance = Instantiate(gameOverPanelPrefab);
            CenterThePanel(gameOverPanelInstance);
        }
        gameOverPanelInstance.SetActive(true);
        gameOverText.transform.localScale = Vector3.zero;
        gameOverText.transform.DOScale(1.0f, 2.0f).SetEase(Ease.OutBack).SetLoops(-1, LoopType.Yoyo);

        btnPlayAgain.transform.localScale = Vector3.zero;
        btnPlayAgain.transform.DOScale(1.0f, 3.0f).SetEase(Ease.OutBack).SetLoops(-1, LoopType.Yoyo);

        btnBackToMainMenu.transform.localScale = Vector3.zero;
        btnBackToMainMenu.transform.DOScale(1.0f, 3.0f).SetEase(Ease.OutBack).SetLoops(-1, LoopType.Yoyo);
    }

    public void HideGameOverPanel()
    {
        gameOverPanelInstance.SetActive(false);
    }

    void StartStarAppearAnimation(GameObject star)
    {
        star.transform.DOScale(1.0f, 1.0f).SetEase(Ease.OutBack);
    }

    void StartStarDisppearAnimation(GameObject star)
    {
        star.gameObject.transform.DOScale(1.5f, 0.5f).SetEase(Ease.OutBack);
        star.gameObject.transform.DOScale(0, 0.5f)
                  .SetEase(Ease.InBack).OnComplete(() =>
                  {
                      star.SetActive(false);
                      invisibleReputationScores.Push(star);
                  });
    }

    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        if (loadingPanelInstance == null)
            loadingPanelInstance = Instantiate(loadingPanelPrefab);

        loadingPanelInstance.SetActive(true);
        while (!asyncOperation.isDone)
        {
            loadingBar.value = asyncOperation.progress;

            if (asyncOperation.progress >= 0.9f)
            {
                // show transition ani
                loadingPanelInstance.SetActive(false);
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public void PlayAgain()
    {
        HideGameOverPanel();
    }

    private void PerformActionOnSceneLoad(string sceneName)
    {
        if (sceneName == "MainScene")
        {
            canvas = FindAnyObjectByType<Canvas>();
            CreateReputationScores(initialReputation);
        }
        else if (sceneName == "MainMenuScene")
        {
            
        }
    }

}
