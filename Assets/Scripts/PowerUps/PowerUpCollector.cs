using UnityEngine;

public class PowerUpCollector : MonoBehaviour
{
    public PowerUpData powerUpData;
    void OnEnable()
    {
        if (powerUpData.availableCount > 0)
        {
            transform.gameObject.SetActive(true);
        }
        else
        {
            transform.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (powerUpData.availableCount > 0)
        {
            transform.gameObject.SetActive(true);
        }
        else
        {
            transform.gameObject.SetActive(false);
        }
    }
}
