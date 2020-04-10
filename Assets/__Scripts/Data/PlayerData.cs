using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class PlayerData : IComparable<PlayerData>
    {
        public string name;
        public int score;

        public int CompareTo(PlayerData other)
        {
            // Sort PlayerData objects by the value of `score`
            return this.score.CompareTo(other.score);
        }

        public override string ToString()
        {
            return "[PlayerData] " + name + ", " + score;
        }
    }

    [Serializable]
    public class LeaderBoard
    {
        public PlayerData[] players;
    }
}
