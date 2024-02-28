using UnityEngine;

public class DialogueStarter : MonoBehaviour
{
    [SerializeField] private GameObject npcObject;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            npcObject.GetComponent<Interactable>()?.Interact();
        }
    }
}
