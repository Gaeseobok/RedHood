using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    // public CanvasGroup fadeCg;

    // [Range(0.5f, 2.0f)]
    // public float fadeDuration = 1.0f;

    public void LoadScene(int index)
    {

        SceneManager.UnloadSceneAsync(index-1);

        if(index == 4)
        {
            SceneManager.LoadScene(2);
            
            GameObject.Find("--- XR ---").SetActive(false);
            GameObject.Find("--- Wolf ---").SetActive(false);

            GameObject.Find("---Nana Ending---").SetActive(true);
            GameObject.Find("Grandma_DoNotmoveHierachy").SetActive(true);
        }
        else
        {
            SceneManager.LoadScene(index);
        }
    }

    public void LoadBeginning()
    {
        SceneManager.LoadScene(1);

    }

    public void LoadEnding(GameObject nana, GameObject cam)
    {
        SceneManager.LoadScene(2);
        nana.SetActive(true);
        cam.SetActive(true);
    }

    // IEnumerator Fade(float finalAlpha)
    // {
    //     fadeCg.alpha = 1.0f;
    //     fadeCg.blocksRaycasts = true;

    //     while(!Mathf.Approximately(fadeCg.alpha, finalAlpha))
    //     {
    //         fadeCg.alpha = Mathf.MoveTowards(fadeCg.alpha, finalAlpha, fadeDuration*Time.deltaTime);

    //         yield return null;
    //     }

    //     fadeCg.blocksRaycasts = false;
    // }
}
