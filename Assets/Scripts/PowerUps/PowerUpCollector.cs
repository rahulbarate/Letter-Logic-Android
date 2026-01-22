using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerUpCollector : MonoBehaviour
{
    public PowerUpData powerUpData;
    public Image powerUpImage;
    public TextMeshProUGUI countTMPro;
    public PowerUpManager powerUpManager;
    public GameDataSave gameDataSave;


    void Start()
    {
        if (gameDataSave.IsTutorialOn == false)
        {
            if (powerUpData.availableCount > 0)
                transform.GetComponent<Button>().interactable = true;
            else
                transform.GetComponent<Button>().interactable = false;
        }
        UpdatePowerUpDetails();
    }
    void UpdatePowerUpDetails()
    {
        if (powerUpData)
        {
            powerUpImage.sprite = powerUpData.powerUpUIIcon;
            countTMPro.text = powerUpData.availableCount.ToString();
        }
    }

    public void PowerUpButtonClicked()
    {
        powerUpManager.ActivatePowerUp(powerUpData);
        UpdatePowerUpDetails();
    }

    void Update()
    {
        if (powerUpData.availableCount > 0)
        {
            transform.GetComponent<Button>().interactable = true;
        }
        else
        {
            transform.GetComponent<Button>().interactable = false;
        }
    }
}
