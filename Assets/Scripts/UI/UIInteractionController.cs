using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIInteractionController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private HintMechanism hintMechanism;
    [SerializeField] private AlphabetSpawner alphabetSpawner;
    [SerializeField] private NumberSpawner numberSpawner;
    [SerializeField] private WordSpawner wordSpawner;
    [SerializeField] private GameObject exitConfirmDialog;
    [SerializeField] private GameObject correctWordPanel;
    [SerializeField] private GameObject correctWordPanelAdButton;
    [SerializeField] private GameObject incorrectWordPanel;
    [SerializeField] private TextMeshProUGUI correctWordPanelTimer;
    [SerializeField] private TextMeshProUGUI incorrectWordPanelTimer;
    [SerializeField] private float timerDuration = 5f;

    private Coroutine timerCoroutine;
    private bool isTimerRunning = false;

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex >= 3)
        {
            if (correctWordPanel.activeSelf && !isTimerRunning)
            {
                isTimerRunning = true;
                timerCoroutine = StartCoroutine(StartTimer("next Word in ", NextWord, correctWordPanelTimer));
            }
            else if (incorrectWordPanel.activeSelf && !isTimerRunning)
            {
                isTimerRunning = true;
                timerCoroutine = StartCoroutine(StartTimer("Respawning in ", RespawnWord, incorrectWordPanelTimer));
            }
            else if (!correctWordPanel.activeSelf && !incorrectWordPanel.activeSelf && isTimerRunning)
            {
                StopCoroutine(timerCoroutine);
                isTimerRunning = false;
            }

        }
    }

    public void AddHint()
    {
        hintMechanism.AddHint(1);
        // ShowPopup(+1);
    }




    public void LoadLevel(int levelIndex)
    {
        // Load the specified level by index
        SceneManager.LoadScene(levelIndex);
    }
    public void TogglePauseMenu()
    {
        if (pauseMenu == null) return;

        bool isActive = pauseMenu.activeSelf;
        // PlayPauseMainTheme();
        pauseMenu.SetActive(!isActive);
        Time.timeScale = isActive ? 1f : 0f;
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
    public void QuitGame()
    {
        Application.Quit();
        CustomLogger.Log("Game is quitting");
    }

    public void UseHint()
    {
        hintMechanism.DeductHint();
    }

    public void ReviveGroundWithAd()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 1:
                alphabetSpawner.ReviveLevel();
                break;
            case 2:
                numberSpawner.ReviveLevel();
                break;
            case 3:
            case 4:
            case 5:
                wordSpawner.ReviveLevel();
                break;
            default:
                return;
        }
    }

    public void NextWord()
    {
        wordSpawner.SpawnNextWord();
        correctWordPanel.SetActive(false);
        correctWordPanelAdButton.SetActive(false);
        exitConfirmDialog.SetActive(false);
        StopCoroutine(timerCoroutine);
        Time.timeScale = 1f;
        // isTimerRunning = false;
    }

    public void RespawnWord()
    {
        // isTimerRunning = false;
        correctWordPanel.SetActive(false);
        correctWordPanelAdButton.SetActive(false);
        incorrectWordPanel.SetActive(false);
        exitConfirmDialog.SetActive(false);
        if (pauseMenu.activeSelf)
            TogglePauseMenu();
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        wordSpawner.ReviveLevel();
        Time.timeScale = 1f;
        // Time.timeScale = 1f;
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
}
