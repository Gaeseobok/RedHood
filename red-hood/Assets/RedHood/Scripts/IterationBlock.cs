using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(BlockActivation))]
public class IterationBlock : MonoBehaviour
{
    public Coroutine CurrentRoutine { private set; get; } = null;

    internal static readonly Stack<BlockActivation> iterStartBlockStack = new();
    private static readonly Stack<VariableBlock> iterNumBlockStack = new();

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

    // 반복문의 설정을 초기화한다.
    private void ResetIteration()
    {
        isFirstBlock = true;

        if (nextBlock != null)
        {
            nextBlock.GetComponentInChildren<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            nextBlock = null;
        }
        GetComponentInChildren<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    // 반복문의 변수들을 설정한다.
    public bool SetIteration()
    {
        if (!isFirstBlock && iterStartBlockStack.Count > 0 &&
            (iterStartBlockStack.Peek() == blockActivation || iterStartBlockStack.Peek() == nextBlock))
        {
            return true;
        }

        if (CompareTag(ITER_START_BLOCK))
        {
            if (isFirstBlock)
            {
                iterStartBlockStack.Clear();
                iterNumBlockStack.Clear();
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
                popUpMessage.ActivateFailureWindow(text);
                popUpMessage.PlayFailureSound();
                ResetIteration();
                return false;
            }

            XRSocketInteractor variableSocket = transform.Find(VAR_SOCKET).GetComponent<XRSocketInteractor>();
            IXRSelectInteractable attach = variableSocket.firstInteractableSelected;
            if (attach != null)
            {
                XRGrabInteractable variableBlock = (XRGrabInteractable)attach;
                VariableBlock iterNumBlock = variableBlock.GetComponent<VariableBlock>();
                iterNumBlockStack.Push(iterNumBlock);
            }
            else
            {
                string text = "변수 블록이 존재하지 않아요";
                popUpMessage.ActivateFailureWindow(text);
                popUpMessage.PlayFailureSound();
                ResetIteration();
                return false;
            }
        }
        return true;
    }

    // 반복문을 실행한다.
    public void Iterate()
    {
        if (iterNumBlockStack.Count == 0)
        {
            return;
        }

        // 변수 블록의 숫자를 감소시킴
        VariableBlock iterNumBlock = iterNumBlockStack.Pop();
        int iterNum = iterNumBlock.GetInt() - 1;
        iterNumBlock.SetInt(iterNum);
        iterNumBlockStack.Push(iterNumBlock);

        if (iterNum > 0)
        {
            // 반복 횟수가 남았다면 반복문 다시 실행
            nextBlock.ExecuteBlock();
        }
        else
        {
            // 반복이 끝났다면 반복문 빠져나옴
            _ = iterStartBlockStack.Pop();
            _ = iterNumBlockStack.Pop();
            ResetIteration();
            if (iterStartBlockStack.Count > 0)
            {
                isFirstBlock = false;
            }

            nextBlock = blockActivation.GetNextBlock();
            if (nextBlock != null)
            {
                StopAllCoroutines();
                CurrentRoutine = StartCoroutine(ExecuteNextBlockWithDelay());
            }

            // 변수 블록의 숫자를 다시 초기 값으로 설정
            CurrentRoutine = StartCoroutine(ResetIterNum(iterNumBlock));
        }
    }

    // 다음 블록을 실행하기 전 ActiveDelay만큼 기다린다.
    private IEnumerator ExecuteNextBlockWithDelay()
    {
        yield return new WaitForSeconds(blockActivation.ActiveDelay);
        nextBlock.ExecuteBlock();
    }

    // ActiveDelay만큼 기다린 후 변수 블록의 숫자를 초기 값으로 설정한다.
    private IEnumerator ResetIterNum(VariableBlock iterNumBlock)
    {
        yield return new WaitForSeconds(blockActivation.ActiveDelay);
        iterNumBlock.SetDefaultInt();
    }
}
