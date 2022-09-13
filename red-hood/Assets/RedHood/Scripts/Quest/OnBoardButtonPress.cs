using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System;

// 코딩 보드의 버튼을 눌렀을 때 블록을 초기화하거나 실행한다. (리셋 & 스타트)
public class OnBoardButtonPress : MonoBehaviour
{
    public Coroutine CurrentRoutine { private set; get; } = null;

    [Tooltip("소켓들의 상위 오브젝트")]
    [SerializeField] private GameObject socketsObject;

    [Tooltip("알림 메세지를 출력할 캔버스")]
    [SerializeField] private Canvas alertCanvas;

    [Tooltip("다음 코드 블록이 실행될 때까지의 지연 시간")]
    [SerializeField] private float delay = 2.0f;

    // 코드 블록이 위치하는 소켓들
    private XRSocketInteractor[] sockets;

    // 소켓들의 상태를 표현하는 오브젝트
    private ChangeMaterial[] pointers;

    // 상황 별 알림 메세지 출력을 위한 변수
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

    // 소켓에 위치한 모든 블록을 리스트 형태로 리턴한다.
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

    // 블록 내부 변수 소켓(Socket_Variable)에 변수 블록이 존재한다면 해당 변수 블록을 리턴한다.
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

    // 리셋 버튼이 눌러졌을 때, 소켓에 위치한 모든 블록을 제거한다.
    public void PressResetButton()
    {
        // 블록 리스트 가져오기
        List<XRGrabInteractable> blockList = GetAttachedBlockList();
        // 모든 블록 제거하기
        foreach (XRGrabInteractable block in blockList)
        {
            // 변수 블록이 존재한다면 제거하기
            GameObject variableBlock = GetAttachedVariableBlock(block);
            if (variableBlock != null)
                Destroy(variableBlock);
            Destroy(block.gameObject);
        }
    }

    // 블록 내부에 비어있는 변수 소켓(Socket_Variable)이 있는지 계산한다.
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

    // 블록의 Activated 이벤트를 활성화한다.
    private void ActivateBlock(XRGrabInteractable block)
    {
        ActivateEventArgs args = new();
        args.interactableObject = block;
        block.activated.Invoke(args);
    }

    // 일정 간격으로 블록을 하나씩 실행한다.
    private IEnumerator ExecuteBlockCodes(List<XRGrabInteractable> blockList)
    {
        int iterStartIdx = -1, iterNum = 0, curIterNum = 0;

        foreach (ChangeMaterial pointer in pointers)
            pointer.ChangeToDefaultMaterial();

        for (int i = 0; i < blockList.Count; i++)
        {
            Debug.LogWarning($"현재 블록 인덱스: {i}");

            if (blockList[i].CompareTag(ITERATION_START_TAG) && iterStartIdx < 0)
            {
                iterStartIdx = i;
                GameObject variableBlock = GetAttachedVariableBlock(blockList[iterStartIdx]);
                iterNum = Convert.ToInt32(variableBlock.GetComponentInChildren<TMP_Text>().text);
                Debug.LogWarning($"총 반복 횟수: {iterNum}");
            }
            else if (blockList[i].CompareTag(ITERATION_END_TAG))
            {
                curIterNum++;
                Debug.LogWarning($"현재 반복 횟수: {curIterNum}");

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
        // 블록 리스트 가져오기
        List<XRGrabInteractable> blockList = GetAttachedBlockList();

        // 모든 소켓에 블록이 모두 채워지지 않은 경우 알림 메세지 출력
        if (blockList.Count < sockets.Length || IsSocketEmpty(blockList))
        {
            errorMessage.SetAlpha(1.0f);
            errorMessage.StartFadeOut();
            return;
        }

        // 모든 블록 실행하기
        StopAllCoroutines();
        CurrentRoutine = StartCoroutine(ExecuteBlockCodes(blockList));
    }
}