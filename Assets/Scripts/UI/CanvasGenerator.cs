using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

//what this script needs to do:
//create a rune, healthbar and canvas for every enemy
//have vectors that correlate to the locations the healthbars and runes should be depening on the enemy
//eyeclops healthbar + rune - Y197.5
//gobblo healtbar - Y109.5 - rune - Y110.8
//tiny eye guy - hp - Y36.5 - rune - 37.48

//steps: (needs to happen on awake)
//get all enemies - shove in array
//for each enemy give them canvas prefab
//then on that canvas add the healthbar and rune at the vector offset

//check enemy name, depending on name set different offset


public class CanvasGenerator : MonoBehaviour
{
    [SerializeField] GameObject EnemyCanvas;
    void Awake()
    {
        //give each enemy a canvas prefab - canvas already has healthbar and rune on it
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject Enemy in Enemies)
        {
            GameObject CurrentEnemyCanvas = Instantiate(EnemyCanvas);
            CurrentEnemyCanvas.transform.SetParent(Enemy.transform);
        }
    }
}
