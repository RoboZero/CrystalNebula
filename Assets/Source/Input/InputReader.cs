using Source.Utility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Source.Input
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
    public class InputReader : DescriptionBaseSO, PlayerInputActions.IOSCommandActions
    {
        public event UnityAction<Vector2, bool> PointerPositionEvent = delegate { };
        public event UnityAction InteractPressedEvent = delegate { }; 
        public event UnityAction InteractReleasedEvent = delegate { }; 
    
        private PlayerInputActions playerInputActions;

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
            if(context.canceled)
                InteractReleasedEvent.Invoke();
        }

        public void EnableOSSystemInput()
        {
            playerInputActions.OSCommand.Enable();
        }
    }
}
