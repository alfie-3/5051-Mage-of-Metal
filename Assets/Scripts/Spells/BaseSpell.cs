using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseSpell : MonoBehaviour
{
    private Vector3 startPos;
    private Transform target;
    private int spellDamage;
    private float delta01=0;
    [SerializeField] float speed=1;
    private bool start=false;

    public virtual void OnStart(Transform enemy, Vector3 position, int damage) { target = enemy; start = true; startPos = position; spellDamage = damage; }
    public virtual void OnHit() { if (target != null) { target.GetComponent<IDamage>().Damage(spellDamage); }; }
    public virtual void Update() {
        if (start)
        {
            if (target == null)
            {
                OnHit();
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
