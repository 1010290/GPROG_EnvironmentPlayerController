/*
Overview:  PlayerAnimation Script is for Animator; handles player animations
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dio.FinalCharacterController
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float locomotionBlendSpeed = 4f;
        private PlayerLocomotionInput _playerLocomotionInput;

        //Creating a reference to input X and Y in the Animator
        private static int inputXHash = Animator.StringToHash("inputX");
        private static int inputYHash = Animator.StringToHash("inputY");

        private Vector3 _currentBlendInput = Vector3.zero;

        private void Awake()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
        }

        private void Update()
        {
            UpdateAnimationState();
        }

        private void UpdateAnimationState()
        {

            //Vector2 inputTarget = _playerLocomotionInput.MovementInput * 1.5f : _playerLocomotionInput.MovementInput;
            Vector2 inputTarget = _playerLocomotionInput.MovementInput;
            //FOR SMOOTHER TRANSITIONS BETWEEN ANIMATIONS
            _currentBlendInput = Vector3.Lerp(_currentBlendInput, inputTarget, locomotionBlendSpeed * Time.deltaTime);

            _animator.SetFloat(inputXHash, _currentBlendInput.x);
            _animator.SetFloat(inputYHash, _currentBlendInput.y);
            
        }

    }
}