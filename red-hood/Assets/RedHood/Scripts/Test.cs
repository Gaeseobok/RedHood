using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public void TestMSG()
    {
        if (gameObject.CompareTag("IterStartBlock"))
            Debug.Log("�ݺ� ���� ��� ����");
        else
            Debug.Log("��� ����~");
    }
}
