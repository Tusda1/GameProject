using UnityEngine;
using UnityEngine.Playables;

public class ObjeckInteracteble : MonoBehaviour, Interactable
{
    public static bool gotClueThisLevel;
    public PlayableDirector playableDirector;
    public SolsticeControler solsticeControler;
    
    public void Interact()
    {
        
        if (!gotClueThisLevel)
        {
            print("interacted");
            Player.indexCount++;
            gotClueThisLevel = true;
            solsticeControler.SolsticChange();
            SolsticeControler.solIndex++;
            SceneChanger.Instance.NextLevel();
        }
        
    }

    public void GoToNextLevel()
    {
        Player.indexCount++;
        gotClueThisLevel = true;
        solsticeControler.SolsticChange();
        SolsticeControler.solIndex++;
        SceneChanger.Instance.NextLevel();
    }

    public void StayHere()
    {
        print("closesText");
    }
}
