using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class IterationBlock : MonoBehaviour
{
    private static Stack<BlockActivation> iterStartBlockStack = new();

    private const string ITER_START_BLOCK = "IterStartBlock";
    private const string ITER_END_BLOCK = "IterEndBlock";
    private const string VAR_SOCKET = "VariableSocket";

    private BlockActivation blockActivation;
    private BlockActivation nextBlock = null;
    private bool setup = false;
    private int iterNum = -1;

    private void Start()
    {
        blockActivation = GetComponent<BlockActivation>();
    }

    // 반복 블록 변수 설정
    public void SetIteration()
    {
        if (setup)
        {
            return;
        }

        if (blockActivation.CompareTag(ITER_START_BLOCK))
        {
            Debug.Log("반복 시작!!");

            iterStartBlockStack.Push(blockActivation);
        }
        else if (blockActivation.CompareTag(ITER_END_BLOCK))
        {
            try
            {
                nextBlock = iterStartBlockStack.Pop();
            } catch (InvalidOperationException)
            {
                // TODO: 에러 처리(문제 오답)
                Debug.LogWarning("반복 에러: 반복 시작 블록이 존재하지 않음");
                return;
            }
            
            XRSocketInteractor variableSocket = blockActivation.transform.Find(VAR_SOCKET).GetComponent<XRSocketInteractor>();
            IXRSelectInteractable attach = variableSocket.firstInteractableSelected;
            if (attach != null)
            {
                XRGrabInteractable variableBlock = (XRGrabInteractable)attach;
                iterNum = variableBlock.GetComponent<VariableBlock>().GetInt();
                Debug.Log("반복 변수: " + iterNum);
            }
            else
            {
                Debug.LogWarning("반복 에러: 변수 블록이 존재하지 않음");
            }
        }

        setup = true;
    }

    // 반복문 실행
    public void Iterate()
    {
        iterNum--;
        if (iterNum > 0)
        {
            // 반복 횟수가 남았다면 반복문 다시 실행
            nextBlock.ExecuteBlock();
            Debug.Log("반복 변수: " + iterNum);

        }
        else
        {
            Debug.Log("반복 끝!!");
            // 반복이 끝났다면 반복문 빠져나옴
            BlockActivation nextBlock = blockActivation.GetNextBlock();
            if (nextBlock != null)
            {
                nextBlock.ExecuteBlock();
            }

            // TODO: IterStartBlock, IterEndBlock 포인터 끄기
        }
    }

}
