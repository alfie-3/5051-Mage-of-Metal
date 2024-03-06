using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GameData
{
    public SerialisableDictionary<string, int> playerScores;

    public GameData()
    {
        this.playerScores = new SerialisableDictionary<string, int>();
    }

}
