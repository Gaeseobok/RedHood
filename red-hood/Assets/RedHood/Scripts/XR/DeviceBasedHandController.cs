using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Hand Controller���� �߻��� input�� ���� HandAnimation Ŭ������ action�� Ʈ�����Ѵ�.
[RequireComponent(typeof(XRController))]
public class DeviceBasedHandController : MonoBehaviour
{
    [SerializeField] private HandAnimation hand;
    private XRController controller;

    private void Start()
    {
        controller = GetComponent<XRController>();
    }

    // Trigger�� Grip input�� value�� HandAnimation�� �����Ѵ�.
    private void Update()
    {
        hand.SetGrip(controller.selectInteractionState.value);
        hand.SetTrigger(controller.activateInteractionState.value);
    }
}