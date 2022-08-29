using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RespawnBlock : MonoBehaviour
{
    //[SerializeField] private XRGrabInteractable attachBlock;
    private XRBaseInteractable block;

    private void Start()
    {
        block = GetComponent<XRSocketInteractor>().startingSelectedInteractable;
    }

    private void InstantiateBlock()
    {
        Instantiate(block, transform.position, transform.rotation);
    }

    public void Respawn()
    {
        Invoke("InstantiateBlock", 0.2f);
    }
}