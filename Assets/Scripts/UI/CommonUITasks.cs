using UnityEngine;
using UnityEngine.UI;

public class CommonUITasks : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] DialogeUI dialogeUI;
    [SerializeField] GameSettings gameSettings;
    [SerializeField] private Sprite audioOn;
    [SerializeField] private Sprite audioOff;
    [SerializeField] private Image muteButtonImage;
    public void TogglePauseMenu()
    {
        if (pauseMenu == null) return;

        bool isActive = pauseMenu.activeSelf;
        // PlayPauseMainTheme();
        pauseMenu.SetActive(!isActive);
        Time.timeScale = isActive ? 1f : 0f;
    }
    public void QuitGame()
    {
        Application.Quit();
        CustomLogger.Log("Game is quitting");
    }
    public void ExitButton()
    {
        dialogeUI.ShowDialoge("Are you Sure?", "Yes", "No", QuitGame, "Are you sure you want to Exit the game?");
        Time.timeScale = 0f;
    }
    public void MuteButtonImageCheck()
    {
        if (gameSettings.MuteAllAudio)
        {
            muteButtonImage.sprite = audioOff;
        }
        else
        {
            muteButtonImage.sprite = audioOn;
        }
    }
}
