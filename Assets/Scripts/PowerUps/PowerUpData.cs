using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PowerUp", menuName = "Power Up/Create Power Up Data")]
public class PowerUpData : ScriptableObject
{
    public enum Type { BombShield, CannonShield, DiamondShield, SpeedRun }
    public Type type;

    public List<float> maxDurationsPerLevel = new List<float>(4);
    public int currentLevel = 1;
    public List<int> buyPricePerLevel = new List<int>(4);
    public List<int> upgradePricePerLevel = new List<int>(3);


    public int availableCount = 0;
}
