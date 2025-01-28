using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndInteractCommand : PlayerActionCommand
{
    public EndInteractCommand(ref GameObject _player)
        : base(ref _player)
    {
    }
    public override void Execute()
    {
        player.EndInteraction();
    }
}
