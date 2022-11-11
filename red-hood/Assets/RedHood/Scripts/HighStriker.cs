using System.Collections;
using UnityEngine;

public class HighStriker : MonoBehaviour
{
    public Coroutine CurrentRoutine { private set; get; } = null;

    [SerializeField] private Transform gauge;
    [SerializeField] private Transform bars;
    [SerializeField] private Material activatedBarMaterial;
    [SerializeField] private float translateDuration = 1.0f;

    private const float minScore = 3000f;
    private const float maxScore = 70000f;
    private const float minGauge = 0.1f;
    private const float maxGauge = 1.1f;
    private float curX;

    private static readonly float[] scaleBounds = { 0.16f, 0.39f, 0.62f, 0.83f };
    private static MeshRenderer[] barRenderers = new MeshRenderer[4];
    private static Material defaultBarMaterial;

    private void Start()
    {
        barRenderers = bars.GetComponentsInChildren<MeshRenderer>();
        defaultBarMaterial = GetComponent<MeshRenderer>().material;

        curX = minGauge;
        SetGaugeScaleX(minGauge);
    }

    private void Update()
    {
        if (transform.localPosition.y > 0.2)
        {
            transform.localPosition = new Vector3(0f, 0.2f, 0f);
        }
        else if (transform.localPosition.y < 0.1)
        {
            transform.localPosition = new Vector3(0f, 0.1f, 0f);
        }
    }

    private IEnumerator TranslateScaleX(float x)
    {
        float time = 0.0f;

        while (gauge.transform.localScale.x < x)
        {
            float newX = time / translateDuration;
            gauge.transform.localScale = new Vector3(newX, 0.1f, 0.08f);
            time += Time.deltaTime;

            for (int i = 0; i < barRenderers.Length; i++)
            {
                if (newX >= scaleBounds[i])
                {
                    barRenderers[i].material = activatedBarMaterial;
                }
                else
                {
                    barRenderers[i].material = defaultBarMaterial;
                }
            }

            yield return null;
        }
    }

    private void SetGaugeScaleX(float x)
    {
        Debug.Log($"ÇöÀç x: {curX}, new x: {x}");
        if (curX < x)
        {
            curX = x;

            StopAllCoroutines();
            CurrentRoutine = StartCoroutine(TranslateScaleX(x));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        float force = -collision.impulse.y;
        if (force < minScore)
        {
            SetGaugeScaleX(minGauge);
        }
        else if (force >= maxScore)
        {
            SetGaugeScaleX(maxGauge);
        }
        else
        {
            float x = (maxGauge - minGauge) / (maxScore - minScore) * force;
            SetGaugeScaleX(x);
        }
    }
}
