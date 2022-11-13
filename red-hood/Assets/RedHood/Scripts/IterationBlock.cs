using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(BlockActivation))]
public class IterationBlock : MonoBehaviour
{
    internal static readonly Stack<BlockActivation> iterStartBlockStack = new();
    private static readonly Stack<int> iterNumStack = new();

    private const string ITER_START_BLOCK = "IterStartBlock";
    private const string ITER_END_BLOCK = "IterEndBlock";
    private const string VAR_SOCKET = "VariableSocket";

    private BlockActivation blockActivation;
    private BlockActivation nextBlock = null;
    private static bool isFirstBlock = true;

    private PopUpMessage popUpMessage;

    private void Start()
    {
        blockActivation = GetComponent<BlockActivation>();
        popUpMessage = GetComponent<PopUpMessage>();
    }

    private void ResetIteration()
    {
        isFirstBlock = true;

        if (nextBlock != null)
        {
            nextBlock.GetComponentInChildren<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        GetComponentInChildren<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    // 반복 블록 변수 설정
    public void SetIteration()
    {
        if (iterStartBlockStack.Count > 0 &&
            (iterStartBlockStack.Peek() == blockActivation || iterStartBlockStack.Peek() == nextBlock))
        {
            return;
        }

        if (CompareTag(ITER_START_BLOCK))
        {
            if (isFirstBlock)
            {
                iterStartBlockStack.Clear();
                iterNumStack.Clear();
                isFirstBlock = false;
            }
            iterStartBlockStack.Push(blockActivation);
        }
        else if (CompareTag(ITER_END_BLOCK))
        {
            try
            {
                nextBlock = iterStartBlockStack.Peek();
            }
            catch (InvalidOperationException)
            {
                string text = "반복 시작 블록이 존재하지 않아요";
                popUpMessage.ActivateErrorWindow(text);
                ResetIteration();
                return;
            }

            XRSocketInteractor variableSocket = transform.Find(VAR_SOCKET).GetComponent<XRSocketInteractor>();
            IXRSelectInteractable attach = variableSocket.firstInteractableSelected;
            if (attach != null)
            {
                XRGrabInteractable variableBlock = (XRGrabInteractable)attach;
                int iterNum = variableBlock.GetComponent<VariableBlock>().GetInt();
                iterNumStack.Push(iterNum);
            }
            else
            {
                string text = "변수 블록이 존재하지 않아요";
                popUpMessage.ActivateErrorWindow(text);
                ResetIteration();
                return;
            }
        }
    }

    // 반복문 실행
    public void Iterate()
    {
        if (iterNumStack.Count == 0)
        {
            return;
        }

        int iterNum = iterNumStack.Pop();
        iterNumStack.Push(--iterNum);

        // TODO: 변수 블록 숫자 감소

        if (iterNum > 0)
        {
            // 반복 횟수가 남았다면 반복문 다시 실행
            nextBlock.ExecuteBlock();
        }
        else
        {
            // 반복이 끝났다면 반복문 빠져나옴
            _ = iterStartBlockStack.Pop();
            _ = iterNumStack.Pop();
            ResetIteration();
            if (iterStartBlockStack.Count > 0)
            {
                isFirstBlock = false;
            }

            nextBlock = blockActivation.GetNextBlock();
            if (nextBlock != null)
            {
                nextBlock.ExecuteBlock();
            }
        }
    }
}
