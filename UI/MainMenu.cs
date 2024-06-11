using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Network Manager")]
    [SerializeField] NetworkManagementCustom m_NetworkManager;

    public GameObject[] Buttons;

    [Header("Nickname Section")]
    public TMP_InputField Nickname;
    public GameObject NickNamePanel;
    private int index;
    [Header("Player Properties")]
    public GameObject PlayerListPanel;

    public static MainMenu instance;

    [Header("Team Select Buttons")]
    public List<GameObject> ButtonList = new List<GameObject>();

    [Header("Host Start Button")]
    public GameObject HostStart;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(instance);
    }
    public void HideButtons()
    {
        foreach (GameObject button in Buttons) { button.SetActive(false); }
    }
    public void NicknamePanel(int index)
    {
        this.index = index;
        HideButtons();
        NickNamePanel.SetActive(true);
    }
    public void JoinGameButton()
    {
        if (index == 0)
            m_NetworkManager.StartHost();
        else if (index == 1)
            m_NetworkManager.StartClient();
        NickNamePanel.SetActive(false);
        PlayerListPanel.SetActive(true);
    }

}
