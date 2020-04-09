using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Data;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LeaderBoardItem : MonoBehaviour
{
    public void WritePlayerData(PlayerData player)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = $"{player.name} {player.score}";
    }
}
