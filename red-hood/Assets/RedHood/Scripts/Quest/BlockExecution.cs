using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System;

// �ڵ� ������ ��ư�� ������ �� ����� �ʱ�ȭ�ϰų� �����Ѵ�. (���� & ��ŸƮ)
public class BlockExecution : AttachedBlock
{
    public static Coroutine CurrentRoutine { private set; get; } = null;

    [Tooltip("�˸� �޼����� ����� ĵ����")]
    [SerializeField] private Canvas alertCanvas;

    [Tooltip("���� �ڵ� ����� ����� �������� ���� �ð�")]
    [SerializeField] private float ExecuteDelay = 1.0f;

    // �ڵ� ����� ��ġ�ϴ� ���ϵ�
    private XRSocketInteractor[] sockets;

    // ���ϵ��� ���¸� ǥ���ϴ� ������Ʈ
    private ChangeMaterial[] pointers;

    // �� ������ ���� ����Ʈ
    private AnswerConfirmation[] answers;

    // ��Ȳ �� �˸� �޼��� ����� ���� ����

    private FadeCanvas errorMessage;
    private FadeCanvas failureMessage;
    private FadeCanvas successMessage;

    private const string ERROR_MESSAGE = "Error Message";
    private const string FAILURE_MESSAGE = "Failure Message";
    private const string SUCCESS_MESSAGE = "Success Message";

    private const string ITERATION_START_TAG = "IterationStart";
    private const string ITERATION_END_TAG = "IterationEnd";
    private const string QUEST_MODEL_TAG = "QuestModel";

    private void Start()
    {
        sockets = socketList.GetComponentsInChildren<XRSocketInteractor>();
        pointers = socketList.GetComponentsInChildren<ChangeMaterial>();
        answers = socketList.GetComponentsInChildren<AnswerConfirmation>();

        errorMessage = alertCanvas.transform.Find(ERROR_MESSAGE).GetComponent<FadeCanvas>();
        failureMessage = alertCanvas.transform.Find(FAILURE_MESSAGE).GetComponent<FadeCanvas>();
        successMessage = alertCanvas.transform.Find(SUCCESS_MESSAGE).GetComponent<FadeCanvas>();
    }

    // ���� ��ư�� �������� ��, ���Ͽ� ������ ��� ����� �����Ѵ�.
    public void OnResetButtonPress()
    {
        // ��� ����Ʈ ��������
        List<XRGrabInteractable> blockList = GetAttachedBlockList(sockets);
        GameObject[] questModels = GameObject.FindGameObjectsWithTag(QUEST_MODEL_TAG);

        // ��� ��� �����ϱ�
        foreach (XRGrabInteractable block in blockList)
        {
            // ���� ����� �����Ѵٸ� �����ϱ�
            XRGrabInteractable variableBlock = GetAttachedVariableBlock(block);
            if (variableBlock != null)
                Destroy(variableBlock.gameObject);
            Destroy(block.gameObject);
        }

        // �ν��Ͻ�ȭ�� ����Ʈ�� �𵨵� ��� �����ϱ�
        foreach (GameObject model in questModels)
        {
            Destroy(model);
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
        bool isClear = true;

        foreach (ChangeMaterial pointer in pointers)
            pointer.ChangeToDefaultMaterial();

        for (int i = 0; i < blockList.Count; i++)
        {
            if (blockList[i].CompareTag(ITERATION_START_TAG) && iterStartIdx < 0)
            {
                iterStartIdx = i;
                XRGrabInteractable variableBlock = GetAttachedVariableBlock(blockList[iterStartIdx]);
                iterNum = Convert.ToInt32(variableBlock.GetComponentInChildren<TMP_Text>().text);
            }
            else if (blockList[i].CompareTag(ITERATION_END_TAG))
            {
                curIterNum++;

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

            if (answers[i].CompareAnswer(blockList[i]) == false)
                isClear = false;

            yield return new WaitForSeconds(ExecuteDelay);

            if (i != iterStartIdx)
                pointers[i].ChangeToDefaultMaterial();
        }

        // ���� ���ο� ���� �˾��� ����
        if (isClear)
        {
            successMessage.SetAlpha(1.0f);
            successMessage.StartFadeOut();
        }
        else
        {
            failureMessage.SetAlpha(1.0f);
            failureMessage.StartFadeOut();
        }
    }

    public void OnStartButtonPress()
    {
        // ��� ����Ʈ ��������
        List<XRGrabInteractable> blockList = GetAttachedBlockList(sockets);

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