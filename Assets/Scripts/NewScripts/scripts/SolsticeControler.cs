using UnityEngine;
using UnityEngine.UI;

public class SolsticeControler : MonoBehaviour
{
    public Sprite[] SolstisObjects;
    public static int solIndex = 1;

    public Image solFirst, solSecond;

    public void SolsticChange()
    {
        solFirst.sprite = SolstisObjects[solIndex];
        solSecond.sprite = SolstisObjects[solIndex+1];
    }
}
