//STATE MACHINE
using UnityEngine;

namespace Dio.FinalCharacterController
{
    public class PlayerState : MonoBehaviour
    {
        //SerializeField Allows for Viewing in the Editor/Inspector
        [field: SerializeField] public PlayerMovementState CurrentPlayerMovementState { get; private set; } = PlayerMovementState.Idling;

        //GET NEW VALUE AND SET TO CURRENTPLAYERMOVEMENTSTATE & BACK TO PLAYER CPNTROLLER
        public void SetPlayerMovementState(PlayerMovementState playerMovementState)
        {
            CurrentPlayerMovementState = playerMovementState;
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