using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadNextScene : MonoBehaviour
{
    [Tooltip("∑ŒµÂ«“ æ¿")]
    [SerializeField] private Scene scene;

    private FadeCanvas fadeCanvas;

    private void Start()
    {
        fadeCanvas = FindObjectOfType<FadeCanvas>();
    }

    public void Load()
    {
        StopAllCoroutines();
        _ = StartCoroutine(LoadSceneWithFade());
    }

    private IEnumerator LoadSceneWithFade()
    {
        fadeCanvas.GetComponent<Image>().color = Color.black;
        fadeCanvas.StartFadeIn();
        yield return fadeCanvas.CurrentRoutine;

        SceneManager.LoadScene(scene.name);
    }
}
