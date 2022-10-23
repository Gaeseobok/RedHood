using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

using static UnityEngine.Random;


public class QuestManager : MonoBehaviour
{
    public Coroutine CurrentRoutine { private set; get; } = null;

    [Tooltip("소켓 리스트(소켓들의 상위 오브젝트)")]
    [SerializeField] private SocketList socketList;

    [Tooltip("알림 메세지를 출력할 캔버스")]
    [SerializeField] private Canvas alertCanvas;

    [Tooltip("다음 코드 블록이 실행될 때까지의 지연 시간")]
    [SerializeField] private float delay = 2.0f;

    [Tooltip("Flower Indicator Particles")]
    [SerializeField] private GameObject particleEffect;

    [Tooltip("flower model")]
    [SerializeField] private GameObject [] flowerModel;

    // position of flower instantiation
    public Vector3 flowerPosition;
    // rotation of flower instantiation
    public Vector3 flowerRotation;
    private GameObject instFlower;
    private Rigidbody instFlowerRigidbody;
    // power to move instantiated flower
    public float power;
    // number of flower instantiation
    public int flowerNum;

    private SocketListScroll scrollComponent;

    // 소켓들의 상태를 표현하는 오브젝트
    private ChangeMaterial[] pointers;

    // ParticleSystem to indicate selected flowerbed
    ParticleSystem [] particles;

    // 상황 별 알림 메세지 출력을 위한 변수
    private FadeCanvas errorMessage;
    private FadeCanvas failureMessage;
    private FadeCanvas successMessage;

    private const string ERROR_MESSAGE = "Error Message";
    private const string FAILURE_MESSAGE = "Failure Message";
    private const string SUCCESS_MESSAGE = "Success Message";

    private const string IF_TAG = "If";
    private const string YELLOW_TAG = "Untagged";
    private const string VIOLET_TAG = "QuestModel";
    private const string LEFT_TAG = "Left";
    private const string RIGHT_TAG = "Right";

    private void Start()
    {
        scrollComponent = GetComponent<SocketListScroll>();
        pointers = socketList.GetComponentsInChildren<ChangeMaterial>(includeInactive: true);

        // particle system off
        particles = particleEffect.GetComponentsInChildren<ParticleSystem>();
        foreach(ParticleSystem particle in particles)
            particle.Stop();

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

        foreach (ChangeMaterial pointer in pointers)
            pointer.ChangeToDefaultMaterial();

        // 모든 블록 실행하기
        //scrollComponent.ResetScroll();
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

    private void DeactivateBlock(XRGrabInteractable block)
    {
        DeactivateEventArgs args = new();
        args.interactableObject = block;
        block.deactivated.Invoke(args);
    }

    private void SelectBlock(XRGrabInteractable block)
    {
        SelectEnterEventArgs args = new();
        args.interactableObject = block;
        block.selectEntered.Invoke(args);
    }

    private GameObject AddFlower()
    {
        int r = Range(0,flowerModel.Length);
        instFlower = Instantiate(flowerModel[r], flowerPosition, Quaternion.Euler(flowerRotation));
        instFlower.tag = flowerModel[r].tag;
        return instFlower;
    }

    // 일정 간격으로 블록을 하나씩 실행한다.
    private IEnumerator ExecuteBlockCodes()
    {
        bool isClear = true;
        bool isClassified = false;

        pointers[0].ChangeToActivatedMaterial();
        yield return new WaitForSeconds(delay/2);
        pointers[0].ChangeToDefaultMaterial();

        for(int j = 0; j < flowerNum && isClear; j++)
        {
            instFlower = AddFlower();
            instFlowerRigidbody = instFlower.GetComponent<Rigidbody>();
            isClassified = false;
            
            for(int i = 1; i < socketList.socketNum -1 && isClear; i+=2)
            {
                if(isClassified == true)
                    break;
                
                // scrollComponent.SetScroll(i);
                XRGrabInteractable conBlock = socketList.SetCurrentBlock(i);
                XRGrabInteractable exeBlock = socketList.SetCurrentBlock(i+1);

                if(conBlock.CompareTag(IF_TAG))
                {
                    XRGrabInteractable colorBlock = socketList.GetCurrentVariableBlock(conBlock);
                    if(colorBlock != null)
                    {
                        if (colorBlock.CompareTag(instFlower.tag))
                        {
                            pointers[i].ChangeToActivatedMaterial();
                            SelectBlock(colorBlock);
                            yield return new WaitForSeconds(delay);
                            pointers[i].ChangeToDefaultMaterial();
                            yield return new WaitForSeconds(delay/4);

                            pointers[i+1].ChangeToActivatedMaterial();
                            yield return new WaitForSeconds(delay/2);                        
                            ActivateBlock(exeBlock);
                            yield return new WaitForSeconds(delay/2);
                            if(exeBlock.CompareTag(LEFT_TAG))
                                instFlowerRigidbody.AddForce(Vector3.left * power);
                            else if(exeBlock.CompareTag(RIGHT_TAG))
                                instFlowerRigidbody.AddForce(Vector3.right * power);
                            yield return new WaitForSeconds(delay); // 있어야 실행됨
                            DeactivateBlock(exeBlock);
                            pointers[i+1].ChangeToDefaultMaterial();

                            isClassified = true;
                            isClear = colorBlock.CompareTag(exeBlock.tag) || (exeBlock.CompareTag(LEFT_TAG) && colorBlock.CompareTag(YELLOW_TAG)) 
                                                                            || (exeBlock.CompareTag(RIGHT_TAG) && colorBlock.CompareTag(VIOLET_TAG));
                        }
                    }
                    else
                    {
                        pointers[i].ChangeToActivatedMaterial();
                        yield return new WaitForSeconds(delay);
                        pointers[i].ChangeToDefaultMaterial();
                        yield return new WaitForSeconds(delay/4);

                        pointers[i+1].ChangeToActivatedMaterial(); 
                        yield return new WaitForSeconds(delay/2);                       
                        ActivateBlock(exeBlock);
                        yield return new WaitForSeconds(delay/2);
                        if(exeBlock.CompareTag(LEFT_TAG))
                            instFlowerRigidbody.AddForce(Vector3.left * power);
                        else if(exeBlock.CompareTag(RIGHT_TAG))
                            instFlowerRigidbody.AddForce(Vector3.right * power);
                        yield return new WaitForSeconds(delay); // 있어야 실행됨
                        DeactivateBlock(exeBlock);
                        pointers[i+1].ChangeToDefaultMaterial();

                        isClassified = true;
                        isClear = instFlower.CompareTag(exeBlock.tag) || (exeBlock.CompareTag(LEFT_TAG) && instFlower.CompareTag(YELLOW_TAG))
                                                                        || (exeBlock.CompareTag(RIGHT_TAG) && instFlower.CompareTag(VIOLET_TAG));
                    }
                }
                else
                {
                    isClear = false;
                }
                yield return new WaitForSeconds(delay);
            }
            Destroy(instFlower);
        }
        pointers[socketList.socketNum-1].ChangeToActivatedMaterial();
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