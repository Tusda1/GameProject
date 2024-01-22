using System;

public enum ActionType
{
    Attack,
    Item,
    Run,
    Talk,
    Menu
}

public class ActionInfo
{
    public Action action;
    public bool isCombatMove;
    public ActionType actionType;

    public ActionInfo(Action action, bool isCombatMove, ActionType actionType)
    {
        this.action = action;
        this.isCombatMove = isCombatMove;
        this.actionType = actionType;
    }
}