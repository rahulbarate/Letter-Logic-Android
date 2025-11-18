using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PowerUpManager : MonoBehaviour
{
    int letterCubeLayer = 6;
    int cannonBallLayer = 7;
    int bombLayer = 8;
    [SerializeField]
    LetterCubeMovement letterCubeMovement;
    // public PowerUpData cannonShield;
    // public PowerUpData bombShield;
    Dictionary<PowerUpData.Type, bool> isPowerUpActive = new Dictionary<PowerUpData.Type, bool>() {
        { PowerUpData.Type.BombShield, false },
        { PowerUpData.Type.CannonShield, false },
        { PowerUpData.Type.DiamondShield, false },
        { PowerUpData.Type.SpeedRun, false },
        };

    public void OnPowerUpPickedUp(PowerUpData powerUpData)
    {
        if (!isPowerUpActive[powerUpData.type]) // set power up active when it is not
        {
            isPowerUpActive[powerUpData.type] = true;
            TogglePowerUp(powerUpData);
            powerUpData.availableCount -= 1;
            StartCoroutine(StartPowerUpTimer(powerUpData));
            CustomLogger.Log(powerUpData.type + " is activated");
        }
        else
        {
            CustomLogger.Log(powerUpData.type + " is already active");
        }
    }

    IEnumerator StartPowerUpTimer(PowerUpData powerUpData)
    {
        yield return new WaitForSeconds(powerUpData.maxDurationsPerLevel[powerUpData.currentLevel - 1]);
        isPowerUpActive[powerUpData.type] = false;
        TogglePowerUp(powerUpData);
    }
    void TogglePowerUp(PowerUpData powerUpData)
    {
        switch (powerUpData.type)
        {
            case PowerUpData.Type.BombShield:
                Physics.IgnoreLayerCollision(letterCubeLayer, bombLayer, isPowerUpActive[powerUpData.type]);
                break;
            case PowerUpData.Type.CannonShield:
                Physics.IgnoreLayerCollision(letterCubeLayer, cannonBallLayer, isPowerUpActive[powerUpData.type]);
                break;
            case PowerUpData.Type.DiamondShield:
                Physics.IgnoreLayerCollision(letterCubeLayer, bombLayer, isPowerUpActive[powerUpData.type]);
                Physics.IgnoreLayerCollision(letterCubeLayer, cannonBallLayer, isPowerUpActive[powerUpData.type]);
                break;
            case PowerUpData.Type.SpeedRun:
                letterCubeMovement.ToggleMovementSpeed();
                break;
        }
    }
}
