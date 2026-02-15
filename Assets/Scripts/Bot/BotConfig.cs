using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewBot", menuName = "Bot Config")]
public class BotConfig : ScriptableObject
{
    [SerializeReference] public List<IBotAction> actions = new List<IBotAction>();
}
