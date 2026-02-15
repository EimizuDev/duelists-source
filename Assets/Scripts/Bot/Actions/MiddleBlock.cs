using System;
using UnityEngine;

[Serializable]
public class MiddleBlock : IBotAction
{
    [field: SerializeField] public bool IsCollapsed { get; set; }
    [field: SerializeField] public bool IsEnabled { get; set; }
    [field: SerializeField] public bool IsOffensive { get; set; }
    [field: SerializeField] public float MaxSecondsBeforeAction { get; set; }
    [field: SerializeField] public float MinSecondsBeforeAction { get; set; }

    public void Action()
    {
        BotController.state = BotController.State.MiddleBlock;
        BotController.animator.SetTrigger("Middle_Block");
    }

    public bool Condition()
    {
        if (PlayerController.state == PlayerController.State.MiddleAttack)
        {
            BotController.stopBlockingDelegate = StopBlocking;

            return true;
        }

        return false;
    }

    private void StopBlocking()
    {
        BotController.animator.SetTrigger("ShouldIdle");
        BotController.stopBlockingDelegate -= StopBlocking;
    }
}
