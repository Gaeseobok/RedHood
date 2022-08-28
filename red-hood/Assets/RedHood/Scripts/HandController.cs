using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// HandController class triggers the actions from the HandAnimation class, when a button is pressed
[RequireComponent(typeof(ActionBasedController))]
public class HandController : MonoBehaviour
{
    private ActionBasedController controller;

    [SerializeField] private HandAnimation hand;

    private void Start()
    {
        controller = GetComponent<ActionBasedController>();
    }

    // Getting the trigger input and the grip input values
    private void Update()
    {
        hand.SetGrip(controller.selectAction.action.ReadValue<float>());
        hand.SetTrigger(controller.activateAction.action.ReadValue<float>());
    }
}