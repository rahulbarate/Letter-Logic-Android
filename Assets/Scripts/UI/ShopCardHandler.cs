using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopCardHandler : MonoBehaviour
{
    public PowerUpData powerUpData;
    [SerializeField] Image powerUpIcon;
    [SerializeField] TextMeshProUGUI powerUpDetails;
    [SerializeField] GameDataSave gameDataSave;
    [SerializeField] DialogeUI dialogeUI;
    [SerializeField] ToastUI toastUI;
    [SerializeField] GameObject upgradeButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateCardDetails();
        // dialogeUI = FindFirstObjectByType<DialogeUI>(FindObjectsInactive.Include);

    }

    void UpdateCardDetails()
    {
        if (powerUpData && powerUpIcon && powerUpDetails)
        {

            powerUpIcon.sprite = powerUpData.powerUpUIIcon;
            string text = $"<b>{GetPowerUpName(powerUpData.subType)}({powerUpData.type})</b>\nAvailable: <u><color=blue>{powerUpData.availableCount}</color></u>\nLv. <u><color=blue>{powerUpData.currentLevel}/4</color></u> | Duration: <u><color=blue>{((powerUpData.maxDurationsPerLevel.Count > 0) ? powerUpData.maxDurationsPerLevel[powerUpData.currentLevel - 1] : "Infinite")}</color></u>\nPrice: <u><color=blue>{powerUpData.buyPricePerLevel[powerUpData.currentLevel - 1]}C</color></u>{(powerUpData.currentLevel < 4 ? " | Upgrade: <u><color=blue>" + powerUpData.upgradePricePerLevel[powerUpData.currentLevel - 1] + "C</color></u>" : "")}";
            powerUpDetails.text = text;
            if (powerUpData.currentLevel >= 4)
                upgradeButton.SetActive(false);
        }
    }

    string GetPowerUpName(PowerUpData.SubType subType)
    {
        if (subType == PowerUpData.SubType.BombShield)
            return "Bomb Shield";
        else if (subType == PowerUpData.SubType.CannonShield)
            return "Cannon Shield";
        else if (subType == PowerUpData.SubType.DiamondShield)
            return "Diamond Shield";
        else if (subType == PowerUpData.SubType.SpeedRun)
            return "Speed Run";
        else if (subType == PowerUpData.SubType.Hint)
            return "Hint";
        else
            return "";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowBuyDialoge()
    {
        if (dialogeUI != null)
        {
            dialogeUI.ShowDialoge("Are you sure ?", "Buy", "Cancel", Buy, $"This will add 1 quantity of <u><color=blue>{GetPowerUpName(powerUpData.subType)}</u></color> Power Up in your inventory for <u><color=blue>{powerUpData.buyPricePerLevel[powerUpData.currentLevel - 1]}C</u></color>");
        }
        else
        {
            CustomLogger.LogWarning("No Dialoge UI Found");
        }
        // if (powerUpData != null)
        //     CustomLogger.Log("Power Up Type " + powerUpData.type);
    }
    public void ShowUpgradeDialoge()
    {
        if (dialogeUI != null && toastUI)
        {
            if (powerUpData.currentLevel < 4)
            {
                dialogeUI.ShowDialoge("Are you sure ?", "Upgrade", "Cancel", Upgrade, $"This will Upgrade your <u><color=blue>{GetPowerUpName(powerUpData.subType)}</u></color> Power Up from <u><color=blue>Lv.{powerUpData.currentLevel} to the Lv.{powerUpData.currentLevel + 1}</u></color> for <u><color=blue>{powerUpData.upgradePricePerLevel[powerUpData.currentLevel - 1]}C</u></color>");
            }
            else
            {
                toastUI.ShowToast("Already at the highest upgrade level");
            }
        }
        else
        {
            CustomLogger.LogWarning("No Dialoge/Toast UI Found");
        }
        // if (powerUpData != null)
        //     CustomLogger.Log("Power Up Sub-Type " + powerUpData.subType);
    }
    public void Buy()
    {
        if (gameDataSave.TotalAvailableCoins >= powerUpData.buyPricePerLevel[powerUpData.currentLevel - 1])
        {
            gameDataSave.TotalAvailableCoins -= powerUpData.buyPricePerLevel[powerUpData.currentLevel - 1];
            powerUpData.availableCount += 1;
            // CustomLogger.Log("Bought " + GetPowerUpName(powerUpData.subType));
            toastUI.ShowToast(GetPowerUpName(powerUpData.subType) + " +1");
            AudioManager.instance.PlayPurchaseSFX();
            UpdateCardDetails();
        }
        else
        {
            // CustomLogger.Log("Not enough money!");
            toastUI.ShowToast("Not enough money!");
        }
    }
    public void Upgrade()
    {
        if (gameDataSave.TotalAvailableCoins >= powerUpData.upgradePricePerLevel[powerUpData.currentLevel - 1])
        {
            gameDataSave.TotalAvailableCoins -= powerUpData.upgradePricePerLevel[powerUpData.currentLevel - 1];
            powerUpData.currentLevel += 1;
            // CustomLogger.Log("Upgraded " + GetPowerUpName(powerUpData.subType));
            toastUI.ShowToast("Upgraded " + GetPowerUpName(powerUpData.subType) + $" Lv.{powerUpData.currentLevel - 1} -> Lv. {powerUpData.currentLevel}");
            AudioManager.instance.PlayPurchaseSFX();
            UpdateCardDetails();
            dialogeUI.HideDialoge();
        }
        else
        {
            // CustomLogger.Log("Not enough money!");
            toastUI.ShowToast("Not enough money!");
        }
    }
}
