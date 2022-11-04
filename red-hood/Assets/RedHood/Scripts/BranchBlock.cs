using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BranchBlock : MonoBehaviour
{
    private bool condition = false;

    private XRSocketInteractor upSocket;
    private XRSocketInteractor downSocket;

    private string UP_SOCKET = "UpSocket";
    private string DOWN_SOCKET = "DownSocket";
    private string TRUE_CUBE = "TrueCube";

    private void Start()
    {
        upSocket = transform.Find(UP_SOCKET).GetComponent<XRSocketInteractor>();
        downSocket = transform.Find(DOWN_SOCKET).GetComponent<XRSocketInteractor>();
    }

    internal void SetConditionValue(bool value)
    {
        condition = value;
    }

    public void Branch()
    {
        IXRInteractable upAttach = upSocket.firstInteractableSelected;
        IXRInteractable downAttach = downSocket.firstInteractableSelected;

        if (upAttach == null || downAttach == null)
        {
            // TODO: ���� ó��(���� ����)
            Debug.Log("�б� ����: ��� ����� �������� ����");
            return;
        }

        BlockActivation upBlock = ((XRGrabInteractable)upAttach).GetComponent<BlockActivation>();
        BlockActivation downBlock = ((XRGrabInteractable)downAttach).GetComponent<BlockActivation>();

        bool isUpTrue = upBlock.CompareTag(TRUE_CUBE);
        bool isDownTrue = downBlock.CompareTag(TRUE_CUBE);

        if (isUpTrue == isDownTrue)
        {
            // TODO: ���� ó��(���� ����)
            Debug.Log("�б� ����: ��� ����� ������ ����");
            return;
        }

        BlockActivation nextBlock;

        if (condition)
        {
            nextBlock = (isUpTrue) ? upBlock : downBlock;
        }
        else
        {
            nextBlock = (isUpTrue) ? downBlock : upBlock;
        }

        nextBlock.ExecuteBlock();
    }
}
