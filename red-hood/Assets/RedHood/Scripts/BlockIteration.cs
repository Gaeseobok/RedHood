using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BlockIteration : MonoBehaviour
{
    private static Stack<BlockActivation> iterStartBlockStack = new();

    private const string ITER_START_TAG = "IterStartBlock";
    private const string ITER_END_TAG = "IterEndBlock";
    private const string VAR_SOCKET = "VariableSocket";

    private BlockActivation block;
    private BlockActivation nextBlock = null;
    private bool setup = false;
    private int iterNum = -1;

    private void Start()
    {
        block = GetComponent<BlockActivation>();
    }

    // 반복 블록 변수 설정
    public void SetIteration(BlockActivation block)
    {
        if (setup)
        {
            return;
        }

        if (block.CompareTag(ITER_START_TAG))
        {
            Debug.Log("반복 시작!!");

            iterStartBlockStack.Push(block);
        }
        else if (block.CompareTag(ITER_END_TAG))
        {
            try
            {
                nextBlock = iterStartBlockStack.Pop();
            } catch (InvalidOperationException)
            {
                PrintErrorMsg();
            }
            
            XRSocketInteractor variableSocket = block.transform.Find(VAR_SOCKET).GetComponent<XRSocketInteractor>();
            IXRSelectInteractable attach = variableSocket.firstInteractableSelected;
            if (attach == null)
            {
                PrintErrorMsg();
            }
            else
            {
                XRGrabInteractable variableBlock = (XRGrabInteractable)attach;
                iterNum = Convert.ToInt32(variableBlock.GetComponentInChildren<TMP_Text>().text);
                Debug.Log("반복 변수: " + iterNum);
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
            // 반복이 끝났다면 반복문 빠져나옴
            //block.GetNextBlock().ExecuteBlock();
            Debug.Log("반복 끝!!");
        }
    }

    private void PrintErrorMsg()
    {
        // TODO: 에러 처리(문제 오답)
        Debug.LogWarning("반복 에러!!");
    }
}
