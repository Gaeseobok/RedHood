using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Hand Controller에서 발생한 input에 따라 HandAnimation 클래스의 action을 트리거한다.
[RequireComponent(typeof(XRController))]
public class DeviceBasedHandController : MonoBehaviour
{
    [SerializeField] private HandAnimation hand;
    private XRController controller;

    private void Start()
    {
        controller = GetComponent<XRController>();
    }

    // Trigger와 Grip input의 value를 HandAnimation에 전달한다.
    private void Update()
    {
        hand.SetGrip(controller.selectInteractionState.value);
        hand.SetTrigger(controller.activateInteractionState.value);
    }
}