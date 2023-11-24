using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enemy logic for health to test health display

public class EnemyBehaviour : MonoBehaviour {
    public int EnemyHP = 10;
    public int EnemyMaxHP = 10;

    void Start() {
        //set health to max
        EnemyHP = EnemyMaxHP;
        StartCoroutine(DieEnemy());

    }

    void Update() {
    }
    IEnumerator DieEnemy() {
        while (EnemyHP != 0)
        {
            yield return new WaitForSeconds(1f);
            EnemyHP = EnemyHP - 1; // testing healthbar 
            print(EnemyHP);
        }

    }
}
