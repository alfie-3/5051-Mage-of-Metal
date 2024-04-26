//Interfaces for both damage and score systems

using UnityEngine;

public interface IDamage
{
    GameObject gameObject { get; }
    public void Damage(int damage);
}
