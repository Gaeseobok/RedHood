using UnityEngine;

// 부착된 변수 블록의 값에 따라 하이 스트라이커의 게이지 바를 활성화한다.
public class HighStrikerGauge : MonoBehaviour
{
    private VariableBlock variable;

    private static readonly int[] scoreBounds = { 100, 200, 300, 400 };
    public static readonly MeshRenderer[] barRenderers = new MeshRenderer[4];

    private const string GAUGE_BARS = "HighStrikerGaugeBars";
    private const string CODING_ZONE = "CodingZone";

    private void Start()
    {
        variable = GetComponent<VariableBlock>();

        Transform barsObject = GameObject.FindGameObjectWithTag(GAUGE_BARS).transform;
        for (int i = 0; i < barRenderers.Length; i++)
        {
            barRenderers[i] = barsObject.GetChild(i).GetComponent<MeshRenderer>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(CODING_ZONE) && variable.IsSelectedBySocket())
        {
            SetGaugeBarVisibility(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(CODING_ZONE))
        {
            SetGaugeBarVisibility(false);
        }
    }

    private void SetGaugeBarVisibility(bool visible)
    {
        int value = variable.GetInt();

        for (int i = 0; i < scoreBounds.Length; i++)
        {
            if (value == scoreBounds[i])
            {
                barRenderers[i].enabled = visible;
                return;
            }
        }
    }
}
