using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class HintMechanism : MonoBehaviour
{

    [SerializeField] GameObject slotSensorsParent;
    [SerializeField] TextMeshProUGUI totalHintText;
    [SerializeField] Spawner spawner;
    [SerializeField] TextMeshProUGUI hintPopupText;
    [SerializeField] float hintPopupDuration = 1f;

    [SerializeField] PowerUpData hintPowerUpData;
    [SerializeField] ToastUI toastUI;
    private Vector3 initialPosition;
    private Color initialColor;


    // Start is called before the first frame update
    void Start()
    {
        if (hintPowerUpData.type != PowerUpData.Type.Guide || hintPowerUpData.subType != PowerUpData.SubType.Hint)
        {
            Debug.LogError("Wrong PowerUp Data assigned");
            return;
        }
        // totalHintText.text = hintPowerUpData.availableCount.ToString();
        // initialPosition = hintPopupText.rectTransform.localPosition;
        // initialColor = hintPopupText.color;
    }

    // Update is called once per frame  
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.H))
        // {
        //     DeductHint();
        // }
    }

    public void DeductHint()
    {
        if (hintPowerUpData.availableCount <= 0)
        {
            Debug.Log("No hints available!");
            toastUI.ShowToast("Sorry, no hints available!");
            return;
        }

        foreach (int index in spawner.correctSlotSensorIndex)
        {
            // CustomLogger.Log(index);
            GameObject slotSensor = slotSensorsParent.transform.GetChild(index).gameObject;
            if (slotSensor != null)
            {
                if (!slotSensor.GetComponent<Light>().enabled && hintPowerUpData.availableCount >= 1)
                {
                    --hintPowerUpData.availableCount;
                    // slotSensor.GetComponent<Light>().enabled = true;
                    slotSensor.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                    // totalHintText.text = hintPowerUpData.availableCount.ToString();
                    // ShowPopup(-1);
                    if (spawner is WordSpawner)
                    {
                        WordSpawner wordSpawner = (WordSpawner)spawner;
                        wordSpawner.ShowTextualHint();
                    }
                }

            }
        }
        spawner.correctSlotSensorIndex.Clear();
    }

    public void AddHint(int value, bool showAd = true)
    {
        hintPowerUpData.availableCount += value;
        // totalHintText.text = hintPowerUpData.availableCount.ToString();
        // ShowPopup(value);
        // if (showAd)
        //     toastUI.ShowToast("", "2x hint(Ad)");
    }
    public void ShowPopup(int value)
    {
        // var popup = Instantiate(popupPrefab, popupParent);
        hintPopupText.transform.DOKill(); // stop on going move transition on y axis
        hintPopupText.DOKill(); // stops on going color fade transition 

        hintPopupText.text = value > 0 ? $"+{value}" : value.ToString();

        // hintPopupText.color = value > 0 ? Color.green : Color.red;
        // setting intitial values;
        hintPopupText.color = initialColor;
        hintPopupText.rectTransform.localPosition = initialPosition;

        hintPopupText.gameObject.SetActive(true);

        float moveAmount = value > 0 ? 50f : -50f;
        hintPopupText.rectTransform.DOLocalMoveY(initialPosition.y + moveAmount, hintPopupDuration);
        hintPopupText.DOFade(0f, hintPopupDuration).OnComplete(() =>
        {
            hintPopupText.gameObject.SetActive(false);
            hintPopupText.rectTransform.localPosition = initialPosition;
            hintPopupText.color = initialColor;
        });
    }


}
