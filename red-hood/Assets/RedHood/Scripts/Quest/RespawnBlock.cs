using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// ������ �ڵ� ���忡�� �ڵ� ����� ������ �� �ش� �ڸ��� ������ �ڵ� ����� �������Ѵ�.
public class RespawnBlock : MonoBehaviour
{
    [SerializeField] private XRGrabInteractable attachBlock;

    private void Start()
    {
        //InstantiateBlock();
    }

    private void InstantiateBlock()
    {
        Instantiate(attachBlock, transform.position, transform.rotation);
    }

    public void Respawn()
    {
        Invoke("InstantiateBlock", 0.2f);
    }
}