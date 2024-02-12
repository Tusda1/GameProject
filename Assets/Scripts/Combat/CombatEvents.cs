public static class CombatEvents
{
    public delegate void TurnEvent(ActionType actionType, string actionName);
    public static TurnEvent playerTurnEnd;
    
    public delegate void CombatEvent();
    public static CombatEvent combatEnd;
    public static CombatEvent updateHealthUI;
}
