using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerActionCommand : Command
{
    protected PlayerController player;

    public PlayerActionCommand(ref GameObject _player)
    {
        this.player = _player.GetComponent<PlayerController>();
    }

    public abstract override void Execute();
}
