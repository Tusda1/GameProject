using System.Collections;
using UnityEngine;

public class NpcMovement : MonoBehaviour
{
    private float _gridSize = 0.5f;
    
    public void MoveNpc(int x, int y, int speed)
    {
        StartCoroutine(Move(new Vector2(x, y)));
    }
    
    private IEnumerator Move(Vector2 direction)
    {
        NpcSpeaker.CurrentSpeaker.waitingForSignal = true;
        
        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + (direction * _gridSize);
        float moveDuration = 0.15f;
        moveDuration *= direction.magnitude;
        
        float elapsedTime = 0;
        while (elapsedTime < moveDuration)
        {
            transform.position = Vector2.Lerp(startPosition, endPosition, (elapsedTime / moveDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        transform.position = endPosition;
        
        NpcSpeaker.CurrentSpeaker.waitingForSignal = false;
    }
}
