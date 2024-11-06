using Source.Utility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Source.Input
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
    public class InputReaderSO : DescriptionBaseSO, PlayerInputActions.IOSCommandActions
    {
        public event UnityAction<Vector2, bool> PointerPositionEvent = delegate { };
        public event UnityAction InteractPressedEvent = delegate { }; 
        public event UnityAction InteractReleasedEvent = delegate { }; 
        public event UnityAction CommandPressedEvent = delegate { }; 
        public event UnityAction CommandCanceledEvent = delegate { }; 
        public event UnityAction InteractCanceledEvent = delegate { };
        public event UnityAction HoldPressedEvent = delegate { }; 
        public event UnityAction HoldReleasedEvent = delegate { }; 
    
        private PlayerInputActions playerInputActions;
        public bool ClickAndDrag { get; set; }

        private void OnEnable()
        {
            if (playerInputActions != null) return;
            
            playerInputActions = new PlayerInputActions();
            playerInputActions.OSCommand.SetCallbacks(this);
            EnableOSSystemInput();
        }

        public void OnPointer(InputAction.CallbackContext context)
        {
            PointerPositionEvent.Invoke(context.ReadValue<Vector2>(), IsDeviceMouse(context));
        }
    
        private bool IsDeviceMouse(InputAction.CallbackContext context) => context.control.device.name == "Mouse";

        public void OnInteract(InputAction.CallbackContext context)
        {
            if(context.performed)
                InteractPressedEvent.Invoke();
            else if(context.canceled)
                InteractReleasedEvent.Invoke();
        }

        public void OnCommand(InputAction.CallbackContext context)
        {
            if(context.performed)
                CommandPressedEvent.Invoke();
            else if(context.canceled)
                CommandCanceledEvent.Invoke();
        }

        public void OnHold(InputAction.CallbackContext context)
        {
            if(context.performed)
                HoldPressedEvent.Invoke();
            else if(context.canceled)
                HoldReleasedEvent.Invoke();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if(context.performed)
                InteractCanceledEvent.Invoke();
        }

        public void EnableOSSystemInput()
        {
            playerInputActions.OSCommand.Enable();
        }
    }
}
