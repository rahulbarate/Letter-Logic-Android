using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ToastUI : MonoBehaviour
{
    [SerializeField] float toastAppearDuration = 0.5f;
    [SerializeField] float toastStayDuration = 2f;
    [SerializeField] float toastDisappearDuration = 0.5f;
    [SerializeField] GameObject toastUI;
    [SerializeField] TextMeshProUGUI toastUIText;
    [SerializeField] GameObject toastUIButton;
    [SerializeField] TextMeshProUGUI toastUIButtonText;
    private Vector3 toastInitialPosition;

    void Awake()
    {
        toastInitialPosition = toastUI.GetComponent<RectTransform>().localPosition;
    }


    public void ShowToast(string toastText, string buttonText)
    {
        toastUI.transform.DOKill();
        toastUIText.gameObject.SetActive(false);
        toastUIButton.SetActive(false);

        toastUIText.text = toastText;
        toastUIButtonText.text = buttonText;

        if (toastText != "")
            toastUIText.gameObject.SetActive(true);
        if (buttonText != "")
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
