using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] GameObject scoreSlotPrefab;

    [SerializeField] RectTransform topScores;
    [SerializeField] RectTransform bottomScore;

    public void GenerateLeaderBoard()
    {
        //Assigns leadboard values for the top players

        List<ScoreEntry> scoreEntries = new List<ScoreEntry>();
        Dictionary<string, int> scoreDict = ScoreSaveManager.RetrieveLeaderboard();

        foreach (RectTransform child in topScores)
        {
            Destroy(child.gameObject);
        }

        foreach (RectTransform child in bottomScore)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < scoreDict.Count; i++)
        {
            scoreEntries.Add(new(scoreDict.ElementAt(i).Key, scoreDict.ElementAt(i).Value));
        }

        scoreEntries = scoreEntries.OrderBy(x => x.score).ToList();
        scoreEntries.Reverse();

        for (int i = 0; i < 10; i++)
        {
            if (i > scoreEntries.Count() - 1)
                continue;

            LeaderboardSlot _slot = Instantiate(scoreSlotPrefab, topScores).GetComponent<LeaderboardSlot>();
            _slot.Init(scoreEntries[i].name, scoreEntries[i].score, i + 1, i == 0, scoreEntries[i].name == ScoreSubmissionManager.submittedName);

        }

        if (scoreEntries.Count > 10)
        {
            LeaderboardSlot _slot = Instantiate(scoreSlotPrefab, bottomScore).GetComponent<LeaderboardSlot>();
            _slot.Init(scoreEntries[^1].name, scoreEntries[^1].score, scoreEntries.Count + 1);
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync(1);
    }
}

public class ScoreEntry
{
    public int score;
    public string name;

    public ScoreEntry(string name, int score)
    {
        this.score = score;
        this.name = name;
    }
}
