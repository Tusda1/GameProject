using UnityEngine;

public class MakeSavefileCopy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SaveFileHandler.CopySaveDataToPersistentPath();
        Debug.Log(Application.persistentDataPath);
        PlayerDataHolder.InitializePlayerData(GameManager.Instance.allAttackMoves);
    }
}
