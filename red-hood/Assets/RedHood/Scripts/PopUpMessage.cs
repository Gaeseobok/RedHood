using System.Collections;
using TMPro;
using UnityEngine;

public class PopUpMessage : MonoBehaviour
{
    public Coroutine CurrentRoutine { private set; get; } = null;

    private static GameObject failureWindow;
    private static GameObject successWindow;
    private static GameObject clearWindow;
    private static AudioSource failureAudio;
    private static AudioSource successAudio;
    private static AudioSource clearAudio;

    private static Vector3 defaultScale;

    private const float closeDelay = 5.0f;

    private const string FAILURE_WINDOW = "FailureWindow";
    private const string SUCCESS_WINDOW = "SuccessWindow";
    private const string CLEAR_WINDOW = "ClearWindow";

    private const string OPEN_GIFT_ANIM = "OpenGift";

    private void Start()
    {
        if (CompareTag(FAILURE_WINDOW))
        {
            failureWindow = gameObject;
            failureAudio = gameObject.GetComponent<AudioSource>();
            defaultScale = transform.localScale;
            gameObject.SetActive(false);
        }
        else if (CompareTag(SUCCESS_WINDOW))
        {
            successWindow = gameObject;
            successAudio = gameObject.GetComponent<AudioSource>();
            gameObject.SetActive(false);
        }
        else if (CompareTag(CLEAR_WINDOW))
        {
            clearWindow = gameObject;
            clearAudio = gameObject.GetComponent<AudioSource>();
            gameObject.SetActive(false);
        }
    }

    internal bool isActivated()
    {
        return failureWindow.activeSelf || successWindow.activeSelf;
    }

    internal void ActivateErrorWindow(string text = null)
    {
        StopAllCoroutines();

        failureWindow.transform.localScale = defaultScale;

        failureWindow.SetActive(true);
        if (text != null)
        {
            failureWindow.GetComponentInChildren<TMP_Text>(true).SetText(text);
        }

        CurrentRoutine = StartCoroutine(DeactivateWindow(failureWindow));
    }

    internal void ActivateSuccessWindow()
    {
        StopAllCoroutines();
        successWindow.transform.localScale = defaultScale;
        successWindow.SetActive(true);
        CurrentRoutine = StartCoroutine(DeactivateWindow(successWindow));
    }

    internal void ActivateClearWindow()
    {
        StopAllCoroutines();
        clearWindow.transform.localScale = defaultScale;
        clearWindow.SetActive(true);
        clearWindow.GetComponentInChildren<Animator>().Play(OPEN_GIFT_ANIM);
        CurrentRoutine = StartCoroutine(DeactivateWindow(clearWindow));
    }

    private IEnumerator DeactivateWindow(GameObject window)
    {
        yield return new WaitForSeconds(closeDelay);

        float time = 0.0f;

        while (time <= 0.5f)
        {
            window.transform.localScale = Vector3.Lerp(defaultScale, Vector3.zero, time * 2);
            time += Time.deltaTime;
            yield return null;
        }
        window.SetActive(false);
    }

    internal void CloseAll()
    {
        failureWindow.SetActive(false);
        successWindow.SetActive(false);
    }

    internal void PlayFailureSound()
    {
        failureAudio.Play();
    }

    internal void PlaySuccessSound()
    {
        successAudio.Play();
    }

    internal void PlayClearSound()
    {
        clearAudio.Play();
    }
}
