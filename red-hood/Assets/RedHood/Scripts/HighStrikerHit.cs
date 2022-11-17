using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class HighStrikerHit : MonoBehaviour
{
    [Tooltip("하이 스트라이커의 게이지 오브젝트")]
    [SerializeField] private Transform gaugeObject;

    [Tooltip("활성화된 바의 머티리얼(게이지가 일정 점수를 넘어가면 해당 점수를 나타내는 바가 활성화된다)")]
    [SerializeField] private Material activatedBarMaterial;

    [Tooltip("게이지가 올라가는 데 걸리는 시간")]
    [SerializeField] private float translateDuration = 4.0f;

    [Tooltip("하이 스트라이커를 망치로 내려쳤을 때 재생할 오디오 클립")]
    [SerializeField] private AudioClip collisionSound;

    //private const float floor = 5000f;
    //private const float ceiling = 700000f;
    private const float maxScore = 1000000f;
    private Vector3 minGauge = new(0.1f, 0.1f, 0.08f);
    private Vector3 maxGauge = new(1.1f, 0.1f, 0.08f);

    private static float score = 0.0f;

    private readonly float[] scaleBounds = { 0.16f, 0.39f, 0.62f, 0.83f };
    private Vector3 pressed = new(0f, 0.1f, 0f);
    private Vector3 released = new(0f, 0.2f, 0f);

    private static readonly MeshRenderer[] barRenderers = new MeshRenderer[4];
    private Material defaultBarMaterial;
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
    }

    private IEnumerator TranslateScaleX(Vector3 gauge)
    {
        float time = 0.0f;

        while (time < translateDuration)
        {
            // 게이지 바 크기 변경
            Vector3 newScale = Vector3.Lerp(minGauge, gauge, time / translateDuration);
            gaugeObject.transform.localScale = newScale;

            // 버튼 위치 원래대로 변경
            transform.localPosition = Vector3.Lerp(pressed, released, time / translateDuration);

            time += Time.deltaTime;

            for (int i = 0; i < barRenderers.Length; i++)
            {
                barRenderers[i].material = newScale.x >= scaleBounds[i] ? activatedBarMaterial : defaultBarMaterial;
            }
            yield return null;
        }

        // 점수 변수 설정
        VariableBlock variable = gameObject.AddComponent<VariableBlock>();
        variable.SetScore(score * 500);
        Destroy(variable);
    }

    private IEnumerator OnSmashButton(Collision collision)
    {
        float time = 0.0f;

        while (time < 0.1f)
        {
            transform.localPosition = Vector3.Lerp(released, pressed, time / 0.5f);
            time += Time.deltaTime;
            yield return null;
        }

        //Vector3 collisionForce = collision.impulse / Time.fixedDeltaTime;
        //double force = Math.Sqrt(Math.Pow(collisionForce.x, 2) + Math.Pow(collisionForce.y, 2) + Math.Pow(collisionForce.z, 2));
        //score = (float)force / maxScore;

        score = collision.relativeVelocity.magnitude / 10.0f;
        audioSource.PlayOneShot(collisionSound, score * 0.01f);

        Vector3 gauge;

        if (score < 0.1f)
        {
            gauge = minGauge;
            score = 0.1f;
        }
        else if (score >= 1.0f)
        {
            audioSource.Play();
            gauge = maxGauge;
            score = 1.0f;
        }
        else
        {
            audioSource.Play();
            gauge = minGauge;
            gauge.x += score;
        }

        StopAllCoroutines();
        _ = StartCoroutine(TranslateScaleX(gauge));

        Destroy(collision.gameObject, translateDuration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(QUEST_MODEL_TAG) && score == 0.0f)
        {
            StopAllCoroutines();
            _ = StartCoroutine(OnSmashButton(collision));
        }
    }

    public void ResetScore()
    {
        score = 0.0f;
        //gaugeObject.transform.localScale = minGauge;
        for (int i = 0; i < barRenderers.Length; i++)
        {
            barRenderers[i].material = defaultBarMaterial;
        }

        VariableBlock variable = gameObject.AddComponent<VariableBlock>();
        variable.SetScore(0.0f);
        Destroy(variable);
    }
}
