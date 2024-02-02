using UnityEngine;

public enum EquipmentGrade
{
    Normal,
    Elite,
    Peerless,
    Mythical,
    Zenith
}

[System.Serializable]
public class StatModifier
{
    public string statName;
    public int modifier;
}

[CreateAssetMenu(fileName = "Equipment", menuName = "Items/Equipment/New Equipment")]
public class EquipmentScrub : ScriptableObject
{
    public string itemName;
    
    [TextArea]
    public string itemDescription;
    
    public Sprite itemSprite;
    public int itemValue;
    public bool isUnique;
    public EquipmentGrade grade;
    
    public StatModifier[] statModifiers;
    
    public WeaponSkills[] weaponSkills;
}