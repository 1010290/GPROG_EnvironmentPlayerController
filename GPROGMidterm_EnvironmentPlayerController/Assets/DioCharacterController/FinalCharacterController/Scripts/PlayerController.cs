using System.Collections;
using System.Collections.Generic;
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
        public float drag = 20f;

        [Header("Camera Settings")]
        public float lookSenseH = 0.1f;
        public float lookSenseV = 0.1f;
        public float lookLimitV = 89f;
        private PlayerLocomotionInput _playerLocomotionInput;
        private Vector2 _cameraRotation = Vector2.zero;
        private Vector2 _playerTargetRotation = Vector2.zero;

        private void Awake()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
        }
        private void Update()
        {
            //FOR MOVEMENT TO BE BASED ON CAMERA DIRECTION
            Vector3 cameraForwardXZ = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
            Vector3 cameraRightXZ = new Vector3(_playerCamera.transform.right.x, 0f, _playerCamera.transform.right.z).normalized;
            Vector3 movementDirection = cameraRightXZ * _playerLocomotionInput.MovementInput.x + cameraForwardXZ * _playerLocomotionInput.MovementInput.y;

            //HOW MUCH PLAYER MOVES IN THIS FRAME
            Vector3 movementDelta = movementDirection * runAcceleration * Time.deltaTime;
            Vector3 newVelocity = _characterController.velocity + movementDelta;

            //ADDS DRAG TO THE PLAYER
            Vector3 currentDrag = newVelocity.normalized * drag * Time.deltaTime;
            //STOPS DRAG FROM SETTING PLAYER BACKWARDS IF VELOCITY IS TOO LOW
            newVelocity = (newVelocity.magnitude > drag * Time.deltaTime) ? newVelocity - currentDrag : Vector3.zero;
            //PREVENTS GOING
            newVelocity = Vector3.ClampMagnitude(newVelocity, runSpeed);


            //TO PHYSICALLY MOVE PLAYER
            _characterController.Move(newVelocity * Time.deltaTime);
        }

        private void LateUpdate()
        {
            _cameraRotation.x += lookSenseH * _playerLocomotionInput.LookInput.x;
            _cameraRotation.y = Mathf.Clamp(_cameraRotation.y - lookSenseV * _playerLocomotionInput.LookInput.y, -lookLimitV, lookLimitV);

            _playerTargetRotation.x += transform.eulerAngles.x + lookSenseH * _playerLocomotionInput.LookInput.x;
            transform.rotation = Quaternion.Euler(0f, _playerTargetRotation.x, 0f);

            _playerCamera.transform.rotation = Quaternion.Euler(_cameraRotation.y, _cameraRotation.x, 0f);
        }

    }
}

