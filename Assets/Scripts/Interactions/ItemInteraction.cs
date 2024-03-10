using UnityEngine;

public class ItemInteraction : MonoBehaviour, Interactable
{
    private bool _interacted;
    [SerializeField] private string itemName;
    [SerializeField] private bool changeSprite;
    [SerializeField] private Sprite newSprite;
    private SpriteRenderer _spriteRenderer;
    
    public void Interact()
    {
        if (_interacted)
            return;
        
        PlayerDataHolder.AddItemToInventory(itemName);
        GameEvents.showPrompt?.Invoke("You found a " + itemName + "!");
        if (changeSprite)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = newSprite;
        }
        
        _interacted = !_interacted;
    }
}
