using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// �ڵ� ������ ��ư�� ������ �� ����� �ʱ�ȭ�ϰų� �����Ѵ�. (���� & ��ŸƮ)
public class OnBoardButtonPress : MonoBehaviour
{
    public Coroutine CurrentRoutine { private set; get; } = null;

    [Tooltip("���ϵ��� ���� ������Ʈ")]
    [SerializeField] private GameObject socketsObject;

    [Tooltip("�˸� �޼����� ����� ĵ����")]
    [SerializeField] private Canvas alertCanvas;

    [Tooltip("���� �ڵ� ����� ����� �������� ���� �ð�")]
    [SerializeField] private float delay = 2.0f;

    // �ڵ� ����� ��ġ�ϴ� ���ϵ�
    private XRSocketInteractor[] sockets;

    // ���ϵ��� ���¸� ǥ���ϴ� ������Ʈ
    private ChangeMaterial[] pointers;

    // ��Ȳ �� �˸� �޼��� ����� ���� ����
    private FadeCanvas errorMessage;

    //private GameObject failureMessage;
    //private GameObject successMessage;

    private const string ERROR_MESSAGE = "Error Message";
    private const string FAILURE_MESSAGE = "Failure Message";
    private const string SUCCESS_MESSAGE = "Success Message";

    private void Start()
    {
        sockets = socketsObject.GetComponentsInChildren<XRSocketInteractor>();
        pointers = socketsObject.GetComponentsInChildren<ChangeMaterial>();

        errorMessage = alertCanvas.transform.Find(ERROR_MESSAGE).GetComponent<FadeCanvas>();
        //failureMessage = alertCanvas.transform.Find(FAILURE_MESSAGE).gameObject;
        //successMessage = alertCanvas.transform.Find(SUCCESS_MESSAGE).gameObject;
    }

    private List<XRGrabInteractable> GetAttachedBlockList()
    {
        // ���Ͽ� ��ġ�� ��� ����� ����Ʈ ���·� ����
        List<XRGrabInteractable> blockList = new();
        foreach (XRSocketInteractor socket in sockets)
        {
            List<IXRSelectInteractable> attachedBlocks = socket.interactablesSelected;
            if (attachedBlocks.Count != 0)
                blockList.Add((XRGrabInteractable)attachedBlocks[0]);
        }
        return blockList;
    }

    public void PressResetButton()
    {
        // ��� ����Ʈ ��������
        List<XRGrabInteractable> blockList = GetAttachedBlockList();
        // ��� ��� �����ϱ�
        foreach (XRGrabInteractable block in blockList)
        {
            // ��� ���� ���� ����(Socket_Variable)�� ���� ��ϵ� �����ϱ�
            XRSocketInteractor variableSocket = block.GetComponentInChildren<XRSocketInteractor>();
            if (variableSocket != null)
            {
                List<IXRSelectInteractable> variableBlocks = variableSocket.interactablesSelected;
                if (variableBlocks.Count > 0)
                {
                    XRGrabInteractable variableBlock = (XRGrabInteractable)variableSocket.interactablesSelected[0];
                    Destroy(variableBlock.gameObject);
                }
            }
            Destroy(block.gameObject);
        }
    }

    private bool IsSocketEmpty(List<XRGrabInteractable> blockList)
    {
        // ��� ���ο� ����ִ� ���� ����(Socket_Variable)�� �ִٸ� true ����
        foreach (XRGrabInteractable block in blockList)
        {
            XRSocketInteractor variableSocket = block.GetComponentInChildren<XRSocketInteractor>();
            if (variableSocket != null)
            {
                List<IXRSelectInteractable> variableBlocks = variableSocket.interactablesSelected;
                if (variableBlocks.Count == 0)
                    return true;
            }
        }
        return false;
    }

    private void ActivateBlock(XRGrabInteractable block)
    {
        // ����� Activated �̺�Ʈ�� Ȱ��ȭ
        ActivateEventArgs args = new();
        args.interactableObject = block;
        block.activated.Invoke(args);
    }

    private IEnumerator ExecuteBlockCodes(List<XRGrabInteractable> blockList)
    {
        // ���� �������� ����� �ϳ��� ����
        for (int i = 0; i < blockList.Count; i++)
        {
            pointers[i].ChangeToActivatedMaterial();
            ActivateBlock(blockList[i]);
            yield return new WaitForSeconds(delay);
            pointers[i].ChangeToSelectedMaterial();
        }
    }

    public void PressStartButton()
    {
        // ��� ����Ʈ ��������
        List<XRGrabInteractable> blockList = GetAttachedBlockList();

        // ��� ���Ͽ� ����� ��� ä������ ���� ��� �˸� �޼��� ���
        if (blockList.Count < sockets.Length || IsSocketEmpty(blockList))
        {
            errorMessage.SetAlpha(1.0f);
            errorMessage.StartFadeOut();
            return;
        }

        // ��� ��� �����ϱ�
        StopAllCoroutines();
        CurrentRoutine = StartCoroutine(ExecuteBlockCodes(blockList));
    }
}