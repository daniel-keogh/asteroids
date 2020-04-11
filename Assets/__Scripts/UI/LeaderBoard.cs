using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private LeaderBoardItem lbItem;
    [SerializeField] private VerticalLayoutGroup layoutGroup;

    void Start()
    {
        PrintItems();
    }

    private void PrintItems()
    {
        // Read the top scores from the leaderboard file and print them to the screen
        var leaderBoard = SaveSystem.LoadLeaderBoard();

        if (leaderBoard.players != null)
        {
            for (int i = 0; i < leaderBoard.players.Length; i++)
            {
                var item = Instantiate(lbItem, layoutGroup.transform, false);
                item.SetPlayerData(leaderBoard.players[i], i + 1);
            }
        }
    }
}
