using System.Collections;
using TMPro;
using UnityEngine;

public class PopUpMessage : MonoBehaviour
{
    public Coroutine CurrentRoutine { private set; get; } = null;

    private static GameObject errorWindow;
    private static GameObject successWindow;

    private const float delay = 5.0f;

    private const string ERROR_WINDOW = "ErrorWindow";
    private const string SUCCESS_WINDOW = "SuccessWindow";

    private void Start()
    {
        if (CompareTag(ERROR_WINDOW))
        {
            errorWindow = gameObject;
            gameObject.SetActive(false);
        }
        else if (CompareTag(SUCCESS_WINDOW))
        {
            successWindow = gameObject;
            gameObject.SetActive(false);
        }
    }

    internal bool isActivated()
    {
        return errorWindow.activeSelf || successWindow.activeSelf;
    }

    internal void ActivateErrorWindow(string text = null)
    {
        StopAllCoroutines();

        errorWindow.SetActive(true);
        if (text != null)
        {
            errorWindow.GetComponentInChildren<TMP_Text>(true).SetText(text);
        }

        CurrentRoutine = StartCoroutine(DeactivateWindow(errorWindow));
    }

    internal void ActivateSuccessWindow()
    {
        StopAllCoroutines();
        successWindow.SetActive(true);
        CurrentRoutine = StartCoroutine(DeactivateWindow(successWindow));
    }

    private IEnumerator DeactivateWindow(GameObject window)
    {
        yield return new WaitForSeconds(delay);

        window.SetActive(false);
    }
}
