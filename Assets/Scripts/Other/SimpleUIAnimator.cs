using UnityEngine;
using DG.Tweening;

public class SimpleUIAnimator : MonoBehaviour
{
    public enum AnimationType
    {
        None,
        OscillateLeftRight,
        OscillateUpDown,
        ZoomInOut,
        Rotate,
        FadePulse
    }

    [Header("Animation Settings")]
    public AnimationType animType = AnimationType.OscillateLeftRight;

    public float duration = 1f;
    public float strength = 20f;
    public Ease easeType = Ease.InOutSine;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        PlayAnimation();
    }

    void PlayAnimation()
    {
        switch (animType)
        {
            case AnimationType.OscillateLeftRight:
                OscillateHorizontal();
                break;

            case AnimationType.OscillateUpDown:
                OscillateVertical();
                break;

            case AnimationType.ZoomInOut:
                ZoomPulse();
                break;

            case AnimationType.Rotate:
                RotateLoop();
                break;

            case AnimationType.FadePulse:
                FadePulse();
                break;
        }
    }

    void OscillateHorizontal()
    {
        rectTransform.DOAnchorPosX(strength, duration)
            .SetRelative()
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(easeType);
    }

    void OscillateVertical()
    {
        rectTransform.DOAnchorPosY(strength, duration)
            .SetRelative()
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(easeType);
    }

    void ZoomPulse()
    {
        transform.DOScale(1 + (strength / 100f), duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(easeType);
    }

    void RotateLoop()
    {
        transform.DORotate(new Vector3(0, 0, 360), duration, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
    }

    void FadePulse()
    {
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        canvasGroup.DOFade(0.3f, duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(easeType);
    }
}
