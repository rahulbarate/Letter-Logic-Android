using UnityEngine;
using System.Collections.Generic;

public class ShopUILoader : MonoBehaviour
{
    [SerializeField] List<PowerUpData> powerUps;
    [SerializeField] GameObject shopCardPrefab;

    void Awake()
    {
        if (powerUps.Count == transform.childCount)
        {
            for (int i = 0; i < powerUps.Count; i++)
            {
                transform.GetChild(i).GetComponent<ShopCardHandler>().powerUpData = powerUps[i];
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
