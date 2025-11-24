using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class PowerUpManager : MonoBehaviour
{
    int letterCubeLayer = 6;
    int cannonBallLayer = 7;
    int bombLayer = 8;
    [SerializeField] LetterCubeMovement letterCubeMovement;

    [SerializeField] Image protectivePowerUpImage;
    [SerializeField] TextMeshProUGUI protectivePowerUpTimer;
    [SerializeField] Image movementPowerUpImage;
    [SerializeField] TextMeshProUGUI movementPowerUpTimer;
    [SerializeField] Sprite cannonShieldTexture;
    [SerializeField] Sprite bombShieldTexture;
    [SerializeField] Sprite diamondShieldTexture;
    [SerializeField] Sprite speedRunTexture;





    // public PowerUpData cannonShield;
    // public PowerUpData bombShield;
    Dictionary<PowerUpData.SubType, bool> isPowerUpActive = new Dictionary<PowerUpData.SubType, bool>() {
        { PowerUpData.SubType.BombShield, false },
        { PowerUpData.SubType.CannonShield, false },
        { PowerUpData.SubType.DiamondShield, false },
        { PowerUpData.SubType.SpeedRun, false },
        };

    bool isProtectivePowerUpActive = false;
    bool isMovementPowerUpActive = false;

    public void OnPowerUpPickedUp(PowerUpData powerUpData)
    {
        if (powerUpData.type == PowerUpData.Type.Protective && (!isProtectivePowerUpActive))
        {
            isProtectivePowerUpActive = true;
            protectivePowerUpImage.gameObject.SetActive(true);
        }
        else if (powerUpData.type == PowerUpData.Type.Movement && (!isMovementPowerUpActive))
        {
            isMovementPowerUpActive = true;
            movementPowerUpImage.gameObject.SetActive(true);
        }
        else
            return;

        CustomLogger.Log(powerUpData.subType + " is activated");
        TogglePowerUp(powerUpData);
        powerUpData.availableCount -= 1;
        StartCoroutine(StartPowerUpTimer(powerUpData));



        // if (!isPowerUpActive[powerUpData.subType]) // set power up active when it is not
        // {
        //     isPowerUpActive[powerUpData.subType] = true;
        //     TogglePowerUp(powerUpData);
        //     powerUpData.availableCount -= 1;
        //     StartCoroutine(StartPowerUpTimer(powerUpData));
        //     CustomLogger.Log(powerUpData.subType + " is activated");
        // }
        // else
        // {
        //     CustomLogger.Log(powerUpData.subType + " is already active");
        // }
    }

    IEnumerator StartPowerUpTimer(PowerUpData powerUpData)
    {
        float duration = powerUpData.maxDurationsPerLevel[powerUpData.currentLevel - 1];
        float endTime = Time.time + duration;

        // Log initial remaining time immediately
        // CustomLogger.Log($"{powerUpData.subType} power-up started: {Mathf.CeilToInt(duration)} seconds remaining");

        if (powerUpData.type == PowerUpData.Type.Protective)
            protectivePowerUpTimer.text = Mathf.CeilToInt(duration).ToString() + "s";
        if (powerUpData.type == PowerUpData.Type.Movement)
            movementPowerUpTimer.text = Mathf.CeilToInt(duration).ToString() + "s";

        while (Time.time < endTime)
        {
            int remainingSeconds = Mathf.CeilToInt(endTime - Time.time);
            // CustomLogger.Log($"{powerUpData.subType} power-up: {remainingSeconds} seconds remaining");  

            if (powerUpData.type == PowerUpData.Type.Protective)
                protectivePowerUpTimer.text = remainingSeconds + "s";
            if (powerUpData.type == PowerUpData.Type.Movement)
                movementPowerUpTimer.text = remainingSeconds + "s";

            yield return new WaitForSeconds(1f);
        }

        // Ensure final state after timer completes
        if (powerUpData.type == PowerUpData.Type.Protective)
        {
            isProtectivePowerUpActive = false;
            protectivePowerUpImage.gameObject.SetActive(false);
        }
        if (powerUpData.type == PowerUpData.Type.Movement)
        {

            isMovementPowerUpActive = false;
            movementPowerUpImage.gameObject.SetActive(false);
        }

        TogglePowerUp(powerUpData);
    }
    void TogglePowerUp(PowerUpData powerUpData)
    {
        switch (powerUpData.subType)
        {
            case PowerUpData.SubType.BombShield:
                Physics.IgnoreLayerCollision(letterCubeLayer, bombLayer, isProtectivePowerUpActive);
                protectivePowerUpImage.sprite = bombShieldTexture;
                break;
            case PowerUpData.SubType.CannonShield:
                Physics.IgnoreLayerCollision(letterCubeLayer, cannonBallLayer, isProtectivePowerUpActive);
                protectivePowerUpImage.sprite = cannonShieldTexture;
                break;
            case PowerUpData.SubType.DiamondShield:
                Physics.IgnoreLayerCollision(letterCubeLayer, bombLayer, isProtectivePowerUpActive);
                Physics.IgnoreLayerCollision(letterCubeLayer, cannonBallLayer, isProtectivePowerUpActive);
                protectivePowerUpImage.sprite = diamondShieldTexture;
                break;
            case PowerUpData.SubType.SpeedRun:
                letterCubeMovement.ToggleMovementSpeed();
                movementPowerUpImage.sprite = speedRunTexture;
                break;
        }
    }
}
