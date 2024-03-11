using UnityEngine;

public class ItemInteraction : MonoBehaviour, Interactable
{
    [SerializeField] private string itemName;
    [SerializeField] private bool changeSprite;
    [SerializeField] private Sprite newSprite;
    private SpriteRenderer _spriteRenderer;
    
    public void Interact()
    {
        PlayerDataHolder.AddItemToInventory(itemName);
        GameEvents.showPrompt?.Invoke("You found a " + itemName + "!", false);
        if (changeSprite)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = newSprite;
        }
    }
}
