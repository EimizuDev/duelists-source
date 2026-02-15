using System;
using UnityEngine;

public interface IBotAction
{
    bool IsCollapsed { get; set; }
    bool IsEnabled { get; set; }
    bool IsOffensive { get; set; }
    float MaxSecondsBeforeAction { get; set; }
    float MinSecondsBeforeAction { get; set; }

    bool Condition();
    void Action();
}
