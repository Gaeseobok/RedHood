using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(BlockActivation))]
public class ConditionBlock : MonoBehaviour
{
    private XRSocketInteractor socket;
    private BlockActivation blockActivation;
    private PopUpMessage popUpMessage;

    private string RIGHT_SOCKET = "Right";

    private void Start()
    {
        socket = transform.Find(RIGHT_SOCKET).GetComponentInChildren<XRSocketInteractor>();
        blockActivation = GetComponent<BlockActivation>();
        popUpMessage = GetComponent<PopUpMessage>();
    }

    public void CompareVariables()
    {
        IXRInteractable attach = socket.firstInteractableSelected;

        if (attach == null)
        {
            string text = "���� ����� �������� �ʾƿ�";
            popUpMessage.ActivateErrorWindow(text);
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
            string text = "�б� ����� �������� �ʾƿ�";
            popUpMessage.ActivateErrorWindow(text);
        }
    }

}
