using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MakeSavefileCopy : MonoBehaviour
{
    [FormerlySerializedAs("allPlayerSkills")] [FormerlySerializedAs("allPlayerAttackMoves")] [SerializeField] private AllAttackMoves allAttackMoves;
    // Start is called before the first frame update
    void Start()
    {
        SaveFileHandler.CopySaveDataToPersistentPath();
        Debug.Log(Application.persistentDataPath);
        PlayerDataHolder.InitializePlayerData(allAttackMoves);
    }
}
