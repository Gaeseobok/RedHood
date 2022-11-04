using System;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VariableBlock : MonoBehaviour
{
    private float score;

    // 변수의 내용을 가져온다
    public int GetInt()
    {
        return Convert.ToInt32(GetComponentInChildren<TMP_Text>().text);
    }

    // 변수를 저장한다
    public void SetScore()
    {
        // TODO: 유저가 내려친 힘 값을 Score Cylinder에 저장
        score = 250f;
    }

    public float GetScore()
    {
        SetScore();
        return score;
    }

}
