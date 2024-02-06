using UnityEngine;

public static class GameEvents
{
    public delegate void GameEvent();
    public static GameEvent interactPressed;
    public static GameEvent downMenu;
    public static GameEvent upMenu;
    public static GameEvent leftMenu;
    public static GameEvent rightMenu;
    public static GameEvent playerFrozen;
    public static GameEvent playerUnfrozen;
}
