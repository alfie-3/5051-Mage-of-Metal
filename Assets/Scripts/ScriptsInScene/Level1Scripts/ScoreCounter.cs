using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    static public ScoreCounter current;

    public int score;
    [SerializeField] TextMeshProUGUI tmproUGUI;

    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }
        else
        {
            Destroy(gameObject);
        }

        tmproUGUI.text = "Score: 000000";
    }

    public static void AddScore(int _score)
    {
        if (current == null) { return; }
        else current.SetScore(_score);
    }

    public void SetScore(int _score)
    {
        score += _score;
        tmproUGUI.text = $"Score: {score:000000}";
    }

    public void SubmitScore()
    {

    }

    public void ResetScore()
    {
        score = 0;
    }
}
