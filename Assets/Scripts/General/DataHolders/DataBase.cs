using System.Linq;
using UnityEngine;

public static class DataBase
{
    public static EquipmentScrub[] allEquipment;
    public static ItemScrub[] allItems;
    public static PlayerSkills[] allPlayerSkills;
    public static EnemyScrub[] allEnemies;
    
    public static void InitializeData()
    {
        allEquipment = Resources.LoadAll<EquipmentScrub>("Scrubs/Items/Equipment");
        allPlayerSkills = Resources.LoadAll<PlayerSkills>("Scrubs/Combat/AttackMoves/Player");
        allEnemies = Resources.LoadAll<EnemyScrub>("Scrubs/Combat/Enemy/Enemies");
        
        //add every item in items folder and subfolders to allItems
        allItems = Resources.LoadAll<ItemScrub>("Scrubs/Items");
    }
}
