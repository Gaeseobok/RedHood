using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// �ڵ� ������ ��ư�� ������ �� ����� �ʱ�ȭ�ϰų� �����Ѵ�. (���� & ��ŸƮ)
public class OnBoardButtonPress : MonoBehaviour
{
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
    private GameObject errorMessage;

    private GameObject failureMessage;
    private GameObject successMessage;

    private const string ERROR_MESSAGE = "Error Message";
    private const string FAILURE_MESSAGE = "Failure Message";
    private const string SUCCESS_MESSAGE = "Success Message";

    private void Start()
    {
        sockets = socketsObject.GetComponentsInChildren<XRSocketInteractor>();
        pointers = socketsObject.GetComponentsInChildren<ChangeMaterial>();

        errorMessage = alertCanvas.transform.Find(ERROR_MESSAGE).gameObject;
        failureMessage = alertCanvas.transform.Find(FAILURE_MESSAGE).gameObject;
        successMessage = alertCanvas.transform.Find(SUCCESS_MESSAGE).gameObject;
    }

    private List<XRGrabInteractable> GetAttachedBlockList()
    {
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
            Destroy(block.gameObject);
        }
    }

    public void PressStartButton()
    {
        // ��� ����Ʈ ��������
        List<XRGrabInteractable> blockList = GetAttachedBlockList();

        // �ڵ� ������ ��ĭ�� ����� ��� ä������ ���� ��� �˸� �޼��� ���
        if (blockList.Count < sockets.Length)
        {
            Debug.Log("��ĭ�� ��� ä���ּ���!");
            errorMessage.GetComponent<CanvasGroup>().alpha = 1.0f;
            errorMessage.GetComponent<FadeCanvas>().StartFadeOut();
            return;
        }

        // ��� �ϳ��� activate �ϱ�

        foreach (XRGrabInteractable block in blockList)
        {
            ActivateEventArgs args = new();
            args.interactableObject = block;
            block.activated.Invoke(args);
        }
    }
}