using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour, IDataPersistence
{
    static SerialisableDictionary<string, int> scores;

    static public void SubmitScore(string name = "User1", int score = 0)
    {
        ScoreManager.SubmitScore(name, score);
    }

    public void LoadData(GameData data)
    {
        scores = data.playerScores;
    }

    public void SaveData(ref GameData data)
    {
        data.playerScores = scores;
    }
}
