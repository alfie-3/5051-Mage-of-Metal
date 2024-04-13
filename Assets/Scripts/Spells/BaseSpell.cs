using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

abstract class BaseSpell : MonoBehaviour
{
    public Vector3 startPos;
    public Transform target;
    private float delta01=0;
    public float speed=1;

    public virtual void OnStart() { }
    public virtual void OnHit() { }
    public virtual void Update() {
        delta01 += Time.deltaTime;
        transform.position = ((target.position - startPos) * (delta01*speed)) + startPos;
        if (delta01 >= 1)
        {
            OnHit();
        }
    }

}
