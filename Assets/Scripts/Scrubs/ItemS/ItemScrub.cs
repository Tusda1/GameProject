using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Item", menuName = "Items/New Item")]
public class ItemScrub : ScriptableObject
{
    public string itemName;
    
    [TextArea]
    public string itemDescription;
    
    public Sprite itemSprite;
    public int itemValue;
    public bool isItemStackable;
    public bool isItemKeyItem;
    public bool doesItemHaveSpecialEffect;  
    public bool isConsumable;
    
    public void UseItem(){}
}
