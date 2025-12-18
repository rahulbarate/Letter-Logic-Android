using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PowerUp", menuName = "Power Up/Create Power Up Data")]
public class PowerUpData : ScriptableObject
{
    public Sprite powerUpUIIcon;
    public enum SubType { BombShield, CannonShield, DiamondShield, SpeedRun, Hint }
    public enum Type { Protective, Movement, Guide }
    public Type type;
    public SubType subType;

    public List<float> maxDurationsPerLevel = new List<float>(4);
    public int currentLevel = 1;
    public List<int> buyPricePerLevel = new List<int>(4);
    public List<int> upgradePricePerLevel = new List<int>(3);


    public int availableCount = 0;
}
