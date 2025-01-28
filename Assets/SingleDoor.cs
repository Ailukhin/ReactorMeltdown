using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystems;

public class SingleDoor : Interactable
{
    public float maxTorque = 5f;
    public float minTorque = -1f;
    public float doorRotationOpenLimit = -120f;
    public float doorRotationClosedLimit;
    public float sensitivity = 1f;
    public float doorBrakingSensitivity = 0.5f;
    private float doorRotation;
    private float torque;
    private float prevTorque = 0f;
    private float directionModifier; // this is like flipping a switch, negative when player needs to pull open the door
    bool isHoldingDoor = false;

    public void Awake()
    {
        interactableType = INTERACTABLE_TYPE.UNFOCUSED;
        doorRotation = doorRotationClosedLimit;
        torque = 0f;
        directionModifier = 1f; 
    }

    public override void EndInteraction()
    {
        isHoldingDoor = false;
    }

    public override void Interaction()
    {
        isHoldingDoor = true;

        Vector3 toPlayer = GameController.GetPlayer().transform.position - transform.position;

        if (Vector3.Angle(transform.forward, toPlayer) < 90f)
        {
            directionModifier = -1f;
        }
        else
        {
            directionModifier = 1f;
        }
    }

    // left mouse down
    public override bool StartInteractionInputCheck()
    {
        return startInteractionKeyCode.TrueForAll(Input.GetKeyDown);
    }

    // left mouse up
    public override bool EndInteractionInputCheck()
    {
        return endInteractionKeyCode.TrueForAll(Input.GetKeyUp);
    }

    private void FixedUpdate()
    {
        torque = doorBrakingSensitivity >= 0f ? torque - doorBrakingSensitivity : torque + doorBrakingSensitivity;

        if (isHoldingDoor)
        {
            torque = directionModifier * Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime + prevTorque;
        }

        torque = Mathf.Clamp(torque, minTorque, maxTorque);

        doorRotation = Mathf.Clamp(doorRotation - torque, doorRotationOpenLimit, doorRotationClosedLimit);

        transform.localEulerAngles = new Vector3(0f, doorRotation, 0.0f);

        prevTorque = torque;
    }
}
