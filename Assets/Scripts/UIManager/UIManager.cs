using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using GameManager;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI Screens")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameScene;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private RectTransform reputationPanel;

    [SerializeField] private TMP_Text gameName;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private TMP_Text pausingText;
    [SerializeField] private Button btnStart;
    [SerializeField] private Button btnResume;
    [SerializeField] private Button btnPlayAgain;
    [SerializeField] private Button btnBacktoMainMenuFromPausePanel;
    [SerializeField] private Button btnBacktoMainMenuFromGameOverPanel;

    [SerializeField] private Stack<GameObject> visibleReputationScores = new Stack<GameObject>();
    [SerializeField] private Stack<GameObject> invisibleReputationScores = new Stack<GameObject>();
    [SerializeField] private GameObject starPrefab;

    private Tween starDisappearAnimation;

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
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //initialReputation = GameManager.GameManager.Instance.initialReputation;
        //maxReputation = GameManager.GameManager.Instance.maxReputation;
        initialReputation = maxReputation = 5;

        CreateReputationScores(initialReputation);

        btnStart.transform.localScale = Vector3.zero;
        btnStart.transform.DOScale(1.0f, 3.0f).SetEase(Ease.OutBack);

        gameName.transform.localScale = Vector3.zero;
        gameName.transform.DOScale(1.0f, 2.0f).SetEase(Ease.OutBack);

        foreach (var star in visibleReputationScores)
        {
            star.transform.DORotate(new Vector3(0, 0, 30), 0.5f)
                     .SetEase(Ease.InOutSine)
                     .SetLoops(-1, LoopType.Yoyo);
        }
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
        TogglePauseMenu();
        Time.timeScale = pauseMenu.activeSelf ? 0 : 1; // Pause/Resume game time
        GameManager.GameManager.isPlaying = pauseMenu.activeSelf ? false : true;
    }


    /// <summary>
    /// Toggles the visibility of the pause menu.
    /// </summary>
    public void TogglePauseMenu()
    {
        if (pauseMenu != null)
        {
            bool isActive = pauseMenu.activeSelf;
            pauseMenu.SetActive(!isActive);
            if (pauseMenu.activeSelf)
            {
                pausingText.transform.localScale = Vector3.zero;
                pausingText.transform.DOScale(1.0f, 2.0f).SetEase(Ease.OutBack);

                btnResume.transform.localScale = Vector3.zero;
                btnResume.transform.DOScale(1.0f, 3.0f).SetEase(Ease.OutBack);

                btnBacktoMainMenuFromPausePanel.transform.localScale = Vector3.zero;
                btnBacktoMainMenuFromPausePanel.transform.DOScale(1.0f, 3.0f).SetEase(Ease.OutBack);
            }
        }
    }

    public void CreateReputationScores(int initialReputation)
    {
        for (int i = 0; i < initialReputation; i++)
        {
            float spawnStarPostition = GetSpawnStarPostition(reputationPanel);
            GameObject star = GameObject.Instantiate(starPrefab);
            star.gameObject.transform.SetParent(reputationPanel.transform, true);
            star.transform.position = new Vector3(spawnStarPostition, reputationPanel.transform.position.y, 0) + new Vector3(50 * i, 0, 0);
            star.transform.localScale = Vector3.zero;
            StartStarAppearAnimation(star);
            visibleReputationScores.Push(star);
        }
    }

    float GetSpawnStarPostition(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        // corners[0]: Bottom Left
        // corners[1]: Top Left
        // corners[2]: Top Right
        // corners[3]: Bottom Right

        return corners[0].x;
    }

    public void UpdateReputationScores(int score)
    {
        // (GameManager.GameManager.Instance.reputation > 0)
        {
            if (invisibleReputationScores.Count > 0 && score > 0) //&& GameManager.GameManager.Instance.reputation < maxReputation
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
        gameOverPanel.SetActive(true);
        gameOverText.transform.localScale = Vector3.zero;
        gameOverText.transform.DOScale(1.0f, 2.0f).SetEase(Ease.OutBack);

        btnPlayAgain.transform.localScale = Vector3.zero;
        btnPlayAgain.transform.DOScale(1.0f, 3.0f).SetEase(Ease.OutBack);

        btnBacktoMainMenuFromGameOverPanel.transform.localScale = Vector3.zero;
        btnBacktoMainMenuFromGameOverPanel.transform.DOScale(1.0f, 3.0f).SetEase(Ease.OutBack);
    }

    public void HideGameOverPanel()
    {
        gameOverPanel.SetActive(false);
    }

    public void MinusStar()
    {
        UpdateReputationScores(-1);
    }

    public void PlusStar()
    {
        UpdateReputationScores(1);
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
}
