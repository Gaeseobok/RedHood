using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class HighStrikerHit : MonoBehaviour
{
    [Tooltip("하이 스트라이커의 게이지 오브젝트")]
    [SerializeField] private Transform gauge;

    [Tooltip("활성화된 바의 머티리얼(게이지가 일정 점수를 넘어가면 해당 점수를 나타내는 바가 활성화된다)")]
    [SerializeField] private Material activatedBarMaterial;

    [Tooltip("게이지가 올라가는 데 걸리는 시간")]
    [SerializeField] private float translateDuration = 4.0f;

    [Tooltip("하이 스트라이커를 망치로 내려쳤을 때 재생할 오디오 클립")]
    [SerializeField] private AudioClip collisionSound;

    //private const float floor = 5000f;
    //private const float ceiling = 700000f;
    private const float maxScore = 7000f;
    private const float minGauge = 0.1f;
    private const float maxGauge = 1.1f;

    private static float force = 0.0f;
    private float curX;

    private static readonly float[] scaleBounds = { 0.16f, 0.39f, 0.62f, 0.83f };

    private static MeshRenderer[] barRenderers = new MeshRenderer[4];
    private static Material defaultBarMaterial;
    private AudioSource audioSource;

    private const string GAUGE_BARS = "HighStrikerGaugeBars";
    private const string QUEST_MODEL_TAG = "QuestModel";

    private void Start()
    {
        Transform barsObject = GameObject.FindGameObjectWithTag(GAUGE_BARS).transform;
        for (int i = 0; i < barRenderers.Length; i++)
        {
            barRenderers[i] = barsObject.GetChild(i).GetComponent<MeshRenderer>();
        }
        defaultBarMaterial = GetComponent<MeshRenderer>().material;

        audioSource = GetComponent<AudioSource>();

        curX = minGauge;
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

        while (time < translateDuration)
        {
            Vector3 newScale = Vector3.Lerp(new(minGauge, 0.1f, 0.08f), new((float)x, 0.1f, 0.08f), time / translateDuration);
            gauge.transform.localScale = newScale;

            time += Time.deltaTime;

            for (int i = 0; i < barRenderers.Length; i++)
            {
                barRenderers[i].material = newScale.x >= scaleBounds[i] ? activatedBarMaterial : defaultBarMaterial;
            }

            yield return null;
        }

        VariableBlock variable = gameObject.AddComponent<VariableBlock>();
        variable.SetScore((x - minGauge) * 500);
        Destroy(variable);
    }

    private void SetGaugeScaleX(float x)
    {
        Debug.Log($"현재 x: {curX}, new x: {x}");
        if (true)
        {
            //curX = x;

            StopAllCoroutines();
            StartCoroutine(TranslateScaleX(x));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(QUEST_MODEL_TAG) || force != 0.0f)
        {
            return;
        }

        Vector3 collisionForce = collision.impulse / Time.fixedDeltaTime;
        double p = Math.Sqrt(Math.Pow(collisionForce.x, 2) + Math.Pow(collisionForce.y, 2) + Math.Pow(collisionForce.z, 2));
        force = (float)p / maxScore;
        audioSource.PlayOneShot(collisionSound, force * 0.3f);

        Debug.Log("force: " + force + "\np: " + p);

        float x;

        if (force < 0.1f)
        {
            x = minGauge;
        }
        else if (force >= 1.0f)
        {
            audioSource.Play();
            x = maxGauge;
        }
        else
        {
            audioSource.Play();
            x = force + minGauge;
        }

        StopAllCoroutines();
        StartCoroutine(TranslateScaleX(x));

        Destroy(collision.gameObject, translateDuration);
    }

    public void ResetForce()
    {
        force = 0.0f;
    }
}
