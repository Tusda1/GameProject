using System;

public class ActionInfo
{
    public Action action;
    public bool isCombatMove;

    public ActionInfo(Action action, bool isCombatMove)
    {
        this.action = action;
        this.isCombatMove = isCombatMove;
    }
}