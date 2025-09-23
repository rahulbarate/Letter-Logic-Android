using System;
using UnityEngine;
using Cinemachine;
public class Spawner : MonoBehaviour
{
    public CinemachineFreeLook cineFreeCam;
    public float letterCubeScale = 97f;
    [SerializeField] public GameObject healthBar;
    [SerializeField] public GameObject gameOverPanel;
    public int currentHealth;
    public int maxHealth = 5;
    public int healthBarSegments;
    public PlaygroundType playgroundType = PlaygroundType.Alphabet;
    public SlotSensorsHandler slotSensorsHandler;
    public GameDataSave gameDataSave;

    int letterCubeToFetch;
    public int LetterCubeToFetch
    {
        get { return letterCubeToFetch; }
        set { letterCubeToFetch = value; }
    }

    //
    protected GameObject single3DLetterModel;
    protected GameObject activeLetterCube;
    protected string letterChoosen;
    protected LetterCubeEventHandler activeLetterCubeEventHandler;
    protected LetterCubeMovement letterCubeMovement;
    public virtual void OnPlacedInSlot(string letterOfSlotSensor) { }

    public virtual void OnLetterCubeBombed(GameObject letterCubeHit) { }

    public virtual void ReviveLevel() { }

    protected void TakeDamage()
    {
        --currentHealth;
        // Debug.Log(currentHealth);
        if (healthBarSegments >= 1 && healthBar != null && healthBar.transform.childCount > 0)
        {
            --healthBarSegments;
            Debug.Log(healthBarSegments);
            if (healthBarSegments >= 0 && healthBar.transform.GetChild(healthBarSegments) != null)
                healthBar.transform.GetChild(healthBarSegments).gameObject.SetActive(false);
        }

        if (currentHealth <= 0)
        {
            Time.timeScale = 0f;
            gameOverPanel.SetActive(true);
        }
    }

}
