using UnityEngine;

public class RegionScript : MonoBehaviour
{
    public static RegionScript Instance { get; private set; }
    public RegionScrub region;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
