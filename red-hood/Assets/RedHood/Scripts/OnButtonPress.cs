using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

// Controller의 Input에 따른 이벤트를 추가한다.
public class OnButtonPress : MonoBehaviour
{
    public InputAction action = null;
    public UnityEvent OnPress = new UnityEvent();
    public UnityEvent OnRelease = new UnityEvent();

    private void Awake()
    {
        action.started += Pressed;
        action.canceled += Released;
    }

    private void OnDestroy()
    {
        action.started -= Pressed;
        action.canceled -= Released;
    }

    private void OnEnable()
    {
        action.Enable();
    }

    private void OnDisable()
    {
        action.Disable();
    }

    private void Pressed(InputAction.CallbackContext context)
    {
        OnPress.Invoke();
    }

    private void Released(InputAction.CallbackContext context)
    {
        OnRelease.Invoke();
    }
}