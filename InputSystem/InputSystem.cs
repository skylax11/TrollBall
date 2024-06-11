using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputSystem : NetworkBehaviour
{
    [SerializeField] public PlayerInput playerInput;
    private PlayerSettings m_Player;

    // Movement 
    public Vector3 Movement;
    public Vector2 Direction;

    // OBSERVE PATTERN
    public Action OnPushAwayAction;

    private void Start()
    {
        m_Player = GetComponent<PlayerSettings>();
    }
    public void OnMove(InputValue val)
    {
        Direction = val.Get<Vector2>();
        if (Direction == Vector2.zero)
        {
            m_Player.StateMove = MovementState.Stopping;
            return;
        }
        if (!playerInput.actions["shifting"].IsPressed())
            m_Player.StateMove = MovementState.Walking;
        else
            OnShifting();
    }
    public void OnShifting()
    {
        if(playerInput.actions["shifting"].IsPressed())
            m_Player.StateMove = MovementState.Running;
    }
    public void OnPushAway()
    {
        if (playerInput.actions["PushAway"].IsPressed())
            OnPushAwayAction?.Invoke();
    }
}
