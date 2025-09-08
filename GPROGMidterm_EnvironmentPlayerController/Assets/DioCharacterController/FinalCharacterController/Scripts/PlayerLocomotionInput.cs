using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dio.FinalCharacterController
{
    [DefaultExecutionOrder(-2)]
    public class PlayerLocomotionInput : MonoBehaviour, PlayerControls.IPlayerLocomotionMapActions
    {
        //INITIALIZE PLAYERCONTROLS INPUT using GET & SET METHODS (Get Variable w/o Setting Outside of Class)
        public PlayerControls PlayerControls { get; private set; }
        public Vector2 MovementInput { get; private set; }
        public Vector2 LookInput { get; private set; }

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

        public void OnMovement(InputAction.CallbackContext context)
        {
            MovementInput = context.ReadValue<Vector2>();
            print(MovementInput);
        }
        public void OnLook(InputAction.CallbackContext context)
        {
            LookInput = context.ReadValue<Vector2>();
        }
    }
}

