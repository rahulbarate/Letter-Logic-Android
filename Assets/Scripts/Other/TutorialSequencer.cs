using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(LetterCubeMovement))]
public class TutorialSequencer : MonoBehaviour
{
    [SerializeField] GameDataSave gameDataSave;

    [SerializeField] GameObject movementUI;
    [SerializeField] GameObject placementUI;
    [SerializeField] GameObject powerupUI;
    [SerializeField] GameObject startHereObj;
    [SerializeField] TextMeshProUGUI powerupUITMPro;
    [SerializeField] GameObject powerupArrow;
    [SerializeField] DialogeUI dialogeUI;
    [SerializeField] LetterCubeMovement letterCubeMovement;

    [Header("AnimationSettings")]
    [SerializeField] float duration = 0.5f;
    Ease easeOut = Ease.InBack;
    Ease easeIn = Ease.OutBack;

    public enum GuideState
    {
        Movement,
        Placement,
        PowerUp,
    }

    public GuideState guideState = GuideState.Movement;

#if UNITY_EDITOR
    void OnValidate()
    {
        if (gameObject.scene.IsValid() == false)
            return;
        if (gameDataSave == null)
            CustomLogger.LogError("Assign gameDataSave");
        if (movementUI == null)
            CustomLogger.LogError("Assign movementUI");
        if (placementUI == null)
            CustomLogger.LogError("Assign placementUI");
        if (startHereObj == null)
            CustomLogger.LogError("Assign startHereObj");
        if (powerupUI == null)
            CustomLogger.LogError("Assign powerupUI");
        if (powerupUITMPro == null)
            CustomLogger.LogError("Assign powerupUITMPro");
        if (powerupArrow == null)
            CustomLogger.LogError("Assign PowerupArrow");
        if (dialogeUI == null)
            CustomLogger.LogError("Assign dialogeUI");
    }
#endif

    void Awake()
    {
        gameDataSave.IsTutorialOn = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        letterCubeMovement = GetComponent<LetterCubeMovement>();
        placementUI.transform.localScale = Vector3.zero; // setting this for animation purpose.
        placementUI.SetActive(false);
        powerupUI.transform.localScale = Vector3.zero; // setting this for animation purpose.
        powerupUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartPlacementSequence()
    {
        if (guideState != GuideState.Movement)
            return;
        Sequence seq = DOTween.Sequence();
        seq.Append(
            movementUI.transform.DOScale(0f, duration).SetEase(easeOut)
        );
        seq.AppendCallback(() =>
        {
            guideState = GuideState.Placement;
            movementUI.SetActive(false);
            placementUI.SetActive(true);
            startHereObj.SetActive(true);
        });
        seq.Append(
            placementUI.transform.DOScale(1f, duration).SetEase(easeIn)
        );

    }

    public void StartPowerupSequence()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(
            placementUI.transform.DOScale(0f, duration).SetEase(easeOut)
        );
        seq.AppendCallback(() =>
        {
            guideState = GuideState.PowerUp;
            movementUI.SetActive(false);
            placementUI.SetActive(false);
            powerupUI.SetActive(true);
            letterCubeMovement.disableMovement = true;
        });
        seq.Append(
            powerupUI.transform.DOScale(1f, duration).SetEase(easeIn)
        );
    }

    public void PowerupPanelOnSequence()
    {
        if (guideState != GuideState.PowerUp)
            return;
        powerupUITMPro.text = "Select Hint power up";
    }

    public void HintPowerupOnSequence()
    {
        if (guideState != GuideState.PowerUp)
            return;
        powerupUITMPro.text = "Correct slot is glowing!\nPlace the Letter Cube in it.";
        powerupArrow.SetActive(false);
        letterCubeMovement.disableMovement = false;
    }

    public void EndTutorial()
    {
        powerupUI.SetActive(false);
        Time.timeScale = 0f;
        dialogeUI.ShowDialoge("Tutorial is Complete!", "Restart", "Main Menu", () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex), "Would you like to Restart or head back to Main Menu?", () => { Time.timeScale = 1f; SceneManager.LoadScene(0); gameDataSave.IsTutorialOn = false; });
    }

    void OnDestroy()
    {
        if (movementUI != null) DOTween.Kill(movementUI.transform);
        if (placementUI != null) DOTween.Kill(placementUI.transform);
        if (powerupUI != null) DOTween.Kill(powerupUI.transform);
    }
}
