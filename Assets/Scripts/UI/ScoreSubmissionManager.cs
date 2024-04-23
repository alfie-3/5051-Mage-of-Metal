using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreSubmissionManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TMP_InputField nameInput;

    private void Start()
    {
        scoreText.text = PlayerStats.playerScore.ToString();
    }

    public void SubmitScore(string name)
    {
        if (name == string.Empty)
            return;

        ScoreSaveManager.AddScore(name, PlayerStats.playerScore);
        PlayerStats.playerScore = 0;
    }
}
