using System;
using UnityEngine;

[Serializable]
public class MiddleAttack : IBotAction
{
    [field: SerializeField] public bool IsCollapsed { get; set; }
    [field: SerializeField] public bool IsEnabled { get; set; }
    [field: SerializeField] public bool IsOffensive { get; set; }
    [field: SerializeField] public float MaxSecondsBeforeAction { get; set; }
    [field: SerializeField] public float MinSecondsBeforeAction { get; set; }

    public void Action()
    {
        BotController.state = BotController.State.MiddleAttack;
        BotController.animator.SetTrigger("Middle_Attack");
    }

    public bool Condition()
    {
        if (PlayerController.state == PlayerController.State.OverheadAttack && PlayerController.state == PlayerController.State.MiddleAttack && PlayerController.state == PlayerController.State.LowAttack) return false;

        if (PlayerController.state == PlayerController.State.Parried || PlayerController.state == PlayerController.State.Idle || PlayerController.state == PlayerController.State.OverheadBlock || PlayerController.state == PlayerController.State.MiddleBlock)
        {
            return true;
        }

        return false;
    }
}
