using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(BlockActivation))]
public class ConditionBlock : MonoBehaviour
{
    private XRSocketInteractor socket;
    private BlockActivation blockActivation;

    private string RIGHT_SOCKET = "Right";

    private void Start()
    {
        socket = transform.Find(RIGHT_SOCKET).GetComponentInChildren<XRSocketInteractor>();
        blockActivation = GetComponent<BlockActivation>();
    }

    public void CompareVariables()
    {
        IXRInteractable attach = socket.firstInteractableSelected;

        if (attach == null)
        {
            // TODO: 에러 처리(문제 오답)
            Debug.Log("조건문 오류: 변수 블록이 존재하지 않음");
            return;
        }

        VariableBlock intVar = ((XRGrabInteractable)attach).GetComponent<VariableBlock>();

        bool condition = intVar.GetScore() > intVar.GetInt();

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
