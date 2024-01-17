using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;


public class EnemyHealthBar : MonoBehaviour {
    [SerializeField] private Slider bar;
    [SerializeField] private Image barImage;
    //[SerializeField] private GameObject barObject;
    private GameObject Enemy; 
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
            Debug.Log("The parent needs to be the enemy, the current parent is: " + gameObject.transform.parent.parent.name);
        }

        enemyScript = Enemy.GetComponent<EnemyBehaviour>();
        rectTransform = GetComponent<RectTransform>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>() as Camera;
        Offset = enemyScript.Offset;
        if(bar && enemyScript)
        {
            bar.maxValue = enemyScript.EnemyHP;
        }
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
        
		// Convert the in-game object's position to screen coordinates
        Vector3 screenPosition = cam.WorldToScreenPoint(Enemy.transform.position)+ Offset;

        // Set the UI object's position to the screen coordinates
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
