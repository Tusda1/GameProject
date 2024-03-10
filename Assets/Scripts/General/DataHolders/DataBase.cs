using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DataBase
{
    public static EquipmentScrub[] allEquipment;
    public static ItemScrub[] allItems;
    public static PlayerSkills[] allPlayerSkills;
    public static EnemyScrub[] allEnemies;
    public static Dictionary<string, int> whichDialogueDictionary = new Dictionary<string, int>();
    
    public static void InitializeData()
    {
        allItems = Resources.LoadAll<ItemScrub>("Scrubs/Items");
        allEquipment = Resources.LoadAll<EquipmentScrub>("Scrubs/Items/Equipment");
        allPlayerSkills = Resources.LoadAll<PlayerSkills>("Scrubs/Combat/AttackMoves/Player");
        allEnemies = Resources.LoadAll<EnemyScrub>("Scrubs/Combat/Enemy/Enemies");

        whichDialogueDictionary = NpcDialogueHandler.GetAllWhichDialogues();
    }
}
