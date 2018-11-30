using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class FadeCanvasGroup : MonoBehaviour
{
    public AnimationCurve curve;

    [Header("Fade Out")]
    public UnityEvent onStartFadeOut;
    public UnityEvent onEndFadeOut;

    [Header("Fade In")]
    public UnityEvent onStartFadeIn;
    public UnityEvent onEndFadeIn;

    private CanvasGroup canvasGroup;
    private bool fade;
    private float totalTime;
    private float elapsedTime;
    private bool fading = false;

    private void Awake()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }

    private void LateUpdate()
    {
        if( !fading )
        {
            return;
        }

        elapsedTime += Time.deltaTime;
        if( elapsedTime >= totalTime )
        {
            fading = false;
            canvasGroup.alpha = (fade) ? 0 : 1;

            if( fade )
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                onEndFadeOut.Invoke();
            }
            else
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                onEndFadeIn.Invoke();
            }

            return;
        }

        float value = curve.Evaluate(elapsedTime / totalTime);
        canvasGroup.alpha = (fade) ? (1 - value) : value;
    }

    public void FadeOut( float duration )
    {
        if( fading || canvasGroup.alpha == 0 )
        {
            return;
        }

        fade = true;
        totalTime = duration;
        elapsedTime = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        fading = true;
        onStartFadeOut.Invoke();
    }

    public void FadeIn( float duration )
    {
        if( fading || canvasGroup.alpha == 1 )
        {
            return;
        }

        fade = false;
        totalTime = duration;
        elapsedTime = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        fading = true;
        onStartFadeIn.Invoke();
    }

    public void Fade( float duration )
    {
        if( fading )
        {
            return;
        }

        if( canvasGroup.alpha == 0 )
        {
            FadeIn(duration);
            return;
        }

        if( canvasGroup.alpha == 1 )
        {
            FadeOut(duration);
            return;
        }
    }
}