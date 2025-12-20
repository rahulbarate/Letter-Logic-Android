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
    // [SerializeField] GameObject toastUIButton;
    // [SerializeField] TextMeshProUGUI toastUIButtonText;
    // [SerializeField] Vector3 offScreenPosition;
    [SerializeField] float visibleY = 280f;
    [SerializeField] float initialY = 500f;
    private Vector3 toastInitialPosition;

    // Queue system for handling multiple toasts
    private Queue<string> toastQueue = new Queue<string>();
    private bool isProcessingQueue = false;

    void Awake()
    {
        toastInitialPosition = toastUI.GetComponent<RectTransform>().position;
    }

    public void ShowToast(string toastText)
    {
        // Debug.Log($"[ToastUI] ShowToast called with: '{toastText}' | Queue count: {toastQueue.Count}");

        // Add to queue
        toastQueue.Enqueue(toastText);

        // Process queue if not already processing
        if (!isProcessingQueue)
        {
            ProcessNextToast();
        }
    }

    private void ProcessNextToast()
    {
        if (toastQueue.Count == 0)
        {
            isProcessingQueue = false;
            return;
        }

        isProcessingQueue = true;
        var toastText = toastQueue.Dequeue();

        // Debug.Log($"[ToastUI] Processing toast: '{toastText}' | Remaining in queue: {toastQueue.Count}");

        // Stop any existing animations and reset state
        toastUI.transform.DOKill();

        // Reset UI elements
        toastUIText.gameObject.SetActive(false);
        // toastUIButton.SetActive(false);
        toastUI.SetActive(false);

        toastUIText.text = toastText;
        // toastUIButtonText.text = buttonText;

        if (toastText != "")
            toastUIText.gameObject.SetActive(true);
        // if (buttonText != "")
        //     toastUIButton.SetActive(true);

        toastUI.SetActive(true);

        RectTransform toastRect = toastUI.GetComponent<RectTransform>();
        // toastRect.localPosition = toastInitialPosition;
        // float visibleY = toastInitialPosition.y - 180f;

        // Debug.Log($"[ToastUI] Starting appear animation for: '{toastText}'");
        toastRect.DOAnchorPosY(visibleY, toastAppearDuration).OnComplete(() =>
        {
            // Debug.Log($"[ToastUI] Appear complete for: '{toastText}', starting stay timer");
            DOVirtual.DelayedCall(toastStayDuration, () =>
            {
                // Debug.Log($"[ToastUI] Stay timer complete for: '{toastText}', starting disappear animation");
                toastRect.DOAnchorPosY(initialY, toastDisappearDuration).OnComplete(() =>
                {
                    // Debug.Log($"[ToastUI] Disappear complete for: '{toastText}', processing next toast");
                    // Process next toast after current one completes
                    ProcessNextToast();
                });
            });
        });
    }

    
}
