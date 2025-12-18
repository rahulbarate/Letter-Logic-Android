using UnityEngine;
using System.Collections.Generic;

public class PowerUpPanelLoader : MonoBehaviour
{
    [SerializeField] List<PowerUpData> powerUps;
    void Awake()
    {
        if (powerUps.Count == transform.childCount)
        {
            for (int i = 0; i < powerUps.Count; i++)
            {
                transform.GetChild(i).GetComponent<PowerUpCollector>().powerUpData = powerUps[i];
            }
        }
    }
}
