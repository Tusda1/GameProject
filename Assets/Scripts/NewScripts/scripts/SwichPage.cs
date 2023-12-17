using System.Collections;
using UnityEngine;

public class SwitchPage : MonoBehaviour
{
    [SerializeField]private bool _hasHapend;
    private bool canFlip = true;
    private GameObject _gameObject;
    
    //private Input_Controler _input;
   
    public GameObject[] background;
    public GameObject flipRight;
    public GameObject flipLeft;
    public int index;

    void Start()
    {
        index = 0;
        //_input = GetComponent<Input_Controler>();
    }
        

    void Update()
    {
        if(index >= Player.indexCount)
            index = Player.indexCount; 

        if(index < 0)
            index = 0 ;
        


        if(index == 0)
        {
            background[0].gameObject.SetActive(true);
        }
        

        if (Input_Controler.Instance.bookDirection)
        {
            _hasHapend = true;
        }
        
        if (!_hasHapend) return;
      
        
        if (Input_Controler.Instance.moveDirection.x > 0.5f && canFlip)
        {
            StartCoroutine(Next());
            _hasHapend = false;
        }
        else if (Input_Controler.Instance.moveDirection.x < -0.5f && canFlip)
        {
            StartCoroutine(Previous());
            _hasHapend = false;
        }
    }

    private IEnumerator Next()
    {
        if (index == Player.indexCount) yield break;
        index += 1;
        
        flipRight.gameObject.SetActive(true);
        canFlip = false;
        yield return new WaitForSeconds(0.5f);
        canFlip = true;
        flipRight.gameObject.SetActive(false);
        
        foreach (var t in background)
        {
            t.gameObject.SetActive(false);
            background[index].gameObject.SetActive(true);
        }
        print("Heyo");
    }

    private IEnumerator Previous()
    {
        if (index == 0) yield break;
        index -= 1;
        
        flipLeft.gameObject.SetActive(true);
        canFlip = false;
        yield return new WaitForSeconds(0.5f);
        canFlip = true;
        flipLeft.gameObject.SetActive(false);
    
        for(int i = 0 ; i < background.Length; i++)
        {
            background[i].gameObject.SetActive(false);
            background[index].gameObject.SetActive(true);
        }
        print("Heyhoooo");
    }

    
}
