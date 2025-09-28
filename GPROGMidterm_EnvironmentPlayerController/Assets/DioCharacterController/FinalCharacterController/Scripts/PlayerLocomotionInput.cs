/*
Overview:   PlayerLocomotionInput Script is responsible for handling player input for movement, looking and jumping using 
            the Unity Input System
*/
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

        public void OnJump(InputAction.CallbackContext context)
        {
            //if context.performed is not true (spacebar is not being hold), just return (don't do anything, don't do code below)
            if (!context.performed)
            {
                return;
            }

            JumpPressed = true;

        }
    }
}