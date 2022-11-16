using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// 떨어진 블록을 일정 시간 후 자동으로 씬에서 제거한다.
public class DestroyUnselectedBlock : MonoBehaviour
{
    private XRGrabInteractable interactable;
    private MeshRenderer[] renderers;
    private Color[] defaultColors;

    private float time;
    private const float threshold = 5.0f;
    private const float delay = 3.0f;

    private void Start()
    {
        interactable = GetComponent<XRGrabInteractable>();
        renderers = GetComponentsInChildren<MeshRenderer>();

        defaultColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            defaultColors[i] = renderers[i].material.color;
        }
    }

    private void Update()
    {
        if (interactable.isSelected)
        {
            StopAllCoroutines();
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = defaultColors[i];
            }
            time = 0;
        }
        else if (time >= threshold)
        {
            StartCoroutine(DestroyObjectWithDelay(delay));
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = Color.Lerp(defaultColors[i], Color.clear, (time - threshold) / delay);
            }
        }
        time += Time.deltaTime;
    }

    private IEnumerator DestroyObjectWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(gameObject);
    }
}
