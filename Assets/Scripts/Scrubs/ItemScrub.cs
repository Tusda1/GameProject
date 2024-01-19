using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "New Item", fileName = "Item")]
public abstract class ItemScrub : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public Sprite itemSprite;
    public int itemValue;
    public bool isItemStackable;
    public bool isItemKeyItem;
    public bool doesItemHaveSpecialEffect;
    public bool isConsumable;
    
    public abstract void UseItem();
}
