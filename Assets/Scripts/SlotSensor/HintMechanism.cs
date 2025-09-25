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
    [SerializeField] float toastAppearDuration = 0.5f;
    [SerializeField] float toastStayDuration = 2f;
    [SerializeField] float toastDisappearDuration = 0.5f;
    [SerializeField] GameObject toastUI;
    [SerializeField] TextMeshProUGUI toastUIText;
    [SerializeField] GameObject toastUIButton;
    [SerializeField] TextMeshProUGUI toastUIButtonText;
    [SerializeField] GameDataSave gameDataSave;
    private Vector3 initialPosition;
    private Color initialColor;
    private Vector3 toastInitialPosition;

    // Start is called before the first frame update
    void Start()
    {
        totalHintText.text = gameDataSave.TotalAvailableHints.ToString();
        initialPosition = hintPopupText.rectTransform.localPosition;
        initialColor = hintPopupText.color;
        toastInitialPosition = toastUI.GetComponent<RectTransform>().localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            DeductHint();
        }
    }

    public void DeductHint()
    {
        if (gameDataSave.TotalAvailableHints <= 0)
        {
            Debug.Log("No hints available!");
            return;
        }

        foreach (int index in spawner.correctSlotSensorIndex)
        {
            // CustomLogger.Log(index);
            GameObject slotSensor = slotSensorsParent.transform.GetChild(index).gameObject;
            if (slotSensor != null)
            {
                if (!slotSensor.GetComponent<Light>().enabled)
                {
                    --gameDataSave.TotalAvailableHints;
                    slotSensor.GetComponent<Light>().enabled = true;
                    totalHintText.text = gameDataSave.TotalAvailableHints.ToString();
                    ShowPopup(-1);
                }

            }
        }
        spawner.correctSlotSensorIndex.Clear();
    }

    public void AddHint()
    {
        gameDataSave.TotalAvailableHints += 1;
        totalHintText.text = gameDataSave.TotalAvailableHints.ToString();
        ShowPopup(1);
        ShowAdToast();
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

    public void ShowAdToast()
    {
        toastUI.transform.DOKill();
        toastUIText.gameObject.SetActive(false);
        toastUIButtonText.text = "+1 hint(Watch Ad)";
        toastUIButton.SetActive(true);
        toastUI.SetActive(true);
        RectTransform toastRect = toastUI.GetComponent<RectTransform>();
        toastRect.localPosition = toastInitialPosition;
        float visibleY = toastInitialPosition.y - 130f;
        toastRect.DOLocalMoveY(visibleY, toastAppearDuration).OnComplete(() =>
        {
            DOVirtual.DelayedCall(toastStayDuration, () =>
            {
                toastRect.DOLocalMoveY(toastInitialPosition.y, toastDisappearDuration).OnComplete(() =>
                {
                    toastUI.SetActive(false);
                    toastUIButton.SetActive(false);
                });
            });
        });
    }
}
