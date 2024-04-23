using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Globalization;
using System.Xml;
using System;
using UnityEngine.SceneManagement;

public class EndScreenLeaderboard : MonoBehaviour
{
    [SerializeField] GameObject scoreSlotPrefab;

    [SerializeField] RectTransform topScores;
    [SerializeField] RectTransform bottomScore;

    public void GenerateLeaderBoard()
    {
        //Assigns leadboard values for the top players

        List<ScoreEntry> scoreEntries = new List<ScoreEntry>();
        Dictionary<string, int> scoreDict = ScoreSaveManager.RetrieveLeaderboard();

        int yourScoreRank = 0;

        for (int i = 0; i < scoreDict.Count; i++)
        {
            scoreEntries.Add(new(scoreDict.ElementAt(i).Key, scoreDict.ElementAt(i).Value));

            if (scoreDict.ElementAt(i).Key == ScoreSubmissionManager.submittedName) yourScoreRank = i;
        }

        scoreEntries = scoreEntries.OrderBy(x => x.score).ToList();
        scoreEntries.Reverse();

        bool showedYou = false;

        for (int i = 0; i < 10; i++)
        {
            if (i > scoreEntries.Count() - 1)
                continue;

            LeaderboardSlot _slot = Instantiate(scoreSlotPrefab, topScores).GetComponent<LeaderboardSlot>();
            _slot.Init(scoreEntries[i].name, scoreEntries[i].score, i + 1, i == 0, scoreEntries[i].name == ScoreSubmissionManager.submittedName);

            if (scoreEntries[i].name == ScoreSubmissionManager.submittedName) showedYou = true;
        }

        if (!showedYou)
        {
            LeaderboardSlot _slot = Instantiate(scoreSlotPrefab, bottomScore).GetComponent<LeaderboardSlot>();
            _slot.Init(scoreEntries[yourScoreRank].name, scoreEntries[yourScoreRank].score, yourScoreRank + 1, yourScoreRank == 0, scoreEntries[yourScoreRank].name == ScoreSubmissionManager.submittedName);
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync(1);
    }
}