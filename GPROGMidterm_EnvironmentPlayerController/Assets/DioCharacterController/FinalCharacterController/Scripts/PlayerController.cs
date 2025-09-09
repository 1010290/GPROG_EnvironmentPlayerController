using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dio.FinalCharacterController
{
    [DefaultExecutionOrder(-1)]
    public class PlayerController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Camera _playerCamera;

        [Header("Base Movement")]
        public float runAcceleration = 50f;
        public float runSpeed = 4f;
        public float sprintAccelerataion = 50f;
        public float sprintSpeed = 7f;
        public float drag = 20f;
        public float movingThreshold = 0.01f;
        public float gravity = 25f;
        public float jumpSpeed = 1.0f;

        [Header("Camera Settings")]
        public float lookSenseH = 0.1f;
        public float lookSenseV = 0.1f;
        public float lookLimitV = 89f;

        private PlayerLocomotionInput _playerLocomotionInput;
        private PlayerState _playerState;
        private Vector2 _cameraRotation = Vector2.zero;
        private Vector2 _playerTargetRotation = Vector2.zero;

        private void Awake()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
            _playerState = GetComponent<PlayerState>();
        }
        private void Update()
        {
            UpdateMovementState();
            HandleLateralMovement();
        }

        private void UpdateMovementState()
        {
            //CHECK IF THERE'S LATERAL MOVEMENT INPUT
            bool isMovementInput = _playerLocomotionInput.MovementInput != Vector2.zero;
            //CHECK IF MOVING LATERALLY
            bool isMovingLaterally = IsMovingLaterally();
            bool isSprinting = _playerLocomotionInput.SprintToggledOn && isMovingLaterally;

            //If isMovingLaterally || isMovementInput then Player is in Running State, else in Idling State
            PlayerMovementState lateralState = isSprinting ? PlayerMovementState.Sprinting :
                                                isMovingLaterally || isMovementInput ? PlayerMovementState.Running : PlayerMovementState.Idling;
            _playerState.SetPlayerMovementState(lateralState);
        }

        private void HandleLateralMovement()
        {
            //QUICK REFERENCES FOR CURRENT STATE
            bool isSprinting = _playerState.CurrentPlayerMovementState == PlayerMovementState.Sprinting;
            //STATE DEPENDENT ACCELERATION AND SPEED
            //IF SPRINTING, SET TO SPRINTACCELERATION; OTHERWISE, SET TO RUNACCELERATION
            float lateralAcceleration = isSprinting ? sprintAccelerataion : runAcceleration;
            //IF SPRINTING, THIS IS THE SPRINTSPD; ELSE, SET ITS THE RUN SPD
            float clampLateralMagnitude = isSprinting ? sprintSpeed : runSpeed;

            //FOR MOVEMENT TO BE BASED ON CAMERA DIRECTION
            Vector3 cameraForwardXZ = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
            Vector3 cameraRightXZ = new Vector3(_playerCamera.transform.right.x, 0f, _playerCamera.transform.right.z).normalized;
            Vector3 movementDirection = cameraRightXZ * _playerLocomotionInput.MovementInput.x + cameraForwardXZ * _playerLocomotionInput.MovementInput.y;

            //HOW MUCH PLAYER MOVES IN THIS FRAME
            Vector3 movementDelta = movementDirection * lateralAcceleration;
            Vector3 newVelocity = _characterController.velocity + movementDelta;

            //ADDS DRAG TO THE PLAYER
            Vector3 currentDrag = newVelocity.normalized * drag * Time.deltaTime;
            //STOPS DRAG FROM SETTING PLAYER BACKWARDS IF VELOCITY IS TOO LOW
            newVelocity = (newVelocity.magnitude > drag * Time.deltaTime) ? newVelocity - currentDrag : Vector3.zero;
            //PREVENTS GOING BEYOND RUNSPEED WHILE ACCELERATING
            newVelocity = Vector3.ClampMagnitude(newVelocity, clampLateralMagnitude);


            //TO PHYSICALLY MOVE PLAYER
            _characterController.Move(newVelocity * Time.deltaTime);
        }

        private void LateUpdate()
        {
            //CAMERA X ROTATION
            _cameraRotation.x += lookSenseH * _playerLocomotionInput.LookInput.x;
            //CAMERA Y ROTATION
            _cameraRotation.y = Mathf.Clamp(_cameraRotation.y - lookSenseV * _playerLocomotionInput.LookInput.y, -lookLimitV, lookLimitV);

            _playerTargetRotation.x += transform.eulerAngles.x + lookSenseH * _playerLocomotionInput.LookInput.x;
            transform.rotation = Quaternion.Euler(0f, _playerTargetRotation.x, 0f);

            _playerCamera.transform.rotation = Quaternion.Euler(_cameraRotation.y, _cameraRotation.x, 0f);
        }

        private bool IsMovingLaterally()
        {
            //GET LATERALVELOCITY AS XY COMPONENT OF PLAYERCONTROLLER'S VELOCITY
            Vector3 lateralVelocity = new Vector3(_characterController.velocity.x, 0f, _characterController.velocity.z);

            return lateralVelocity.magnitude > movingThreshold;
        }
    }
}

