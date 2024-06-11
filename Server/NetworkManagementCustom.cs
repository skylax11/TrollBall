using Assets.Scripts.Teams;
using Assets.Scripts.UI;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManagementCustom : NetworkManager
{
    public GameObject[] Buttons;

    [Header("Nickname Section")]
    public TMP_InputField Nickname;
    public GameObject NickNamePanel;
    [Header("Player Properties")]
    public GameObject PlayerListPanel;
    public TextMeshProUGUI[] TextMeshProUGUIs;
    public static List<PlayerLobbySettings> Players = new List<PlayerLobbySettings>();
    [Header("Team Properties")]
    public List<TeamSelectPanel> Teams = new List<TeamSelectPanel>();
    [Header("Prefab For Player")]
    public GameObject FootballerPrefab;

    [Header("Blue Team Start Positions")]
    public List<Transform> blueTeamPos;
    [Header("Red Team Start Positions")]
    public List<Transform> redTeamPos;

    public static int connectionIndex = 0;

    public static Team_SuperClass TeamBlue = new Team_SuperClass();
    public static Team_SuperClass TeamRed  = new Team_SuperClass();

    public override void OnServerChangeScene(string newSceneName)
    {
        connectionIndex = 0;
        base.OnServerChangeScene(newSceneName);
        playerPrefab = FootballerPrefab;

    }
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (SceneManager.GetActiveScene().name == "SampleScene" && (blueTeamPos.Count == 0 || redTeamPos.Count == 0))
        {
            foreach (var item in GameObject.FindGameObjectsWithTag("Blue"))
                if (!blueTeamPos.Contains(item.transform))
                    blueTeamPos.Add(item.transform);

            foreach (var item in GameObject.FindGameObjectsWithTag("Red"))
                if (!redTeamPos.Contains(item.transform))
                    redTeamPos.Add(item.transform);

            GameObject.FindGameObjectWithTag("GoalPost_Red").GetComponent<ScoreGoal>().Team = TeamRed;
            GameObject.FindGameObjectWithTag("GoalPost_Blue").GetComponent<ScoreGoal>().Team = TeamBlue;
        }
        if (PlayerPrefs.GetInt("teamIndex_Saved" + conn.connectionId) == 1)
            startPositions = redTeamPos;
        else if (PlayerPrefs.GetInt("teamIndex_Saved" + conn.connectionId) == 2)
            startPositions = blueTeamPos;

        base.OnServerAddPlayer(conn);
    }

}
