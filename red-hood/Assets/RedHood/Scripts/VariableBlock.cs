using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VariableBlock : MonoBehaviour
{
    private XRGrabInteractable interactable;
    private TMP_Text tmp;

    private int defaultNum;
    private static float score = 0.0f;

    private const string SCORE_CYLINDER = "ScoreCylinder";

    private void Start()
    {
        interactable = GetComponent<XRGrabInteractable>();
        tmp = GetComponentInChildren<TMP_Text>();
        if (tmp != null)
        {
            defaultNum = GetInt();
        }
    }

    private void Update()
    {
        Debug.Log("score : " + score);
    }

    // ������ ������ �����´�
    public int GetInt()
    {
        return Convert.ToInt32(tmp.text);
    }

    // ���� ����� ���ڸ� �ʱ� ������ �����Ѵ�
    internal void SetDefaultInt()
    {
        SetInt(defaultNum);
    }

    // ������ ������ �����Ѵ�
    internal void SetInt(int num)
    {
        tmp.SetText(Convert.ToString(num));
    }

    public float GetScore()
    {
        return score;
    }

    // ������ �����Ѵ�
    public void SetScore(float value)
    {
        // ������ ����ģ �� ���� Score Cylinder�� ����
        score = value;

        GameObject[] scoreCylinder = GameObject.FindGameObjectsWithTag(SCORE_CYLINDER);
        for (int i = 0; i < scoreCylinder.Length; i++)
        {
            TMP_Text scoreTmp = scoreCylinder[i].GetComponentInChildren<TMP_Text>();
            int text = Convert.ToInt16(value);
            scoreTmp.SetText(Convert.ToString(text));
        }
    }

    public bool IsSelectedBySocket()
    {
        List<IXRSelectInteractor> interactors = interactable.interactorsSelecting;
        return interactors.Count != 0 && interactors[0].transform.GetComponent<XRSocketInteractor>() != null;
    }
}
