using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

// Controller의 Input에 따른 이벤트를 추가한다.
public class OnButtonPress : MonoBehaviour
{
    public InputAction Action = null;
    public UnityEvent OnPress = new UnityEvent();
    public UnityEvent OnRelease = new UnityEvent();

    private void Awake()
    {
        Action.started += Pressed;
        Action.canceled += Released;
    }

    private void OnDestroy()
    {
        Action.started -= Pressed;
        Action.canceled -= Released;
    }

    private void OnEnable()
    {
        Action.Enable();
    }

    private void OnDisable()
    {
        Action.Disable();
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