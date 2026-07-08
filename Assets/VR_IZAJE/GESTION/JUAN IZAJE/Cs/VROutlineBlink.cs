using UnityEngine;
using DG.Tweening;

public class VREyeBlinkCurtain : MonoBehaviour
{
    [Header("Blink Overlay")]
    [SerializeField] private CanvasGroup blinkOverlay;

    [Header("Blink Settings")]
    [SerializeField] private float fadeToBlackDuration = 0.05f;
    [SerializeField] private float fadeFromBlackDuration = 0.08f;

    private Sequence blinkSequence;

    private void Start()
    {
        if (blinkOverlay != null)
            blinkOverlay.alpha = 0f;
    }

    public void TriggerBlink(float secondsClosed)
    {
        if (blinkOverlay == null) return;

        if (blinkSequence != null && blinkSequence.IsActive())
            blinkSequence.Kill();

        blinkSequence = DOTween.Sequence();

        blinkSequence
            .Append(blinkOverlay.DOFade(1f, fadeToBlackDuration).SetEase(Ease.OutQuad))
            .AppendInterval(secondsClosed)
            .Append(blinkOverlay.DOFade(0f, fadeFromBlackDuration).SetEase(Ease.InQuad));
    }
}
