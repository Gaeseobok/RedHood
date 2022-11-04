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
            // TODO: ���� ó��(���� ����)
            Debug.Log("���ǹ� ����: ���� ����� �������� ����");
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
            // TODO: ���� ó��(���� ����)
            Debug.Log("���ǹ� ����: �б� ����� �������� ����");
        }
    }

}
