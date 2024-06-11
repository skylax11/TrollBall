using Assets.Scripts.UI;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using static System.Net.Mime.MediaTypeNames;

public class PlayerLobbySettings : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnChangeName))]
    public string PlayerName;
    [SyncVar(hook = nameof(OnChangeTeam))]
    public int teamIndex;

    public TeamSelectPanel previousTeam;

    [Header("Network Manager")]
    [SerializeField] NetworkManagementCustom m_NetworkManager;

    [Header("TMP Name Input")]
    public TMP_InputField nameInputField;

    private int connectionIndex;

    private void Awake()
    {
        m_NetworkManager = NetworkManager.singleton as NetworkManagementCustom;
    }
    private void Start()
    {
        if (transform.CompareTag("PlayerLobby"))
        {
            MainMenu.instance.ButtonList[0].GetComponent<Button>().onClick.AddListener(CmdSwitchToSpectator);
            MainMenu.instance.ButtonList[1].GetComponent<Button>().onClick.AddListener(CmdSwitchToRedTeam);
            MainMenu.instance.ButtonList[2].GetComponent<Button>().onClick.AddListener(CmdSwitchToBlueTeam);
        }

        if (!isLocalPlayer)
            return;

        int teamId = PlayerPrefs.GetInt("teamIndex_Saved" + connectionIndex);

        if (teamId != -1 && !transform.CompareTag("PlayerLobby"))
        {
            teamIndex = teamId;
        }
        
    }
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        CmdSetDisplayName(PlayerNameDisplayPanel.DisplayName);
    }
    public override void OnStartClient()
    {
        base.OnStartClient();

        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            GameObject.FindGameObjectWithTag("GoalPost_Red").GetComponent<ScoreGoal>().Team = NetworkManagementCustom.TeamRed;
            GameObject.FindGameObjectWithTag("GoalPost_Blue").GetComponent<ScoreGoal>().Team = NetworkManagementCustom.TeamBlue;
        }

        connectionIndex = NetworkManagementCustom.connectionIndex;
        gameObject.name += connectionIndex;
        NetworkManagementCustom.connectionIndex++;

        if (isServer && SceneManager.GetActiveScene().name == "Lobby")
            MainMenu.instance.HostStart.SetActive(true);

        if (!NetworkManagementCustom.Players.Contains(this))
            NetworkManagementCustom.Players.Add(this);

        for (int i = 0; i < NetworkManagementCustom.Players.Count; i++)
        {
            if (NetworkManagementCustom.Players[i].previousTeam == null)
            {
                SetTeamByTeamId(NetworkManagementCustom.Players[i]);

                m_NetworkManager.Teams[NetworkManagementCustom.Players[i].teamIndex].AddPlayer(PlayerNameDisplayPanel.DisplayName, this);
                previousTeam = m_NetworkManager.Teams[NetworkManagementCustom.Players[i].teamIndex];
            }
        }
        CmdUpdateUI();
    }
    public override void OnStopClient()
    {
        base.OnStopClient();
        NetworkManagementCustom.Players.Remove(this);
    }
    public void OnChangeTeam(int oldValue, int newValue)
    {
        teamIndex = newValue;
        PlayerPrefs.SetInt("teamIndex_Saved" + connectionIndex, teamIndex);
        CmdUpdateUI();
    }
    public void OnChangeName(string oldValue, string newValue)
    {
        PlayerName = newValue;
        CmdUpdateUI();
    }
    [Command]
    private void CmdUpdateUI()
    {
        RpcUpdateUI();
    }
    [ClientRpc]
    private void RpcUpdateUI()
    {
        if (TeamSelectPanel.PlayerList.Count <= 0)
            return;

            for (int i = 0; i < TeamSelectPanel.PlayerList.Count; i++)
            {
                TeamSelectPanel.textMeshProUGUIs[i].text = TeamSelectPanel.PlayerList[i].PlayerName;
            }
    }
    #region Switch Team
    [Command]
    public void CmdSwitchToBlueTeam()
    {
        RpcSwitchToBlueTeam();
    }
    [ClientRpc]
    public void RpcSwitchToBlueTeam()
    {
        teamIndex = 2;
        ChangeTeam();
    }
    [Command]
    public void CmdSwitchToRedTeam()
    {
        RpcSwitchToRedTeam();
    }
    [ClientRpc]
    public void RpcSwitchToRedTeam()
    {
        teamIndex = 1;
        ChangeTeam();
    }
    [Command]
    public void CmdSwitchToSpectator()
    {
        RpcSwitchToSpectator();
    }
    [ClientRpc]
    public void RpcSwitchToSpectator()
    {
        teamIndex = 0;
        ChangeTeam();
    }
    private void ChangeTeam()
    {
        previousTeam.RemovePlayer(PlayerName, this);
        m_NetworkManager.Teams[teamIndex].AddPlayer(PlayerName, this);
        previousTeam = m_NetworkManager.Teams[teamIndex];
    }
    #endregion
    [Command]
    private void CmdSetDisplayName(string displayName)
    {
        PlayerName = displayName;
    }
    private PlayerLobbySettings SetTeamByTeamId(PlayerLobbySettings playerLobby)
    {
        switch (playerLobby.teamIndex)
        {
            case 0:
                playerLobby.previousTeam = m_NetworkManager.Teams[0];
                break;
            case 1:
                playerLobby.previousTeam = m_NetworkManager.Teams[1];
                break;
            case 2:
                playerLobby.previousTeam = m_NetworkManager.Teams[2];
                break;
        }
        return playerLobby;
    }
}
