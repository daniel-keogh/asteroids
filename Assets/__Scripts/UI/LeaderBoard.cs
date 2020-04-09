using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        // Read the top scores and print them to the scren
        var leaderBoard = Data.SaveSystem.LoadLeaderBoard();

        if (leaderBoard.players != null)
        {
            for (int i = 0; i < leaderBoard.players.Length; i++)
            {
                var item = Instantiate(lbItem, layoutGroup.transform, false);
                item.WritePlayerData(leaderBoard.players[i]);
            }
        }
    }
}
