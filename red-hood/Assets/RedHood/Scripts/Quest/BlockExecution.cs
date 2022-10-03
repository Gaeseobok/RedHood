using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System;

// 코딩 보드의 버튼을 눌렀을 때 블록을 초기화하거나 실행한다. (리셋 & 스타트)
public class BlockExecution : MonoBehaviour
{
    public static Coroutine CurrentRoutine { private set; get; } = null;

    [Tooltip("소켓 리스트(소켓들의 상위 오브젝트)")]
    [SerializeField] private SocketList socketList;

    [Tooltip("알림 메세지를 출력할 캔버스")]
    [SerializeField] private Canvas alertCanvas;

    [Tooltip("다음 코드 블록이 실행될 때까지의 지연 시간")]
    [SerializeField] private float ExecuteDelay = 1.0f;

    private SocketListScroll scrollComponent;

    // 소켓들의 상태를 표현하는 오브젝트
    private ChangeMaterial[] pointers;

    // 각 소켓의 정답 리스트
    private AnswerConfirmation[] answers;

    // 상황 별 알림 메세지 출력을 위한 변수

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

    // 리셋 버튼이 눌러졌을 때, 소켓에 부착된 모든 블록을 제거한다.
    public void OnResetButtonPress()
    {
        scrollComponent.ResetScroll();

        // 모든 블록 제거하기
        for (int i = 0; i < socketList.socketNum; i++)
            socketList.DestroyBlocks(i);

        // 인스턴스화된 퀘스트용 모델들 모두 제거하기
        GameObject[] questModels = GameObject.FindGameObjectsWithTag(QUEST_MODEL_TAG);

        foreach (GameObject model in questModels)
            Destroy(model);
    }

    // 플레이 버튼이 눌러졌을 때, 모든 블록을 실행한다.
    public void OnStartButtonPress()
    {
        // 모든 소켓에 블록이 모두 채워지지 않은 경우 알림 메세지 출력
        if (socketList.IsSocketEmpty())
        {
            errorMessage.SetAlpha(1.0f);
            errorMessage.StartFadeOut();
            return;
        }

        // 모든 블록 실행하기
        scrollComponent.ResetScroll();
        StopAllCoroutines();
        CurrentRoutine = StartCoroutine(ExecuteBlockCodes());
    }

    // 블록의 Activated 이벤트를 활성화한다.
    private void ActivateBlock(XRGrabInteractable block)
    {
        ActivateEventArgs args = new();
        args.interactableObject = block;
        block.activated.Invoke(args);
    }

    // 일정 간격으로 블록을 하나씩 실행한다.
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

        // 정답 여부에 따라 팝업을 띄운다
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