using UnityEngine;

public static class DataBase
{
    public static EquipmentScrub[] allEquipment;
    public static PlayerSkills[] allPlayerSkills;
    
    public static void InitializeData()
    {
        allEquipment = Resources.LoadAll<EquipmentScrub>("Scrubs/Items/Equipment");
        allPlayerSkills = Resources.LoadAll<PlayerSkills>("Scrubs/Combat/AttackMoves/Player");
    }
}
