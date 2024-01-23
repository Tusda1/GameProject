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
    public string actionName;
    public bool isCombatMove;
    public ActionType actionType;

    public ActionInfo(Action action, string actionName, bool isCombatMove, ActionType actionType)
    {
        this.action = action;
        this.actionName = actionName;
        this.isCombatMove = isCombatMove;
        this.actionType = actionType;
    }
}