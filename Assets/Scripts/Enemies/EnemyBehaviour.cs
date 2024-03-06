using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enemy logic for health to test health display


//system that randomly generates points along the spline
//same script gets enemies within range and shoves em into a list inside a dictionary
//enemy script movement towards player set on a bool
// if player reaches point(trigger) script sets all enemies grouped in the list to true and they start running at the player 
//not sure if this is what we need though

//alternatively - enemies set to always be running at player 
//spawn points set within level and when player gets to certain points on spline enemies spawn in from spawn points - enemies have navmeshes to navigate surroundings


public class EnemyBehaviour : MonoBehaviour, IDamage {
    //public
    public int EnemyHP = 10;
    public int EnemyMaxHP = 10;
    public Vector3 Offset;
    [TextArea]
    public string UIInfo = "Make sure UILocation game object is linked in the field below";
    //serialized
    [SerializeField] private GameObject DieEffect;
    [SerializeField] private Transform UILocation;

    public Action OnDeath = delegate { };

    void Awake() 
    {
        //checks each child to find ui location point
        foreach (Transform child in gameObject.transform)
        {
            if (child.tag == "UIReference") {
                UILocation = child;
            }
            else{
                Debug.Log("UI Location n" +
                    "ot found");
            }
        }
        //offset for ui
        Offset = UILocation.localPosition * 80;
    }
    void Start() {
        //set health to max
        EnemyHP = EnemyMaxHP;
    }

//function that damages enemy when called
    public void Damage(int damage)
    {
        EnemyHP -= damage;
        if (EnemyHP <= 0)
        {
            Kill();
        }
    }

//if enemy dies
    private void Kill()
    {
        //special effect
        OnDeath.Invoke();
        Debug.Log("Enemy Killed"); //debug log for testing
        Destroy(gameObject); //destroy enemy
    }


//function for testing healthbar
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
