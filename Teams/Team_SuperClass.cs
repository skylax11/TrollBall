using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Teams
{
    public class Team_SuperClass
    {
        public int ScoredGoals = 0;
        public void ScoreGoal()
        {
            ScoredGoals++;
        }
    }
}
