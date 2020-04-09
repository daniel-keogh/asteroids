using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LeaderBoardItem : MonoBehaviour
{
    public void WriteText(string text)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = text;
    }
}
