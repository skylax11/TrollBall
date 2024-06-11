using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TeamSelectPanel : MonoBehaviour
{
    public static List<TextMeshProUGUI> textMeshProUGUIs = new List<TextMeshProUGUI>();
    public static List<PlayerLobbySettings> PlayerList = new List<PlayerLobbySettings>();
    public Transform Hierarchy;
    public GameObject TMP_Text_Prefab;
    public int teamIndex;

    public void AddPlayer(string text , PlayerLobbySettings player)
    {
        GameObject theText = Instantiate(TMP_Text_Prefab);
        theText.transform.SetParent(Hierarchy);
        theText.transform.localScale = Vector3.one;
        theText.GetComponent<TextMeshProUGUI>().text = text;
        textMeshProUGUIs.Add(theText.GetComponent<TextMeshProUGUI>());
        PlayerList.Add(player); 
    }
    public void RemovePlayer(string text, PlayerLobbySettings player)
    {
        var selectedText = textMeshProUGUIs.Where(x=>x.text == text).FirstOrDefault();
        textMeshProUGUIs.Remove(selectedText);
        Destroy(selectedText.gameObject);
        PlayerList.Remove(player);
    }
}
