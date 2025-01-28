using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    private void Awake()
    {
        tag = "Interactable";
    }

    public abstract void StartInteraction();
    public abstract void EndInteraction();

    public abstract bool StartInteractionInputCheck();
    public abstract bool EndInteractionInputCheck();

    [SerializeField] protected List<KeyCode> startInteractionKeyCode;
    [SerializeField] protected List<KeyCode> endInteractionKeyCode;
}