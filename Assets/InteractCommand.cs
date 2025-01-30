using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractCommand : PlayerActionCommand
{
    public InteractCommand(ref GameObject _player, ref GameObject _interactable)
        : base(ref _player) 
    {
        interactable = _interactable;
    }
    public override void Execute()
    {
        Interactable interactableComponent = interactable.GetComponent<Interactable>();
        player.StartInteraction(interactable);
    }

    GameObject interactable;
}
