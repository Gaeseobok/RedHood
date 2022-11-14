using System;
using TMPro;
using UnityEngine;

public class VariableBlock : MonoBehaviour
{
    private TMP_Text tmp;

    private int defaultNum;
    private float score;

    private void Start()
    {
        tmp = GetComponentInChildren<TMP_Text>();
        defaultNum = GetInt();
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

    // ������ �����Ѵ�
    public void SetScore()
    {
        // TODO: ������ ����ģ �� ���� Score Cylinder�� ����
        score = 250f;
    }

    public float GetScore()
    {
        SetScore();
        return score;
    }

}
