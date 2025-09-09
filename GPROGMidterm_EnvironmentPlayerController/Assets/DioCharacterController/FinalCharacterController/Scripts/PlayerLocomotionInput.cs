using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dio.FinalCharacterController
{
    [DefaultExecutionOrder(-2)]
    public class PlayerLocomotionInput : MonoBehaviour, PlayerControls.IPlayerLocomotionMapActions
    {
        [SerializeField] private bool holdToSprint = true;
        public bool SprintToggledOn {  get; private set; }
        //INITIALIZE PLAYERCONTROLS INPUT using GET & SET METHODS (Get Variable w/o Setting Outside of Class)
        public PlayerControls PlayerControls { get; private set; }
        public Vector2 MovementInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public bool JumpPressed { get; private set; }

        //ENABLE PLAYER CONTROLS & PLAYER CONTROLS MAP
        private void OnEnable()
        {
            PlayerControls = new PlayerControls();
            PlayerControls.Enable();

            PlayerControls.PlayerLocomotionMap.Enable();
            PlayerControls.PlayerLocomotionMap.SetCallbacks(this);
        }
        //DISABLE PLAYER CONTROLS & PLAYER CONTROLS MAP WHEN NOT IN USE
        private void OnDisable()
        {
            PlayerControls.PlayerLocomotionMap.Disable();
            PlayerControls.PlayerLocomotionMap.RemoveCallbacks(this);
        }

        private void LateUpdate()
        {
            //TO MAKE JUMPPRESSED FALSE SO ITS ONLY TRUE FOR A DURATION OF A FRAME
            JumpPressed = false;
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            MovementInput = context.ReadValue<Vector2>();
            print(MovementInput);
        }
        
        public void OnLook(InputAction.CallbackContext context)
        {
            LookInput = context.ReadValue<Vector2>();
        }

        public void OnToggleSprint(InputAction.CallbackContext context)
        {
            //IF HOLD SHIFT
            if (context.performed)
            {
                SprintToggledOn = holdToSprint || !SprintToggledOn;
            }
            //CASE FOR RELEASING SHIFT KEY
            else if (context.canceled)
            {
                SprintToggledOn = !holdToSprint && SprintToggledOn;
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            //IF JUMP NOT PERFORMED, RETURN (DONT DO ANYTHING)
            if (!context.performed)
            {
                return;

                JumpPressed = true;
            }
        }
    }
}

