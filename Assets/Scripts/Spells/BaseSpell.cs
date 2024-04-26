//Base inheritance script for spells that target and damage enemies

using UnityEngine;

public class BaseSpell : MonoBehaviour
{
    [Header("Spell properties")]
    private Vector3 startPos;
    private int spellDamage;
    private float delta01 = 0;
    [SerializeField] float speed = 1;
    private bool hasStarted = false;

    [Header("Enemy links")]
    private Transform target;
    IDamage enemyIDamage;


    //Spawned spell targets enemy and gets initial start position
    public virtual void OnStart(Transform enemy, Vector3 position, int damage, IDamage iDam) { target = enemy; enemyIDamage = iDam; hasStarted = true; startPos = position; spellDamage = damage; }

    //Events that happens when enemy is struck, gameobject is then destroyed
    public virtual void OnHit() { if (target != null) { enemyIDamage.Damage(spellDamage); hasStarted = false; delta01 = 0; gameObject.SetActive(false); }  }

    //Tracking spell behaviour
    public virtual void Update() {
        if (hasStarted)
        {
            if (target == null)
            {
                Destroy(gameObject);
            }
            delta01 += Time.deltaTime * speed * 2;
            transform.position = ((target.position - startPos + new Vector3(0,0.7f,0)) * (delta01 * speed)) + startPos;
            if (delta01 >= 1)
            {
                OnHit();
            }
        }
    }
}
