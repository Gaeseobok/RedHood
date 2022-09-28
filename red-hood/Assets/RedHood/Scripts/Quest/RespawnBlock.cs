using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// ������ �ڵ� ���忡�� �ڵ� ����� ������ �� �ش� �ڸ��� ������ �ڵ� ����� �������Ѵ�.
public class RespawnBlock : MonoBehaviour
{
    [SerializeField] private XRGrabInteractable attachBlock;

    private void Awake()
    {
        InstantiateBlock();
    }

    private void InstantiateBlock()
    {
        if (gameObject.activeInHierarchy)
            Instantiate(attachBlock, transform.position, transform.rotation);
    }

    public void Respawn()
    {
        Invoke(nameof(InstantiateBlock), 0.2f);
    }
}