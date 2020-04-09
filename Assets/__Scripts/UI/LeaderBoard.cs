using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private LeaderBoardItem lbItem;
    [SerializeField] private int leaderboardSize = 10;
    [SerializeField] private VerticalLayoutGroup layoutGroup;

    private Data.LeaderBoard leaderBoard;

    void Start()
    {
        // Read the top scores and print them to the LeaderBoard
        leaderBoard = Data.SaveSystem.LoadLeaderBoard();

        if (leaderBoard.players != null)
        {
            for (int i = 0; i < leaderBoard.players.Length; i++)
            {
                if (i < leaderboardSize)
                {
                    var item = Instantiate(lbItem, layoutGroup.transform);
                    item.WriteText($"{leaderBoard.players[i].name} {leaderBoard.players[i].score}");
                }
            }
        }
    }
}
