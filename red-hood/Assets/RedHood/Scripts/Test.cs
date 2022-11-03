using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public void TestMSG()
    {
        if (gameObject.CompareTag("IterStartBlock"))
            Debug.Log("반복 시작 블록 실행");
        else
            Debug.Log("블록 실행~");
    }
}
