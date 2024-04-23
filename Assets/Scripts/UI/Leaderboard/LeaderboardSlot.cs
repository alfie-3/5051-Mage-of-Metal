using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardSlot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI rank;
    [SerializeField] TextMeshProUGUI nameAndScore;
    [Space]
    [SerializeField] TextMeshProUGUI you;
    [SerializeField] Image crown;

    public void Init(string name, int score, int rank, bool showCrown = false, bool showYou = false)
    {
        nameAndScore.text = $"{name} - {score}";
        this.rank.text = $"#{rank}";

        crown.enabled = showCrown;
        you.enabled = showYou;
    }
}