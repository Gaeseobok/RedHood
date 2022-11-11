using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// 부착된 변수 블록의 값에 따라 하이 스트라이커의 게이지 바를 활성화한다.
public class HighStrikerGauge : MonoBehaviour
{
    private XRGrabInteractable interactable;
    private int variable;

    private static readonly int[] scoreBounds = { 100, 200, 300, 400 };
    private static readonly MeshRenderer[] bars = new MeshRenderer[4];

    private const string GAUGE_BARS = "HighStrikerGaugeBars";
    private const string CODING_ZONE = "CodingZone";

    private void Start()
    {
        interactable = GetComponent<XRGrabInteractable>();
        variable = GetComponent<VariableBlock>().GetInt();

        Transform barsObject = GameObject.FindGameObjectWithTag(GAUGE_BARS).transform;
        for (int i = 0; i < bars.Length; i++)
        {
            bars[i] = barsObject.GetChild(i).GetComponent<MeshRenderer>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(CODING_ZONE) && IsSelectedBySocket())
        {
            SetGaugeBarVisibility(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(CODING_ZONE) && IsSelectedBySocket())
        {
            SetGaugeBarVisibility(false);
        }
    }

    private bool IsSelectedBySocket()
    {
        List<IXRSelectInteractor> interactors = interactable.interactorsSelecting;
        return interactors.Count != 0 && interactors[0].transform.GetComponent<XRSocketInteractor>() != null;
    }

    private void SetGaugeBarVisibility(bool visible)
    {
        for (int i = 0; i < scoreBounds.Length; i++)
        {
            if (variable == scoreBounds[i])
            {
                bars[i].enabled = visible;
                return;
            }
        }
    }
}
