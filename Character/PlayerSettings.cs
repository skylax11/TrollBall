using Assets.Scripts.UI;
using Mirror;
using Mirror.Examples.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSettings : NetworkBehaviour
{
    [Header("Movement Properties")]
    public Vector3 Movement;
    public float Speed = 0f;
    public float Accelerate = 0f;
    public MovementState StateMove;

    // Needed components...
    private InputSystem m_Inputs;
    private Rigidbody m_Rigidbody;
    private PlayerScored m_PlayerScored;

    [Header("Camera Stuff")]
    public GameObject CamPrefab;
    public CamHolder m_Camera;

    [Header("Pushing Away")]
    public Transform PushAwayPoint;

    [SyncVar(hook = nameof(ChangeColor))]
    Color32 m_Color = new Color32(32, 32, 32, 32);

    [Header("Position")]
    public Vector3 Position;


    public override void OnStartClient()
    {
        base.OnStartClient();
        m_Color = UnityEngine.Random.ColorHSV();
    }
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Inputs = GetComponent<InputSystem>();
        if (!isLocalPlayer)
            return;

        CmdSetPosition();
    }
    private void Start()
    {
        if (isLocalPlayer)
            m_Camera.cam.enabled = true;
        else
            m_Camera.cam.enabled = false;

        m_PlayerScored = GetComponent<PlayerScored>();
    }
    void FixedUpdate()
    {
        if (!isLocalPlayer) 
            return;

        if (StateMove == MovementState.Walking)
        {
            Speed += Accelerate * Time.deltaTime;
            Speed = Mathf.Clamp(Speed, 0, 20);
        }
        else if (StateMove == MovementState.Running)
        {
            Speed += Accelerate * Time.deltaTime;
            Speed = Mathf.Clamp(Speed, 0, 25);
        }
        else
        {
            Speed -= Accelerate * Time.deltaTime;
            Speed = Mathf.Clamp(Speed, 0, 25);
        }

        CmdRotate();
        CmdMove();
    }
    private void ChangeColor(Color32 oldValue,Color32 nextValue)
    {
        GetComponent<MeshRenderer>().material.color = m_Color;
    }
    [Command]
    public void CmdOnPushAway()
    {
        RpcOnPushAway();
    }
    [ClientRpc]
    private void RpcOnPushAway()
    {
        RaycastHit hit;
        if (Physics.Raycast(PushAwayPoint.position, PushAwayPoint.forward, out hit, 3f))
            if (hit.transform.TryGetComponent(out Rigidbody enemyRB))
            {
                enemyRB.AddForce(PushAwayPoint.forward * 500);
                if (hit.transform.CompareTag("Ball"))
                {
                    m_PlayerScored.InvokeScore(hit);
                }
            }
    }
    [Command]
    private void CmdRotate()
    {
        RpcRotate();
    }
    [ClientRpc]
    private void RpcRotate()
    {
        if (m_Camera == null)
            return; 
        float yRotation = m_Camera.yRot;
        transform.localRotation = Quaternion.Slerp(transform.localRotation , Quaternion.Euler(0f, yRotation, 0f) , Time.deltaTime * 45f);
    }
    [Command]
    private void CmdMove()
    { // checks everything on site of server.
      // and validates the logics
        RpcMove();
    }
    [ClientRpc]
    private void RpcMove()
    { // do the stuff.
        Movement = m_Inputs.Movement;
        Vector2 direction = m_Inputs.Direction;

        if (direction.y != 0)
            Movement += direction.y > 0 ? transform.forward : -transform.forward;
        if (direction.x != 0)
            Movement += direction.x > 0 ? transform.right : -transform.right;

        Vector3 totalVelo = Movement * Speed * Time.deltaTime;
        m_Rigidbody.velocity += totalVelo;

    }
    [Command]
    private void CmdSetPosition()
    {
        RpcSetPosition();
    }
    [ClientRpc]
    private void RpcSetPosition()
    {
        transform.position = Position;
    }
}
public enum MovementState
{
    Walking,
    Stopping,
    Running
}