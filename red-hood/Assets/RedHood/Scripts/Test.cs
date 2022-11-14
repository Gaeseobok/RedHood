using UnityEngine;

public class Test : MonoBehaviour
{
    public void TestMSG()
    {
        if (gameObject.CompareTag("IterStartBlock"))
        {
            Debug.Log("반복 시작 블록 실행");
        }
        else if (gameObject.CompareTag("IterEndBlock"))
        {
            Debug.Log("반복 끝 블록 실행");
        }
        else if (GetComponent<ConditionBlock>())
        {
            Debug.Log("조건 블록 실행");
        }
        else if (GetComponent<BranchBlock>())
        {
            Debug.Log("분기 블록 실행");
        }
        else if (gameObject.CompareTag("TrueCube"))
        {
            Debug.Log("True Cube 실행");
        }
        else if (gameObject.CompareTag("FalseCube"))
        {
            Debug.Log("False Cube 실행");
        }
        else
        {
            Debug.Log("블록 실행~");
        }
    }
}
