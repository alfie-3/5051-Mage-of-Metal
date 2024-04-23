using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ScoreSaveManager
{
    const string fileName = "leaderboard.score";
    static Dictionary<string, int> leaderboardDict;

    public static Dictionary<string, int> RetrieveLeaderboard()
    {
        leaderboardDict ??= LoadScoreDict();

        return leaderboardDict;
    }

    public static void AddScore(string name, int score)
    {
        leaderboardDict ??= LoadScoreDict();

        if (leaderboardDict.ContainsKey(name))
        {
            leaderboardDict[name] = score;
        }
        else
            leaderboardDict.Add(name, score);

        SaveLeaderboard();
    }

    public static Dictionary<string, int> LoadScoreDict()
    {
        string path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + fileName;

        if (!File.Exists(path)) { return new(); }

        using StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();

        return JsonConvert.DeserializeObject<Dictionary<string, int>>(json);
    }

    public static void SaveLeaderboard()
    {
        leaderboardDict ??= LoadScoreDict();

        string path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + fileName;

        Debug.Log($"Saving scores at: {path}");

        string json = JsonConvert.SerializeObject(leaderboardDict);

        using StreamWriter writer = new StreamWriter(path);
        writer.Write(json);


        Debug.Log(json);
    }
}
