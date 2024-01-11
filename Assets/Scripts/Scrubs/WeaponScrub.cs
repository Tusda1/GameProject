using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponGrade
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

[CreateAssetMenu(menuName = "New Weapon", fileName = "Weapon")]
public class WeaponScrub : ScriptableObject
{
    public string weaponName;
    public string weaponDescription;
    public Sprite weaponSprite;
    public int weaponValue;
    public int weaponDamage;
    public bool doesWeaponHaveSpecialEffect;
    public bool isWeaponUnique;
}
