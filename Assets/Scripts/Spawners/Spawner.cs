using System;
using UnityEngine;
using Cinemachine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
public class Spawner : MonoBehaviour
{
    public CinemachineFreeLook cineFreeCam;
    public CameraEffect cameraEffect;
    public MilestoneManager milestoneManager;
    protected float letterCubeScale = 0.93f;
    [SerializeField] public TextMeshProUGUI healthText;
    // [SerializeField] public GameObject healthBar;
    [SerializeField] public GameObject gameOverPanel;
    [SerializeField] public GameObject gameOverAdPanel;
    public int currentHealth;
    public int maxHealth = 5;
    // public int healthBarSegments;
    public PlaygroundType playgroundType = PlaygroundType.Alphabet;
    public SlotSensorsHandler slotSensorsHandler;
    public GameDataSave gameDataSave;
    public List<int> correctSlotSensorIndex = new();

    public float afterBombedSequenceDelay = 2f;
    protected GameObject single3DLetterModel;
    protected GameObject activeLetterCube;
    protected string letterChoosen;
    protected LetterCubeEventHandler activeLetterCubeEventHandler;
    protected LetterCubeMovement letterCubeMovement;
    protected bool isLevelWon = false;

    public virtual void OnPlacedInSlot(string letterOfSlotSensor) { }

    public virtual void OnLetterCubeBombed(GameObject letterCubeHit) { }
    public virtual void OnLetterCubeFell(GameObject letterCubeHit) { }

    public virtual void ReviveLevel() { }
    public virtual void GetDoubleReward() { }


    protected void TakeDamage()
    {
        --currentHealth;
        healthText.text = currentHealth.ToString();

        // Debug.Log(currentHealth);
        // if (healthBarSegments >= 1 && healthBar != null && healthBar.transform.childCount > 0)
        // {
        //     --healthBarSegments;
        //     // Debug.Log(healthBarSegments);
        //     if (healthBarSegments >= 0 && healthBar.transform.GetChild((maxHealth - healthBarSegments) - 1) != null)
        //         healthBar.transform.GetChild((maxHealth - healthBarSegments) - 1).gameObject.SetActive(false);
        // }

        if (currentHealth <= 0)
        {
            AudioManager.instance.PlayGameOverSFX();
            gameDataSave.NoOfTimesGameOver += 1;
            Time.timeScale = 0f;
            if (gameDataSave.NoOfTimesGameOver == 1 || gameDataSave.NoOfTimesGameOver == 3)
            {
                gameOverAdPanel.SetActive(true);
            }
            else
            {
                AdService.E_RewardedAdCompleted -= OnRewardedAdCompleted;
                gameOverAdPanel.SetActive(false);
            }
            gameOverPanel.SetActive(true);

            if (gameDataSave.NoOfTimesGameOver >= 5 && AdService.Instance != null)
            {
                // show Interestitial add
                AdService.Instance.ShowInterstitialAd();
                // reset counter
                gameDataSave.NoOfTimesGameOver = 0;
            }
        }

    }
    protected void OnRewardedAdCompleted()
    {
        if (isLevelWon)
        {
            GetDoubleReward();
        }
        else
        {
            ReviveLevel();
        }
        AdService.E_RewardedAdCompleted -= OnRewardedAdCompleted;
    }

    void OnDisable()
    {
        AdService.E_RewardedAdCompleted -= OnRewardedAdCompleted;
        // SceneManager.activeSceneChanged -= OnSceneChanged;
    }
    void OnDestroy()
    {
        AdService.E_RewardedAdCompleted -= OnRewardedAdCompleted;
        // SceneManager.activeSceneChanged -= OnSceneChanged;
    }
    void OnEnable()
    {
        // SceneManager.activeSceneChanged += OnSceneChanged;
    }
    // void OnSceneChanged(Scene current, Scene next)
    // {
    //     if (current != next)
    //         gameDataSave.NoOfTimesGameOver = 0;
    // }

    // void OnDisable()
    // {
    //     // SceneManager.activeSceneChanged -= OnSceneChanged;
    // }

}
