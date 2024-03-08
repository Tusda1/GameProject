using UnityEngine;

public static class PlayerEvents 
{
    public delegate void PlayerEvent();
    public static PlayerEvent playerFrozen;
    public static PlayerEvent playerUnfrozen;
    public static PlayerEvent playerCantInteract;
    public static PlayerEvent playerCanInteract;

    public static PlayerEvent playerMoved;

    public delegate void ReturnPlayerEvent(GameObject player);
    public static ReturnPlayerEvent returnPlayer;
}
