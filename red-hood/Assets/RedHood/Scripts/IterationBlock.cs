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

    private void Start()
    {
        blockActivation = GetComponent<BlockActivation>();
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
                // TODO: 에러 처리(문제 오답)
                Debug.LogWarning("반복 에러: 반복 시작 블록이 존재하지 않음");
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
                Debug.LogWarning("반복 에러: 변수 블록이 존재하지 않음");
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
