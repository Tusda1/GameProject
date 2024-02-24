using UnityEngine;

public class MakeSavefileCopy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SaveFileHandler.CopySaveDataToPersistentPath();
        NpcDialogueHandler.CopyDialogueDataToPersistentPath();
        Debug.Log(Application.persistentDataPath);
        DataBase.InitializeData();
        PlayerDataHolder.InitializePlayerData(DataBase.allPlayerSkills);
    }
}
