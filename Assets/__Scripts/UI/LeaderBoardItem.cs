using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Data;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LeaderBoardItem : MonoBehaviour
{
    public void SetPlayerData(PlayerData player, int rank)
    {
        // Reference: "textmeshpro right and left align on same line"
        // https://forum.unity.com/threads/textmeshpro-right-and-left-align-on-same-line.485157/
        GetComponentInChildren<TextMeshProUGUI>().text = (
            $@"<align=left>{rank}. {player.name}<line-height=0.001>
            <align=right>{string.Format("{0:n0}", player.score)}"
        );
    }
}
