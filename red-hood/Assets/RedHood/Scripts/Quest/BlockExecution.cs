using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System;

// �ڵ� ������ ��ư�� ������ �� ����� �ʱ�ȭ�ϰų� �����Ѵ�. (���� & ��ŸƮ)
public class BlockExecution : MonoBehaviour
{
    public static Coroutine CurrentRoutine { private set; get; } = null;

    [Tooltip("���� ����Ʈ(���ϵ��� ���� ������Ʈ)")]
    [SerializeField] private SocketList socketList;

    [Tooltip("�˸� �޼����� ����� ĵ����")]
    [SerializeField] private Canvas alertCanvas;

    [Tooltip("���� �ڵ� ����� ����� �������� ���� �ð�")]
    [SerializeField] private float ExecuteDelay = 1.0f;

    private SocketListScroll scrollComponent;

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
        scrollComponent = GetComponent<SocketListScroll>();

        pointers = socketList.GetComponentsInChildren<ChangeMaterial>(includeInactive: true);
        answers = socketList.GetComponentsInChildren<AnswerConfirmation>(includeInactive: true);

        errorMessage = alertCanvas.transform.Find(ERROR_MESSAGE).GetComponent<FadeCanvas>();
        failureMessage = alertCanvas.transform.Find(FAILURE_MESSAGE).GetComponent<FadeCanvas>();
        successMessage = alertCanvas.transform.Find(SUCCESS_MESSAGE).GetComponent<FadeCanvas>();
    }

    // ���� ��ư�� �������� ��, ���Ͽ� ������ ��� ����� �����Ѵ�.
    public void OnResetButtonPress()
    {
        scrollComponent.ResetScroll();

        // ��� ��� �����ϱ�
        for (int i = 0; i < socketList.socketNum; i++)
            socketList.DestroyBlocks(i);

        // �ν��Ͻ�ȭ�� ����Ʈ�� �𵨵� ��� �����ϱ�
        GameObject[] questModels = GameObject.FindGameObjectsWithTag(QUEST_MODEL_TAG);

        foreach (GameObject model in questModels)
            Destroy(model);
    }

    // �÷��� ��ư�� �������� ��, ��� ����� �����Ѵ�.
    public void OnStartButtonPress()
    {
        // ��� ���Ͽ� ����� ��� ä������ ���� ��� �˸� �޼��� ���
        if (socketList.IsSocketEmpty())
        {
            errorMessage.SetAlpha(1.0f);
            errorMessage.StartFadeOut();
            return;
        }

        // ��� ��� �����ϱ�
        scrollComponent.ResetScroll();
        StopAllCoroutines();
        CurrentRoutine = StartCoroutine(ExecuteBlockCodes());
    }

    // ����� Activated �̺�Ʈ�� Ȱ��ȭ�Ѵ�.
    private void ActivateBlock(XRGrabInteractable block)
    {
        ActivateEventArgs args = new();
        args.interactableObject = block;
        block.activated.Invoke(args);
    }

    // ���� �������� ����� �ϳ��� �����Ѵ�.
    private IEnumerator ExecuteBlockCodes()
    {
        int iterStartIdx = -1, iterNum = 0, curIterNum = 0;
        bool isClear = true;

        foreach (ChangeMaterial pointer in pointers)
            pointer.ChangeToDefaultMaterial();

        for (int i = 0; i < socketList.socketNum; i++)
        {
            scrollComponent.SetScroll(i);
            XRGrabInteractable block = socketList.SetCurrentBlock(i);

            if (block.CompareTag(ITERATION_START_TAG) && iterStartIdx < 0)
            {
                iterStartIdx = i;
                XRGrabInteractable variableBlock = socketList.GetCurrentVariableBlock(block);
                iterNum = Convert.ToInt32(variableBlock.GetComponentInChildren<TMP_Text>().text);
            }
            else if (block.CompareTag(ITERATION_END_TAG))
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
            ActivateBlock(block);

            if (answers[i].CompareAnswer(block) == false)
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
}