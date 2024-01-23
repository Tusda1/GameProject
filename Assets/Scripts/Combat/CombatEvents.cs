using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatEvents
{
    public delegate void TurnEvent(ActionType actionType);
    public static TurnEvent playerTurnEnd;
}
