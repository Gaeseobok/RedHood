using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace RedHood
{
    // HandController class triggers the actions from the Hand class, when a button is pressed
    [RequireComponent(typeof(ActionBasedController))]
    public class HandController : MonoBehaviour
    {
        private ActionBasedController controller;

        public Hand hand;

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
}