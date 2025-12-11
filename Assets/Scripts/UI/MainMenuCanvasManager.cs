using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;
using TMPro;
public class MainMenuCanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject milestonePanel;
    [SerializeField] TextMeshProUGUI coinsTMPro;
    [SerializeField] GameDataSave gameDataSave;

    void OnEnable()
    {
        UpdateAvailableCoins();
    }

    void Update()
    {
        UpdateAvailableCoins();
    }

    public void LoadLevel(int levelIndex)
    {
        // Load the specified level by index
        SceneManager.LoadScene(levelIndex);
    }
    public void QuitGame()
    {
        Application.Quit();
        CustomLogger.Log("Game is quitting");
    }

    public void ToggleMilestonePanel()
    {
        if (milestonePanel != null)
        {
            milestonePanel.SetActive(!milestonePanel.activeSelf);
        }
    }

    public void UpdateAvailableCoins()
    {
        if (gameDataSave != null && coinsTMPro != null && coinsTMPro.text != gameDataSave.TotalAvailableCoins.ToString())
            coinsTMPro.text = gameDataSave.TotalAvailableCoins.ToString();
    }

}
