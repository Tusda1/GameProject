using UnityEngine;

public static class GameEvents
{
    public delegate void GameEvent();
    public static GameEvent interactPressed;
    public static GameEvent down;
    public static GameEvent up;
    public static GameEvent left;
    public static GameEvent right;
    public static GameEvent playerFrozen;
    public static GameEvent playerUnfrozen;
    public static GameEvent levelUp;
    public static GameEvent endLevelUp;
}
