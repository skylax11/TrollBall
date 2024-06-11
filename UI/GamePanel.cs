using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class GamePanel : NetworkBehaviour
    {
        public static GamePanel Instance;

        public TextMeshProUGUI RedTeamScore;
        public TextMeshProUGUI BlueTeamScore;

        private void Awake() => Instance = this;

    }
}
