using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;


public class EnemyHealthBar : MonoBehaviour {

    //Script for the Healthbars on the enemies - script is on slider
    [Header("Slider Assets")]
    [SerializeField] private Slider bar;
    [SerializeField] private Image barImage;
    //[SerializeField] private GameObject barObject;
    private GameObject Enemy; 

    //other needed objects
    private Vector3 Offset;
    private Camera cam;
    private RectTransform rectTransform;
    private Transform UILocation;
    private EnemyBehaviour enemyScript;

    private void Start() {
        //activate healthbar
        gameObject.SetActive(true);
        
        //make sure hierarchy is correct before setting enemy
        if(gameObject.transform.parent.parent.tag == "Enemy")
        {
            Enemy = gameObject.transform.parent.parent.gameObject;
        }
        else
        {
            Debug.Log("The parent of the parent needs to be the enemy, the current parent of the parent is: " + gameObject.transform.parent.parent.name);
        }

        //get the other needed things
        enemyScript = Enemy.GetComponent<EnemyBehaviour>();
        rectTransform = GetComponent<RectTransform>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>() as Camera;
        Offset = enemyScript.Offset; //set offset to  whatever its set in the enemies script

        //if slider bar and enemy script are present, set value
        if(bar && enemyScript)
        {
            bar.maxValue = enemyScript.EnemyHP;
        }
        //otherwise return error
        else if(!bar)
        {
            Debug.Log("bar not found");
        }
        else if(!enemyScript)
        {
            Debug.Log("script not found");
        }
        
    }

    // Update is called once per frame
    void Update() {
        
		// convert game to screen pos
        Vector3 screenPosition = cam.WorldToScreenPoint(Enemy.transform.position)+ Offset;

        // set the sliders position
        rectTransform.position = screenPosition;

        //if health below a certain point change its color
        if(enemyScript.EnemyHP > 5) {
            barImage.color = Color.green;
        }
        else if (enemyScript.EnemyHP < 5) {
            barImage.color = Color.red;
        }
        else if (enemyScript.EnemyHP < 5) {
            barImage.color = Color.red;
        }
        //if enemy dies disable healthbar
        else  if(enemyScript.EnemyHP == 0) {
            gameObject.SetActive(false);
        }
        //set slider to health
        bar.value = enemyScript.EnemyHP;
    }

}
