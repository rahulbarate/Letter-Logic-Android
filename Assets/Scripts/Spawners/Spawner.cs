using System;
using UnityEngine;
using Cinemachine;
using System.Collections.Generic;
using TMPro;
public class Spawner : MonoBehaviour
{
    public CinemachineFreeLook cineFreeCam;
    public MilestoneManager milestoneManager;
    protected float letterCubeScale = 0.93f;
    [SerializeField] public TextMeshProUGUI healthText;
    // [SerializeField] public GameObject healthBar;
    [SerializeField] public GameObject gameOverPanel;
    public int currentHealth;
    public int maxHealth = 5;
    // public int healthBarSegments;
    public PlaygroundType playgroundType = PlaygroundType.Alphabet;
    public SlotSensorsHandler slotSensorsHandler;
    public GameDataSave gameDataSave;
    public List<int> correctSlotSensorIndex = new();
    protected GameObject single3DLetterModel;
    protected GameObject activeLetterCube;
    protected string letterChoosen;
    protected LetterCubeEventHandler activeLetterCubeEventHandler;
    protected LetterCubeMovement letterCubeMovement;

    public virtual void OnPlacedInSlot(string letterOfSlotSensor) { }

    public virtual void OnLetterCubeBombed(GameObject letterCubeHit) { }
    public virtual void OnLetterCubeFell(GameObject letterCubeHit) { }

    public virtual void ReviveLevel() { }

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
            Time.timeScale = 0f;
            gameOverPanel.SetActive(true);
        }

    }

}
