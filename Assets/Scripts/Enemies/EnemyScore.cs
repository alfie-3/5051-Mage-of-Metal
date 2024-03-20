using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyBehaviour))]
public class EnemyScore : MonoBehaviour
{
    [SerializeField] int score;

    public void OnEnable()
    {
        GetComponent<EnemyBehaviour>().OnDeath += IncreaseScore;
    }

    public void OnDisable()
    {
        GetComponent<EnemyBehaviour>().OnDeath -= IncreaseScore;
    }

    public void IncreaseScore()
    {
        ScoreCounter.AddScore(score);
    }
}
