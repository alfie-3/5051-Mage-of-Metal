using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CanvasGenerator : MonoBehaviour
{
    //script that generates a canvas for each enemy - healthbar is child of canvas
    //serialized
    [SerializeField] GameObject EnemyCanvas;
    void Awake()
    {
        //gets all enemies
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //for each enemy
        foreach(GameObject Enemy in Enemies)
        {
            //generates canvas + makes enemy parent of canvas
            GameObject CurrentEnemyCanvas = Instantiate(EnemyCanvas);
            CurrentEnemyCanvas.transform.SetParent(Enemy.transform);
        }
    }
}
