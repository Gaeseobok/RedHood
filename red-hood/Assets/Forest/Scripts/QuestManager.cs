using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System;

public class QuestManager : MonoBehaviour
{
    public Coroutine CurrentRoutine { private set; get; } = null;

    [Tooltip("소켓들의 상위 오브젝트")]
    [SerializeField] private GameObject socketsObject;

    [Tooltip("알림 메세지를 출력할 캔버스")]
    [SerializeField] private Canvas alertCanvas;

    [Tooltip("다음 코드 블록이 실행될 때까지의 지연 시간")]
    [SerializeField] private float delay = 2.0f;

    [Tooltip("Flower Indicators")]
    [SerializeField] private GameObject particleEffect;

    // 코드 블록이 위치하는 소켓들
    private XRSocketInteractor[] sockets;

    // 소켓들의 상태를 표현하는 오브젝트
    private ChangeMaterial[] pointers;

    // ParticleSystem to indicate selected flowerbed
    ParticleSystem [] particles;

    // 상황 별 알림 메세지 출력을 위한 변수
    private FadeCanvas blankMessage;
    private FadeCanvas failureMessage;
    private FadeCanvas successMessage;
    private FadeCanvas syntaxMessage;

    //private GameObject failureMessage;
    //private GameObject successMessage;

    private const string BLANK_MESSAGE = "Blank Error Message";
    private const string FAILURE_MESSAGE = "Failure Message";
    private const string SUCCESS_MESSAGE = "Success Message";
    private const string SYNTAX_MESSAGE = "Syntax Error Message";


    private const string CONDITIONAL_TAG = "Conditional";
    // private const string EXECUTIONAL_TAG = "Executional";
    private const string PICK_TAG = "Q3_Pick";
    private const string PASS_TAG = "Q3_Pass";

    private void Start()
    {
        sockets = socketsObject.GetComponentsInChildren<XRSocketInteractor>();
        pointers = socketsObject.GetComponentsInChildren<ChangeMaterial>();

        // particle system off
        particles = particleEffect.GetComponentsInChildren<ParticleSystem>();
        foreach(ParticleSystem particle in particles)
        {
            // var emission = particle.emission;
            // emission.enabled = false;
            particle.Stop();
        }
        
        blankMessage = alertCanvas.transform.Find(BLANK_MESSAGE).GetComponent<FadeCanvas>();
        failureMessage = alertCanvas.transform.Find(FAILURE_MESSAGE).GetComponent<FadeCanvas>();
        successMessage = alertCanvas.transform.Find(SUCCESS_MESSAGE).GetComponent<FadeCanvas>();
        syntaxMessage = alertCanvas.transform.Find(SYNTAX_MESSAGE).GetComponent<FadeCanvas>();
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

    // 변수 블록 XRGrabInteractable 형태로 리턴
    private XRGrabInteractable GetAttachedXRGrabInteractable(XRGrabInteractable block)
    {
        XRSocketInteractor variableSocket = block.GetComponentInChildren<XRSocketInteractor>();
        if (variableSocket != null)
        {
            List<IXRSelectInteractable> variableBlocks = variableSocket.interactablesSelected;
            if (variableBlocks.Count > 0)
            {
                XRGrabInteractable variableBlock = (XRGrabInteractable)variableSocket.interactablesSelected[0];
                return variableBlock;
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

        //remove failure canvas
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

    private void SelectBlock(XRGrabInteractable block)
    {
        SelectEnterEventArgs args = new();
        args.interactableObject = block;
        block.selectEntered.Invoke(args);
    }

    // 일정 간격으로 블록을 하나씩 실행한다.
    private IEnumerator ExecuteBlockCodes(List<XRGrabInteractable> blockList)
    {
        Boolean flag = true;

        foreach (ChangeMaterial pointer in pointers)
            pointer.ChangeToDefaultMaterial();
    

        for (int i = 0; i < blockList.Count; i++)
        {
            Debug.LogWarning($"현재 블록 인덱스: {i}");
            pointers[i].ChangeToActivatedMaterial();

            int temp = i%2;

            if(temp == 0)
            {
                if (blockList[i].CompareTag(CONDITIONAL_TAG))
                {
                    // 조건블록일 때
                    XRGrabInteractable colorBlock = GetAttachedXRGrabInteractable(blockList[i]);
                    SelectBlock(colorBlock); //색 변화                        
                }
                else if (blockList[i].CompareTag("Untagged"))
                {
                    continue;
                }
                else
                {
                    //에러창 뜨게하기
                    flag = false;
                    Debug.LogWarning("조건 블록 아님");
                    syntaxMessage.SetAlpha(1.0f);
                }
            }
            else
            {
                XRGrabInteractable variableBlock = GetAttachedXRGrabInteractable(blockList[i-1]);
                if (blockList[i].CompareTag(PICK_TAG))
                {
                    Debug.LogWarning("담기 블록");
                    ActivateBlock(variableBlock);
                    if(variableBlock.CompareTag(PICK_TAG))
                    {
                        flag = true;
                        Debug.LogWarning("주어진 꽃");
                        
                    }
                    else
                    {
                        flag = false;
                        Debug.LogWarning("주어진 꽃 아님");
                    }
                }
                else if(blockList[i].CompareTag(CONDITIONAL_TAG))
                {
                    flag = false;
                    Debug.LogWarning("실행 블록 아님");
                }
            }
            yield return new WaitForSeconds(delay);
            pointers[i].ChangeToDefaultMaterial();

            if(!flag)
            {
                failureMessage.SetAlpha(1.0f);
                break;
            }
        }

        if(flag)
        {
            successMessage.SetAlpha(1.0f);
        }
    }

    public void PressStartButton()
    {

        // 블록 리스트 가져오기
        List<XRGrabInteractable> blockList = GetAttachedBlockList();

        // 모든 소켓에 블록이 모두 채워지지 않은 경우 알림 메세지 출력
        if (blockList.Count < sockets.Length || IsSocketEmpty(blockList))
        {
            blankMessage.SetAlpha(1.0f);
            blankMessage.StartFadeOut();
            return;
        }

        // 모든 블록 실행하기
        StopAllCoroutines();
        CurrentRoutine = StartCoroutine(ExecuteBlockCodes(blockList));
    }
}
