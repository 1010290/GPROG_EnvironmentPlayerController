/*
OVERVIEW: PlayerState Script is the State Machine, which determines which state the player is in (Ex. Idling, Walking, Running, etc.)
*/
using UnityEngine;

namespace Dio.FinalCharacterController
{
    public class PlayerState : MonoBehaviour
    {
        //SerializeField Allows for Viewing in the Editor/Inspector
        [field: SerializeField] public PlayerMovementState CurrentPlayerMovementState { get; private set; } = PlayerMovementState.Idling;

        //GET NEW VALUE AND SET TO CURRENTPLAYERMOVEMENTSTATE & BACK TO PLAYER CONTROLLER
        public void SetPlayerMovementState(PlayerMovementState playerMovementState)
        {
            CurrentPlayerMovementState = playerMovementState;
        }

        //HELPER METHOD USED FOR HandleVerticalMovement() Method in PLAYERCONTROLLER
        public bool InGroundedState()
        {
            //RETURNS TRUE IF PLAYER IS IN IDLING/WALKING/RUNNING/SPRINTING STATE
            return CurrentPlayerMovementState == PlayerMovementState.Idling ||
                    CurrentPlayerMovementState == PlayerMovementState.Walking ||
                    CurrentPlayerMovementState == PlayerMovementState.Running ||
                    CurrentPlayerMovementState == PlayerMovementState.Sprinting;
        }
    }
    
    public enum PlayerMovementState
    {
        Idling = 0,
        Walking = 1,
        Running = 2,
        Sprinting = 3,
        Jumping = 4,
        Falling = 5,
        Strafing = 6,
    }
}