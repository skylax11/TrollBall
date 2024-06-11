using Assets.Scripts.Teams;
using Assets.Scripts.UI;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreGoal : NetworkBehaviour
{
    public Team_SuperClass Team;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ball"))
        {
            Team.ScoreGoal();
            Ball.isScored = true;
        }
    }
}
