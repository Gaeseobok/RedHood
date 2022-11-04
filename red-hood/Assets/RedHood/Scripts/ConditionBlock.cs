using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ConditionBlock : MonoBehaviour
{
    private XRSocketInteractor leftSocket;
    private XRSocketInteractor rightSocket;
    private BlockActivation blockActivation;

    private string LEFT_SOCKET = "Left";
    private string RIGHT_SOCKET = "Right";

    private void Start()
    {
        leftSocket = transform.Find(LEFT_SOCKET).GetComponentInChildren<XRSocketInteractor>();
        rightSocket = transform.Find(RIGHT_SOCKET).GetComponentInChildren<XRSocketInteractor>();
        blockActivation = GetComponent<BlockActivation>();
    }

    public void CompareVariables()
    {
        IXRInteractable leftAttach = leftSocket.firstInteractableSelected;
        IXRInteractable rightAttach = rightSocket.firstInteractableSelected;

        if (leftAttach == null || rightAttach == null)
        {
            // TODO: 에러 처리(문제 오답)
            Debug.Log("조건문 오류: 변수 블록이 존재하지 않음");
            return;
        }

        VariableBlock scoreVar = ((XRGrabInteractable)leftAttach).GetComponent<VariableBlock>();
        VariableBlock intVar = ((XRGrabInteractable)rightAttach).GetComponent<VariableBlock>();

        bool condition = scoreVar.GetScore() > intVar.GetInt();

        BlockActivation nextBlock = blockActivation.GetNextBlock();

        if (nextBlock != null)
        {
            nextBlock.GetComponent<BranchBlock>().SetConditionValue(condition);
        }
        else
        {
            // TODO: 에러 처리(문제 오답)
            Debug.Log("조건문 오류: 분기 블록이 존재하지 않음");
        }
    }

}
