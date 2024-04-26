//Script for any non-enemy gameobjects that gives the player points
using UnityEngine;

public class BonusPointItems : MonoBehaviour, IDamage
{
    [Header("Item score variables")]
    [SerializeField] int scoreAmount=50;
    [SerializeField] float scoreMultiplierIncrease=0.1f;

    [Space]
    [Header("Materials")]
    [SerializeField] Texture2D mainTexture;

    public void Damage(int damage)
    {
        LevelManager.player.GetComponent<IScore>().AddScore(scoreMultiplierIncrease, scoreAmount);
    }
}