using UnityEngine;

public class Test : MonoBehaviour
{
    public void TestMSG()
    {
        if (gameObject.CompareTag("IterStartBlock"))
        {
            Debug.Log("�ݺ� ���� ��� ����");
        }
        else if (gameObject.CompareTag("IterEndBlock"))
        {
            Debug.Log("�ݺ� �� ��� ����");
        }
        else if (GetComponent<ConditionBlock>())
        {
            Debug.Log("���� ��� ����");
        }
        else if (GetComponent<BranchBlock>())
        {
            Debug.Log("�б� ��� ����");
        }
        else if (gameObject.CompareTag("TrueCube"))
        {
            Debug.Log("True Cube ����");
        }
        else if (gameObject.CompareTag("FalseCube"))
        {
            Debug.Log("False Cube ����");
        }
        else
        {
            Debug.Log("��� ����~");
        }
    }
}
