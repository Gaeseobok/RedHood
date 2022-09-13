using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// 유저가 코딩 보드에서 코드 블록을 가져갈 시 해당 자리에 가져간 코드 블록을 리스폰한다.
public class RespawnBlock : MonoBehaviour
{
    [SerializeField] private XRGrabInteractable attachBlock;

    private void Start()
    {
        InstantiateBlock();
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