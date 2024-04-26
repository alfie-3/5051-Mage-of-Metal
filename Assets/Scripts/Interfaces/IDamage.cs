//Interfaces for both damage and score systems

using UnityEngine;

public interface IDamage
{
    GameObject gameObject { get; }
    public void Damage(int damage);
}
public interface IScore
{
    public void AddScore(float scoreMult, int score);
    public void DamageScore(float scoreMult, Color vignCol, float vignTime);
}
