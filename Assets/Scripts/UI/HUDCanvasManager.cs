using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class HUDCanvasManager : CommonUITasks
{

    [SerializeField] private HintMechanism hintMechanism;
    [SerializeField] private Cinemachine.CinemachineFreeLook vCam;
    [SerializeField] private Image zoomButtonImage;
    [SerializeField] private Sprite zoomInTexture;
    [SerializeField] private Sprite zoomOutTexture;
    [SerializeField] private RectTransform powerUpPanelRectTransform;
    [SerializeField] private float visibleX;
    [SerializeField] private float initialX;
    [SerializeField] private float panelAppearDuration = 0.2f;
    [SerializeField] private Image powerUpPanelToggleButton;
    [SerializeField] private Sprite leftArrow;
    [SerializeField] private Sprite rightArrow;
    [SerializeField] GameDataSave gameDataSave;


    bool isPowerUpPanelActive = false;

    void Start()
    {
        if (gameDataSave.MuteAllAudio)
            gameDataSave.audioMixer.SetFloat("MasterVolume", gameDataSave.MuteVolume);
        else
            gameDataSave.audioMixer.SetFloat("MasterVolume", gameDataSave.MaxVolume);
    }



    public void UseHint()
    {
        hintMechanism.DeductHint();
    }
    public void AddHint()
    {
        hintMechanism.AddHint(1);
        // ShowPopup(+1);
    }
    public void Zoom()
    {
        if (vCam.m_Lens.FieldOfView == 40f) // need to zoom in
        {
            vCam.m_Lens.FieldOfView = 60f;
            zoomButtonImage.sprite = zoomInTexture;
        }
        else
        {
            vCam.m_Lens.FieldOfView = 40f;
            zoomButtonImage.sprite = zoomOutTexture;
        }
    }

    public void TogglePowerUpMenu()
    {
        if (!isPowerUpPanelActive)
        {
            powerUpPanelRectTransform.DOAnchorPosX(visibleX, panelAppearDuration);
            isPowerUpPanelActive = true;
            powerUpPanelToggleButton.sprite = rightArrow;
        }
        else
        {
            powerUpPanelRectTransform.DOAnchorPosX(initialX, panelAppearDuration);
            isPowerUpPanelActive = false;
            powerUpPanelToggleButton.sprite = leftArrow;
        }

    }
    public void ToggleMute()
    {
        // mute all
        if (gameDataSave.MuteAllAudio)
        {
            gameDataSave.audioMixer.SetFloat("MasterVolume", gameDataSave.MaxVolume);
            gameDataSave.MuteAllAudio = false;
        }
        else
        {
            gameDataSave.audioMixer.SetFloat("MasterVolume", gameDataSave.MuteVolume);
            gameDataSave.MuteAllAudio = true;
        }

    }
}
