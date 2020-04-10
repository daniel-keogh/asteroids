using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class OptionsMenu : MonoBehaviour
{
    public void ClearLeaderBoard()
    {
        SaveSystem.ClearLeaderBoard();
    }
}
