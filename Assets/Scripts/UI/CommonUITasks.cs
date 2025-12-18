using UnityEngine;

public class CommonUITasks : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] DialogeUI dialogeUI;
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
}
