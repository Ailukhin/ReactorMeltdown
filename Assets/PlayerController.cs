using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM
{
	public static PlayerState_Normal state_Normal = new PlayerState_Normal();
	public static PlayerState_Interacting state_Interacting = new PlayerState_Interacting();

	public static PlayerState ChangeState(PLAYER_STATE _state, ref PlayerState _curState)
	{
		PlayerState nextState = null;

		_curState.LeaveState();

		switch (_state)
		{
			case PLAYER_STATE.NORMAL:
				nextState = state_Normal;
				break;
			case PLAYER_STATE.INTERACTING:
				nextState = state_Interacting;
				break;
			default:
				nextState = state_Normal;
				break;
		}

		nextState.EnterState(_curState.playerGO);

		return nextState;
	}
}

public abstract class PlayerState
{
	public virtual void EnterState(GameObject _player)
	{
		playerGO = _player;
		player = _player.GetComponent<PlayerController>();
	}
	public abstract void Process();
	public virtual void FixedProcess() { }
	public abstract void LeaveState();

	public PlayerController player { get; protected set; }
	public GameObject playerGO { get; protected set; }
}

public class PlayerState_Normal : PlayerState
{
	public override void EnterState(GameObject _player)
	{
		base.EnterState(_player);

		player.NormalStateEnter();
	}
	public override void Process()
	{
		player.NormalStateUpdate();
	}

	public override void FixedProcess()
	{
		player.NormalStateFixedUpdate();
	}

	public override void LeaveState()
	{

	}

}

public class PlayerState_Interacting : PlayerState
{
	public override void EnterState(GameObject _player)
	{
		base.EnterState(_player);

		player.InteractingStateEnter();
	}

	public override void Process()
	{
		player.InteractingStateUpdate();
	}

	public override void LeaveState()
	{
	}
}

public class PlayerController : MonoBehaviour
{
	private Queue<PlayerActionCommand> ActionCommandQueue = new Queue<PlayerActionCommand>();   // a queue holding all actions to be processed

	private PlayerState currentState;

	public GameObject currentInteractable { get; private set; }

	private void Awake()
	{
		currentState = PlayerFSM.state_Normal;
		currentState.EnterState(gameObject);
	}

	void Update()
	{
		currentState.Process();

		ProcessActionCommands();
	}

	void ProcessActionCommands()
	{
		while (ActionCommandQueue.Count != 0)
		{
			Command cmd = ActionCommandQueue.Dequeue();

			if (cmd != null)
			{
				cmd.Execute();
			}
		}
	}

	public void StartInteraction(GameObject _interactable)
	{
		currentInteractable = _interactable;
		Interactable interactable = currentInteractable.GetComponent<Interactable>();
		currentState = PlayerFSM.ChangeState(PLAYER_STATE.INTERACTING, ref currentState);
		interactable.Interaction();
	}

	public void EndInteraction()
	{
		currentInteractable.GetComponent<Interactable>().EndInteraction();
		currentState = PlayerFSM.ChangeState(PLAYER_STATE.NORMAL, ref currentState);
		currentInteractable = null;
	}

	public void InteractingStateEnter()
	{
		Interactable interactable = currentInteractable.GetComponent<Interactable>();


		if (interactable.interactableType == INTERACTABLE_TYPE.FOCUSED)
		{
			/* In the game I'm making, I focus the camera in a direction and position. We might want to do something similar. */
			//cameraScript.StartZoom(playerCamera.transform, interactable.cameraFocusLocation);
			//gameObject.transform.rotation = interactable.playerFocusLocation.rotation;
			//float y = gameObject.transform.position.y;
			//gameObject.transform.position = new Vector3(interactable.playerFocusLocation.position.x, y, interactable.playerFocusLocation.position.z);
			//Cursor.visible = true;
			//Cursor.lockState = CursorLockMode.Confined;
		}
		else if (interactable.interactableType == INTERACTABLE_TYPE.UNFOCUSED)
		{
			// empty for now
		}
	}

	public void InteractingStateUpdate()
	{
		TryQueueEndInteractCommand();
	}

	bool TryQueueInteractCommand(ref GameObject _interactable)
	{
		bool status = false;

		Interactable interactable = _interactable.GetComponent<Interactable>();

		if (currentInteractable == null && interactable.StartInteractionInputCheck())
		{
			GameObject player = this.gameObject;
			ActionCommandQueue.Enqueue(new InteractCommand(ref player, ref _interactable));
			status = true;
		}

		return status;
	}

	bool TryQueueEndInteractCommand()
	{
		bool status = false;

		Interactable interactable = currentInteractable.GetComponent<Interactable>();
		if (currentInteractable != null && interactable.EndInteractionInputCheck())
		{
			GameObject player = this.gameObject;
			ActionCommandQueue.Enqueue(new EndInteractCommand(ref player));
			status = true;
		}

		return status;
	}

	public void NormalStateUpdate()
	{
		Look();

		/* Might wanna do something like this */ 
		//QueueUseItem();
		//QueueItemPickup();
		//QueueItemDrop();
		//SetSprint();
	}

	public void NormalStateFixedUpdate()
	{
		Move();
	}

	// Handles player movement
	void Move()
	{
		// if a key is pressed and the player is on a platform
		if (KeyBindings.IfAnyKey())
		{

		}
	}

	// Handles player rotation (if we're doing top-down)
	void Look()
	{

	}
}