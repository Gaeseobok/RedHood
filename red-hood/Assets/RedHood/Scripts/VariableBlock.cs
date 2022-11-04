using System;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VariableBlock : MonoBehaviour
{
    private float score;

    // ������ ������ �����´�
    public int GetInt()
    {
        return Convert.ToInt32(GetComponentInChildren<TMP_Text>().text);
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
