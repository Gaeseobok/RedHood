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

    // �ݺ� ��� ���� ����
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
                // TODO: ���� ó��(���� ����)
                Debug.LogWarning("�ݺ� ����: �ݺ� ���� ����� �������� ����");
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
                Debug.LogWarning("�ݺ� ����: ���� ����� �������� ����");
                ResetIteration();
                return;
            }
        }
    }

    // �ݺ��� ����
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
            // �ݺ� Ƚ���� ���Ҵٸ� �ݺ��� �ٽ� ����
            nextBlock.ExecuteBlock();
        }
        else
        {
            // �ݺ��� �����ٸ� �ݺ��� ��������
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
