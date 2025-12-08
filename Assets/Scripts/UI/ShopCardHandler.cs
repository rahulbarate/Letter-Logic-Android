using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopCardHandler : MonoBehaviour
{
    public PowerUpData powerUpData;
    [SerializeField] Image powerUpIcon;
    [SerializeField] TextMeshProUGUI powerUpDetails;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (powerUpData)
        {
            if (powerUpIcon)
                powerUpIcon.sprite = powerUpData.powerUpUIIcon;
            if (powerUpDetails)
            {
                string text = $"{powerUpData.type}\n{powerUpData.subType}\nLv. {powerUpData.currentLevel}/4 | Duration {powerUpData.maxDurationsPerLevel[powerUpData.currentLevel - 1]}s\nPrice: {powerUpData.buyPricePerLevel[powerUpData.currentLevel - 1]} | Upgrade: {(powerUpData.currentLevel < 4 ? powerUpData.upgradePricePerLevel[powerUpData.currentLevel - 1] : "-")}";
                powerUpDetails.text = text;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowBuyDialoge()
    {
        if (powerUpData != null)
            CustomLogger.Log("Power Up Type " + powerUpData.type);
    }
    public void ShowUpgradeDialoge()
    {
        if (powerUpData != null)
            CustomLogger.Log("Power Up Sub-Type " + powerUpData.subType);
    }
}
