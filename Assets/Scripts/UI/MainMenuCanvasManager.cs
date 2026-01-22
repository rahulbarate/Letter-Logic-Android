using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;
using TMPro;
public class MainMenuCanvasManager : CommonUITasks
{
    [SerializeField] private GameObject milestonePanel;
    [SerializeField] TextMeshProUGUI coinsTMPro;
    [SerializeField] GameDataSave gameDataSave;
    void Start()
    {
        MuteButtonImageCheck();
    }

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

    public void ToggleMute()
    {
        AudioManager.instance.ToggleMute();
        MuteButtonImageCheck();
    }

    public void HelpButtonClickHandler()
    {
        dialogeUI.ShowDialoge("Start Tutorial", "Yes(Recommended)", "No", LoadTutorialScene, "Are you sure you want to start a tutorial");
    }

    public void LoadTutorialScene()
    {
        SceneManager.LoadScene(6);
    }

}
