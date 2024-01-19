using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using File = UnityEngine.Windows.File;

public class MainMenuManager : MonoBehaviour
{
    private bool _saveFileExists;
    [SerializeField] private GameObject[] _butttons;
    
    // Start is called before the first frame update
    void Start()
    {
        if (File.Exists(Path.Combine(Application.persistentDataPath, "SavegameCopy.json")))
        {
            _saveFileExists = true;
            Debug.Log("Save file exists.");
            
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
}
