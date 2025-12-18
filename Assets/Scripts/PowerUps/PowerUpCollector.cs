using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerUpCollector : MonoBehaviour
{
    public PowerUpData powerUpData;
    public Image powerUpImage;
    public TextMeshProUGUI countTMPro;
    public PowerUpManager powerUpManager;


    void Start()
    {
        if (powerUpData.availableCount > 0)
        {
            transform.GetComponent<Button>().interactable = true;
            // transform.gameObject.SetActive(true);
        }
        else
        {
            transform.GetComponent<Button>().interactable = false;
            // transform.gameObject.SetActive(false);
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
        powerUpManager.OnPowerUpPickedUp(powerUpData);
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
