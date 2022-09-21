using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// ���Ͽ� ���� �ڵ� ����� ũ�⿡ �°� ������ Attach Transform�� �����Ѵ�.
public class BlockBasedAttachTransform : MonoBehaviour
{
    //[Tooltip("The distance between Pointer and Code Block")]
    //[SerializeField] private float marginLeft = 0.033f;

    private XRSocketInteractor socketInteractor;
    private Vector3 defaultAttachTransform;

    private const float BLOCK_WIDTH = 0.1f;

    private void Start()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();
        defaultAttachTransform = socketInteractor.attachTransform.localPosition;
    }

    public void SetAttachTransform()
    {
        XRGrabInteractable selectedBlock = (XRGrabInteractable)socketInteractor.interactablesSelected[0];
        float selectedBlockWidth = selectedBlock.transform.GetChild(0).Find("Center").localScale.x;
        float x = (selectedBlockWidth - BLOCK_WIDTH) / 2;

        socketInteractor.attachTransform.localPosition = new Vector3(-x, defaultAttachTransform.y, defaultAttachTransform.z);
    }

    public void RestoreAttachTransform()
    {
        socketInteractor.attachTransform.localPosition = defaultAttachTransform;
    }
}