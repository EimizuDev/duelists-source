using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private BotController _botController;

    public enum State
    {
        Idle,
        OverheadAttack,
        OverheadBlock,
        MiddleAttack,
        MiddleBlock,
        LowAttack,
        LowBlock,
        Parried
    }

    static public State state;

    [HideInInspector] public bool isRecovering = false;

    private void SwitchState(State newState)
    {
        state = newState;
        _botController.DoAction();
    }

    private void Start()
    {
        InputAction _overheadBlockAction = InputSystem.actions.FindAction("OverheadBlock");

        _overheadBlockAction.started += context =>
        {
            if (state == State.Idle && GameState.fightBegan)
            {
                animator.ResetTrigger("Overhead_Attack");
                animator.ResetTrigger("ShouldIdle");
                animator.SetTrigger("Overhead_Block");
                SwitchState(State.OverheadBlock);
            }
        };

        _overheadBlockAction.canceled += context =>
        {
            animator.ResetTrigger("Overhead_Block");
            animator.SetTrigger("ShouldIdle");
        };

        InputAction _middleBlockAction = InputSystem.actions.FindAction("MiddleBlock");

        _middleBlockAction.started += context =>
        {
            if (state == State.Idle && GameState.fightBegan)
            {
                animator.ResetTrigger("Overhead_Attack");
                animator.ResetTrigger("ShouldIdle");
                animator.SetTrigger("Middle_Block");
                SwitchState(State.MiddleBlock);
            }
        };

        _middleBlockAction.canceled += context =>
        {
            animator.ResetTrigger("Middle_Block");
            animator.SetTrigger("ShouldIdle");
        };

        InputAction _lowBlockAction = InputSystem.actions.FindAction("LowBlock");

        _lowBlockAction.started += context =>
        {
            if (state == State.Idle && GameState.fightBegan)
            {
                animator.ResetTrigger("Overhead_Attack");
                animator.ResetTrigger("ShouldIdle");
                animator.SetTrigger("Low_Block");
                SwitchState(State.LowBlock);
            }
        };

        _lowBlockAction.canceled += context =>
        {
            animator.ResetTrigger("Low_Block");
            animator.SetTrigger("ShouldIdle");
        };
    }

    private void OnOverheadAttack(InputValue value)
    {
        if (value.isPressed && state == State.Idle && GameState.fightBegan)
        {
            animator.SetTrigger("Overhead_Attack");
            SwitchState(State.OverheadAttack);
        }
    }

    private void OnMiddleAttack(InputValue value)
    {
        if (value.isPressed && state == State.Idle && GameState.fightBegan)
        {
            animator.SetTrigger("Middle_Attack");
            SwitchState(State.MiddleAttack);
        }
    }

    private void OnLowAttack(InputValue value)
    {
        if (value.isPressed && state == State.Idle && GameState.fightBegan)
        {
            animator.SetTrigger("Low_Attack");
            SwitchState(State.LowAttack);
        }
    }

    private void OnIdle()
    {
        if (state != State.Idle)
        {
            SwitchState(State.Idle);
            isRecovering = false;
        }
    }
}
