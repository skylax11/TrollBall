using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;

namespace Assets.Scripts.UI
{
    public class PlayerTableUI : NetworkBehaviour
    {
        public static TextMeshProUGUI RedTeamScore;
        public static TextMeshProUGUI BlueTeamScore;

        private void Start()
        {
            RedTeamScore  = GamePanel.Instance.RedTeamScore;
            BlueTeamScore = GamePanel.Instance.BlueTeamScore;
        }

        /// <summary> Update UI on server and send to clients </summary>
        [Command]
        public void CmdUpdateTable()
        {
            RpcUpdateTable();
        }
        [ClientRpc]
        public void RpcUpdateTable()
        {
            RedTeamScore.text = NetworkManagementCustom.TeamBlue.ScoredGoals.ToString();
            BlueTeamScore.text = NetworkManagementCustom.TeamRed.ScoredGoals.ToString();
        }
    }
}
