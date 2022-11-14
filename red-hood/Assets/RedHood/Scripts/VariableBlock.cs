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

    // 변수의 내용을 가져온다
    public int GetInt()
    {
        return Convert.ToInt32(tmp.text);
    }

    // 변수 블록의 숫자를 초기 값으로 설정한다
    internal void SetDefaultInt()
    {
        SetInt(defaultNum);
    }

    // 변수의 내용을 변경한다
    internal void SetInt(int num)
    {
        tmp.SetText(Convert.ToString(num));
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
