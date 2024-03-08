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
    public static GameEvent levelUp;
    public static GameEvent endLevelUp;
    
    //Sound
    public static GameEvent playWritingSound;
    
    public delegate void MovePlayer(int x, int y);
    public static MovePlayer movePlayer;
    
    public delegate void PromptEvent(string prompt);
    public static PromptEvent showPrompt;
    public static GameEvent hidePrompt;
    
    public delegate void SceneEvent(string sceneName, bool additive, string doorName);
    public static SceneEvent loadScene;
    public delegate void FindObject(string doorName);
    public static FindObject lookForObject;

    public static GameEvent SceneFadeOut;
    public static GameEvent SceneFadeIn;
    
    public delegate void ReturnDoor(Vector2 position);
    public static ReturnDoor returnDoor;
}
