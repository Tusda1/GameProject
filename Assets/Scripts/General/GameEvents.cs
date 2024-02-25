using UnityEngine;

public static class GameEvents
{
    public delegate void GameEvent();
    //Input
    public static GameEvent interactPressed;
    public static GameEvent down;
    public static GameEvent up;
    public static GameEvent left;
    public static GameEvent right;
    
    //Player
    public static GameEvent playerFrozen;
    public static GameEvent playerUnfrozen;
    public static GameEvent levelUp;
    public static GameEvent endLevelUp;
    
    //Sound
    public static GameEvent playWritingSound;
    
    public delegate void MovePlayer(int x, int y);
    public static MovePlayer movePlayer;
}
