using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enemy logic for health to test health display

public class EnemyBehaviour : MonoBehaviour, IDamage {
    public int EnemyHP = 10;
    public int EnemyMaxHP = 10;
    public Vector3 Offset;
    [TextArea]
    public string UIInfo = "Make sure UILocation game object is linked in the field below";
    [SerializeField] GameObject DieEffect;
    [SerializeField] private Transform UILocation;

    void Awake() 
    {
        Offset = UILocation.localPosition * 80;
    }
    void Start() {
        //set health to max
        EnemyHP = EnemyMaxHP;
    }

    public void Damage(int damage)
    {
        EnemyHP -= damage;

        if (EnemyHP <= 0)
        {
            Kill();
        }
    }

    private void Kill()
    {
        GameObject effect = Instantiate(DieEffect, gameObject.transform);
        Destroy(effect, 0.3f);
        Debug.Log("Enemy Killed");
        Destroy(gameObject, 0.3f);
    }



    /*
    IEnumerator DieEnemy() {
        while (EnemyHP != 0)
        {
            yield return new WaitForSeconds(1f);
            EnemyHP = EnemyHP - 1; // testing healthbar 
            print(EnemyHP);
        }

    }
    */
}
