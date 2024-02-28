
public static class ButtonScriptCombatMenu
{
    private static void FillerMethod(){}
    
    private static void AttackMoveButton()
    {
        MenuManouverer.Instance.ClearChoices();
        MenuManouverer.Instance.AddChoice("Back", BackToMain, "", false, ActionType.Menu);
        
        foreach (var attackMove in PlayerDataHolder.unlockedPlayerSkills)
        {
            MenuManouverer.Instance.AddChoice(attackMove, FillerMethod, attackMove, true, ActionType.Attack);
        }

        
        foreach (var equipped in PlayerDataHolder.EquippedItems)
        {
            foreach (var equipment in DataBase.allEquipment)
            {
                if (equipped == equipment.name)
                {
                    foreach (var weaponSkill in equipment.weaponSkills)
                    {
                        MenuManouverer.Instance.AddChoice(weaponSkill.attackMoveName, FillerMethod, weaponSkill.attackMoveName, true, ActionType.Attack);
                    }
                }
            }
        }
        
        
        MenuManouverer.Instance.LoadNewMenu();
    }

    private static void InventoryButton()
    {
        
    }

    public static void BackToMain()
    {
        MenuManouverer.Instance.ClearChoices();
        
        MenuManouverer.Instance.AddChoice("Attack", AttackMoveButton, "", false, ActionType.Menu);
        MenuManouverer.Instance.AddChoice("Inventory", InventoryButton, "", false, ActionType.Menu);
        MenuManouverer.Instance.AddChoice("Talk", FillerMethod, "", true, ActionType.Talk);
        MenuManouverer.Instance.AddChoice("Run", FillerMethod, "", true, ActionType.Run);
        
        MenuManouverer.Instance.LoadNewMenu();
    }
}
