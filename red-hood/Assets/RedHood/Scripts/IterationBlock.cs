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

    // �ݺ����� ������ �ʱ�ȭ�Ѵ�.
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

    // �ݺ����� �������� �����Ѵ�.
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
                string text = "�ݺ� ���� ����� �������� �ʾƿ�";
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
                string text = "���� ����� �������� �ʾƿ�";
                popUpMessage.ActivateFailureWindow(text);
                popUpMessage.PlayFailureSound();
                ResetIteration();
                return false;
            }
        }
        return true;
    }

    // �ݺ����� �����Ѵ�.
    public void Iterate()
    {
        if (iterNumBlockStack.Count == 0)
        {
            return;
        }

        // ���� ����� ���ڸ� ���ҽ�Ŵ
        VariableBlock iterNumBlock = iterNumBlockStack.Pop();
        int iterNum = iterNumBlock.GetInt() - 1;
        iterNumBlock.SetInt(iterNum);
        iterNumBlockStack.Push(iterNumBlock);

        if (iterNum > 0)
        {
            // �ݺ� Ƚ���� ���Ҵٸ� �ݺ��� �ٽ� ����
            nextBlock.ExecuteBlock();
        }
        else
        {
            // �ݺ��� �����ٸ� �ݺ��� ��������
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

            // ���� ����� ���ڸ� �ٽ� �ʱ� ������ ����
            CurrentRoutine = StartCoroutine(ResetIterNum(iterNumBlock));
        }
    }

    // ���� ����� �����ϱ� �� ActiveDelay��ŭ ��ٸ���.
    private IEnumerator ExecuteNextBlockWithDelay()
    {
        yield return new WaitForSeconds(blockActivation.ActiveDelay);
        nextBlock.ExecuteBlock();
    }

    // ActiveDelay��ŭ ��ٸ� �� ���� ����� ���ڸ� �ʱ� ������ �����Ѵ�.
    private IEnumerator ResetIterNum(VariableBlock iterNumBlock)
    {
        yield return new WaitForSeconds(blockActivation.ActiveDelay);
        iterNumBlock.SetDefaultInt();
    }
}
