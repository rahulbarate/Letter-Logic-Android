using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUIManager : CommonUITasks
{
    [SerializeField] private Spawner spawner;
    [SerializeField] private GameObject correctWordPanel;
    [SerializeField] private GameObject incorrectWordPanel;
    [SerializeField] private float timerDuration = 3f;
    [SerializeField] private TextMeshProUGUI correctWordPanelTimer;
    [SerializeField] private TextMeshProUGUI incorrectWordPanelTimer;

    [SerializeField] private GameObject reviveButtonPanel;
    [SerializeField] private GameObject getDoubleRewardPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button gameOverRestartButton;


    private Coroutine timerCoroutine;
    private bool isTimerRunning = false;
    private WordSpawner wordSpawner;


    void Start()
    {
        if (spawner is WordSpawner spawner1)
        {
            wordSpawner = spawner1;
            gameOverRestartButton.onClick.AddListener(RespawnWord);
        }
        else
        {
            gameOverRestartButton.onClick.AddListener(ReloadScene);
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex >= 3)
        {
            if (correctWordPanel.activeSelf && !isTimerRunning)
            {
                isTimerRunning = true;
                timerCoroutine = StartCoroutine(StartTimer("next Word in \n", NextWord, correctWordPanelTimer));
            }
            else if (incorrectWordPanel.activeSelf && !isTimerRunning)
            {
                isTimerRunning = true;
                timerCoroutine = StartCoroutine(StartTimer("Respawning in \n", RespawnWord, incorrectWordPanelTimer));
            }
            else if (!correctWordPanel.activeSelf && !incorrectWordPanel.activeSelf && isTimerRunning)
            {
                StopCoroutine(timerCoroutine);
                isTimerRunning = false;
            }

        }
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
    public void ReviveGroundWithAd()
    {
        if (AdService.Instance.IsRewardedAdReady())
        {
            AdService.Instance.ShowRewardedAd();
        }
        if (reviveButtonPanel != null)
            reviveButtonPanel.SetActive(false);

        // spawner.ReviveLevel();
    }
    public void GetDoubleRewardButton()
    {
        if (AdService.Instance.IsRewardedAdReady())
        {
            AdService.Instance.ShowRewardedAd();
        }
        if (getDoubleRewardPanel != null)
            getDoubleRewardPanel.SetActive(false);

    }

    private IEnumerator StartTimer(string messagePrefix, System.Action onTimerFinished, TextMeshProUGUI timerText)
    {
        float remaining = timerDuration;
        while (remaining > 0)
        {
            timerText.text = messagePrefix + Mathf.CeilToInt(remaining) + " sec";
            yield return new WaitForSeconds(1f);
            remaining -= 1f;
        }
        timerText.text = messagePrefix + "0 sec";
        onTimerFinished();
    }
    public void RespawnWord()
    {
        // isTimerRunning = false;
        correctWordPanel.SetActive(false);
        incorrectWordPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        // if (pauseMenu.activeSelf)
        // hudCanvasManager.TogglePauseMenu();
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        wordSpawner.RespawnLetterCubes();
        Time.timeScale = 1f;
        AudioManager.instance.TransitionToDefault();
        // Time.timeScale = 1f;
    }
    public void NextWord()
    {
        wordSpawner.SpawnNextWord();
        correctWordPanel.SetActive(false);
        // correctWordPanelAdButton.SetActive(false);
        // exitConfirmDialog.SetActive(false);
        StopCoroutine(timerCoroutine);
        Time.timeScale = 1f;
        // isTimerRunning = false;
    }
}
