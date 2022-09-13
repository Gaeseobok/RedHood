using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Canvas group의 오브젝트들을 페이드한다.
public class FadeCanvas : MonoBehaviour
{
    public Coroutine CurrentRoutine { private set; get; } = null;

    [SerializeField] private float fadeDuration = 1.0f;
    [SerializeField] private float fadeDelay = 0.0f;
    private CanvasGroup canvasGroup = null;
    private float alpha = 0.0f;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void StartFadeIn()
    {
        StopAllCoroutines();
        CurrentRoutine = StartCoroutine(FadeIn(fadeDuration));
    }

    public void StartFadeOut()
    {
        StopAllCoroutines();
        CurrentRoutine = StartCoroutine(FadeOutWithDelay(fadeDuration, fadeDelay));
    }

    public void SetAlpha(float value)
    {
        alpha = value;
        canvasGroup.alpha = alpha;
    }

    private IEnumerator FadeIn(float duration)
    {
        float time = 0.0f;

        while (alpha <= 1.0f)
        {
            SetAlpha(time / duration);
            time += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator FadeOutWithDelay(float duration, float delay)
    {
        yield return new WaitForSeconds(delay);

        float time = 0.0f;

        while (alpha >= 0.0f)
        {
            SetAlpha(1 - (time / duration));
            time += Time.deltaTime;
            yield return null;
        }
    }
}