using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Data
{
    public static class SaveSystem
    {
        public static int MaxLeaderBoardSize
        {
            get
            {
                return maxLeaderBoardSize;
            }
            set
            {
                maxLeaderBoardSize = Mathf.Abs(value);
            }
        }

        private static int maxLeaderBoardSize = 12;

        // Path to the LeaderBoard data file
        private static readonly string LEADERBOARD_DATA_PATH = Application.persistentDataPath + "/leaderboard.json";

        public static void SaveToLeaderBoard(PlayerData player)
        {
            // Read current LeaderBoard
            LeaderBoard current = LoadLeaderBoard();
            List<PlayerData> temp;

            if (current.players != null)
            {
                temp = new List<PlayerData>(current.players);
            }
            else
            {
                temp = new List<PlayerData>();
            }

            if (temp.Count == maxLeaderBoardSize)
            {
                PlayerData min = temp.Min();

                if (player.score < min.score)
                {
                    // Player score not high enough to be added
                    return;
                }

                // Remove the lowest score
                temp.Remove(min);
            }

            temp.Add(player);

            // Create the new LeaderBoard
            LeaderBoard lb = new LeaderBoard();

            // Sort the List in descending order
            temp.Sort();
            temp.Reverse();
            lb.players = temp.ToArray();

            // Save the new LeaderBoard to the leaderboard.json file
            string json = JsonUtility.ToJson(lb, true);
            File.WriteAllText(LEADERBOARD_DATA_PATH, json);
        }

        public static LeaderBoard LoadLeaderBoard()
        {
            try
            {
                string json = File.ReadAllText(LEADERBOARD_DATA_PATH);
                return JsonUtility.FromJson<LeaderBoard>(json);
            }
            catch (FileNotFoundException)
            {
                return new LeaderBoard();
            }
        }

        public static void ClearLeaderBoard()
        {
            // Delete leaderboard.json
            if (File.Exists(LEADERBOARD_DATA_PATH))
            {
                File.Delete(LEADERBOARD_DATA_PATH);
            }
        }
    }
}