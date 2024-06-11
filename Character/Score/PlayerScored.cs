using Assets.Scripts.UI;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScored : NetworkBehaviour
{
    private PlayerTableUI m_PlayerTableUI;
    private GameObject collisionBall;
    private Vector3 initialBallPos = new Vector3(0,0,0);

    private void Start()
    {
        m_PlayerTableUI = GetComponentInParent<PlayerTableUI>();
    }
    public void InvokeScore(RaycastHit hit)
    {
        collisionBall = hit.transform.gameObject;
        InvokeRepeating("CheckForScored", 0.1f, 0.1f);
    }
    private void CheckForScored()
    {
        m_PlayerTableUI.CmdUpdateTable();

        if (Ball.isScored)
            StartCoroutine("SetBallAgain", collisionBall);
    }
    private IEnumerator SetBallAgain(GameObject collision)
    {
        Ball.isScored = false;
        collisionBall.transform.tag = "Untagged";

        yield return new WaitForSeconds(3f);
        CmdSetBallPosition();
        yield return new WaitForSeconds(0.5f);
        collisionBall.transform.tag = "Ball";
        CancelInvoke("CheckForScored");
    }
    [Command]
    private void CmdSetBallPosition()
    {
        RpcSetBallPosition();
    }
    [ClientRpc]
    private void RpcSetBallPosition()
    {
        collisionBall.transform.position = initialBallPos;
    }
}
