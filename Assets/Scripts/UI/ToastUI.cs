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

    // Milestone Popup System
    [Header("Milestone Popup Settings")]
    [SerializeField] GameObject milestonePopupUI;
    [SerializeField] TextMeshProUGUI milestoneIDText;
    [SerializeField] TextMeshProUGUI rewardedCoinText;
    [SerializeField] UnityEngine.UI.Image milestoneImage;
    [SerializeField] Color defaultColor = Color.gray;
    [SerializeField] Color milestoneAchievedColor = Color.green;
    [SerializeField] float imageFillReduceDuration = 1f;
    [SerializeField] float imageFillIncreaseDuration = 1f;
    [SerializeField] float milestoneAppearDuration = 0.5f;
    [SerializeField] float milestoneDisappearDuration = 0.5f;
    [SerializeField] float milestoneStayDuration = 0.5f;
    [SerializeField] float visibleX = 0f;
    [SerializeField] float initialX = 800f;

    // Queue system for handling multiple milestone popups
    private Queue<(int milestoneID, int rewardedCoin)> milestoneQueue = new Queue<(int, int)>();
    private bool isProcessingMilestoneQueue = false;

    void Awake()
    {
        toastInitialPosition = toastUI.GetComponent<RectTransform>().position;

        // Initialize milestone popup
        if (milestoneImage != null)
        {
            milestoneImage.color = defaultColor;
            milestoneImage.fillAmount = 1f;
        }
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

    public void ShowMilestonePopup(int milestoneID, int rewardedCoin)
    {
        // Add to milestone queue
        milestoneQueue.Enqueue((milestoneID, rewardedCoin));

        // Process queue if not already processing
        if (!isProcessingMilestoneQueue)
        {
            ProcessNextMilestonePopup();
        }
    }

    private void ProcessNextMilestonePopup()
    {
        if (milestoneQueue.Count == 0)
        {
            isProcessingMilestoneQueue = false;
            return;
        }

        isProcessingMilestoneQueue = true;
        var (milestoneID, rewardedCoin) = milestoneQueue.Dequeue();

        // Stop any existing animations and reset state
        milestonePopupUI.transform.DOKill();
        if (milestoneImage != null)
        {
            milestoneImage.DOKill();
        }

        // Reset UI elements
        milestonePopupUI.SetActive(false);

        // Set text values
        if (milestoneIDText != null)
            milestoneIDText.text = "Milestone Achieved: " + milestoneID.ToString();
        if (rewardedCoinText != null)
            rewardedCoinText.text = "Reward coins: " + rewardedCoin.ToString();

        // Reset image state
        if (milestoneImage != null)
        {
            milestoneImage.color = defaultColor;
            milestoneImage.fillAmount = 1f;
        }

        milestonePopupUI.SetActive(true);

        RectTransform milestoneRect = milestonePopupUI.GetComponent<RectTransform>();

        // Start the milestone animation sequence
        milestoneRect.DOAnchorPosX(visibleX, milestoneAppearDuration).OnComplete(() =>
        {
            // Step 1: Reduce fill amount from 1 to 0
            if (milestoneImage != null)
            {
                milestoneImage.DOFillAmount(0f, imageFillReduceDuration).OnComplete(() =>
                {
                    // Step 2: Change color to achieved color
                    milestoneImage.DOColor(milestoneAchievedColor, 0.3f).OnComplete(() =>
                    {
                        // Step 3: Increase fill amount from 0 to 1
                        milestoneImage.DOFillAmount(1f, imageFillIncreaseDuration).OnComplete(() =>
                        {
                            DOVirtual.DelayedCall(milestoneStayDuration, () =>
                            {
                                // Step 4: Hide popup and reset color
                                milestoneRect.DOAnchorPosX(initialX, milestoneDisappearDuration).OnComplete(() =>
                                {
                                    milestoneImage.color = defaultColor;
                                    ProcessNextMilestonePopup();
                                    // milestoneImage.DOColor(defaultColor, 0.3f).OnComplete(() =>
                                    // {
                                    //     // Process next milestone popup
                                    // });
                                });
                            });
                        });
                    });
                });
            }
            else
            {
                // If no milestone image, just hide after a delay
                DOVirtual.DelayedCall(2f, () =>
                {
                    milestoneRect.DOAnchorPosX(initialX, milestoneDisappearDuration).OnComplete(() =>
                    {
                        ProcessNextMilestonePopup();
                    });
                });
            }
        });
    }


}
