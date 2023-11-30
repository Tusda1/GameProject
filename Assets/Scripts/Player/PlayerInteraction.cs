using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Controls _input = null;
    private bool isInteractPressed = false;

    public static PlayerInteraction Instance { get; private set; }

    public bool IsInteractPressed => isInteractPressed;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Duplicate PlayerInteraction instance found. Destroying the duplicate.");
            Destroy(gameObject);
        }

        _input = new Controls();
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void Update()
    {
        isInteractPressed = _input.Player.Interact.WasReleasedThisFrame();
    }
}