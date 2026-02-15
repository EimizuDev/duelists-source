using System.Collections;
using UnityEngine;

public class BotController : MonoBehaviour
{
    static public Animator animator;

    public PlayerController playerController;

    public BotConfig selectedBotConfig;

    public enum State
    {
        Idle,
        Acting,
        OverheadAttack,
        OverheadBlock,
        MiddleAttack,
        MiddleBlock,
        LowAttack,
        LowBlock
    }

    static public State state;

    private int _offensiveAmount = 0;

    public delegate void StopBlockingDelegate();
    static public StopBlockingDelegate stopBlockingDelegate;

    public void DoAction()
    {
        if (state == State.Idle && GameState.fightBegan)
        {
            int attackNumber = Random.Range(0, _offensiveAmount);

            int offensiveIndex = -1;
            foreach (IBotAction action in selectedBotConfig.actions)
            {
                offensiveIndex += action.IsOffensive ? 1 : 0;

                if ((attackNumber == offensiveIndex || !action.IsOffensive) && action.IsEnabled && (action.IsOffensive || !playerController.isRecovering) && action.Condition())
                {
                    state = State.Acting;

                    animator.ResetTrigger("Low_Parry");
                    animator.ResetTrigger("Middle_Parry");
                    animator.ResetTrigger("Overhead_Parry");

                    StartCoroutine(DelayedAction(action));
                    break;
                }
            }
        }
    }

    IEnumerator DelayedAction(IBotAction action)
    {
        yield return new WaitForSeconds(Random.Range(action.MinSecondsBeforeAction, action.MaxSecondsBeforeAction));

        action.Action();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();

        foreach (IBotAction action in selectedBotConfig.actions)
        {
            _offensiveAmount += action.IsOffensive ? 1 : 0;
        }
    }

    private void Update()
    {
        if (PlayerController.state == PlayerController.State.Idle && stopBlockingDelegate != null)
        {
            stopBlockingDelegate();
        }
    }

    public void OnIdle()
    {
        if (state != State.Acting)
        {
            state = State.Idle;
            DoAction();
        }
    }
}
