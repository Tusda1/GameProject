using UnityEngine;

public enum EquipmentGrade
{
    Normal,
    Elite,
    Peerless,
    Mythical,
    Zenith
}

[CreateAssetMenu(menuName = "New Equipment", fileName = "Equipment")]
public class EquipmentScrub : ScriptableObject
{
    public string itemName;
    
    [TextArea]
    public string itemDescription;
    
    public Sprite itemSprite;
    public int itemValue;
    public bool doesHaveSpecialEffect;
    public bool isUnique;
    public EquipmentGrade grade;

    public float attack;
    public float defense;
}