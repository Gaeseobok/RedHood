using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System;

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

    private const string ITERATION_START_TAG = "IterationStart";
    private const string ITERATION_END_TAG = "IterationEnd";

    private void Start()
    {
        sockets = socketsObject.GetComponentsInChildren<XRSocketInteractor>();
        pointers = socketsObject.GetComponentsInChildren<ChangeMaterial>();

        errorMessage = alertCanvas.transform.Find(ERROR_MESSAGE).GetComponent<FadeCanvas>();
        //failureMessage = alertCanvas.transform.Find(FAILURE_MESSAGE).gameObject;
        //successMessage = alertCanvas.transform.Find(SUCCESS_MESSAGE).gameObject;
    }

    // ���Ͽ� ��ġ�� ��� ����� ����Ʈ ���·� �����Ѵ�.
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

    // ��� ���� ���� ����(Socket_Variable)�� ���� ����� �����Ѵٸ� �ش� ���� ����� �����Ѵ�.
    private GameObject GetAttachedVariableBlock(XRGrabInteractable block)
    {
        XRSocketInteractor variableSocket = block.GetComponentInChildren<XRSocketInteractor>();
        if (variableSocket != null)
        {
            List<IXRSelectInteractable> variableBlocks = variableSocket.interactablesSelected;
            if (variableBlocks.Count > 0)
            {
                XRGrabInteractable variableBlock = (XRGrabInteractable)variableSocket.interactablesSelected[0];
                return variableBlock.gameObject;
            }
        }
        return null;
    }

    // ���� ��ư�� �������� ��, ���Ͽ� ��ġ�� ��� ����� �����Ѵ�.
    public void PressResetButton()
    {
        // ��� ����Ʈ ��������
        List<XRGrabInteractable> blockList = GetAttachedBlockList();
        // ��� ��� �����ϱ�
        foreach (XRGrabInteractable block in blockList)
        {
            // ���� ����� �����Ѵٸ� �����ϱ�
            GameObject variableBlock = GetAttachedVariableBlock(block);
            if (variableBlock != null)
                Destroy(variableBlock);
            Destroy(block.gameObject);
        }
    }

    // ��� ���ο� ����ִ� ���� ����(Socket_Variable)�� �ִ��� ����Ѵ�.
    private bool IsSocketEmpty(List<XRGrabInteractable> blockList)
    {
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

    // ����� Activated �̺�Ʈ�� Ȱ��ȭ�Ѵ�.
    private void ActivateBlock(XRGrabInteractable block)
    {
        ActivateEventArgs args = new();
        args.interactableObject = block;
        block.activated.Invoke(args);
    }

    // ���� �������� ����� �ϳ��� �����Ѵ�.
    private IEnumerator ExecuteBlockCodes(List<XRGrabInteractable> blockList)
    {
        int iterStartIdx = -1, iterNum = 0, curIterNum = 0;

        foreach (ChangeMaterial pointer in pointers)
            pointer.ChangeToDefaultMaterial();

        for (int i = 0; i < blockList.Count; i++)
        {
            Debug.LogWarning($"���� ��� �ε���: {i}");

            if (blockList[i].CompareTag(ITERATION_START_TAG) && iterStartIdx < 0)
            {
                iterStartIdx = i;
                GameObject variableBlock = GetAttachedVariableBlock(blockList[iterStartIdx]);
                iterNum = Convert.ToInt32(variableBlock.GetComponentInChildren<TMP_Text>().text);
                Debug.LogWarning($"�� �ݺ� Ƚ��: {iterNum}");
            }
            else if (blockList[i].CompareTag(ITERATION_END_TAG))
            {
                curIterNum++;
                Debug.LogWarning($"���� �ݺ� Ƚ��: {curIterNum}");

                if (curIterNum >= iterNum)
                    pointers[iterStartIdx].ChangeToDefaultMaterial();
                else
                {
                    i = iterStartIdx;
                    continue;
                }
            }
            pointers[i].ChangeToActivatedMaterial();
            ActivateBlock(blockList[i]);
            yield return new WaitForSeconds(delay);

            if (i != iterStartIdx)
                pointers[i].ChangeToDefaultMaterial();
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