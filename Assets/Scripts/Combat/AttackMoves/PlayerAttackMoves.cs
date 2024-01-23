using System;
using System.Collections.Generic;

public static class PlayerAttackMoves
{
    public static Dictionary<string, Action> attackMoves = new Dictionary<string, Action>();
    public static string currentAttackMove;
    

    public static void PutAttackMovesInDictionary()
    {
        attackMoves.Add("Punch", Punch);
    }
        
    #region AttackMoves

    public static void Punch()
    {
        currentAttackMove = "Punch";
    }

    #endregion
}
