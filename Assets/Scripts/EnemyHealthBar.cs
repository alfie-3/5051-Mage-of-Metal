using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemyHealthBar : MonoBehaviour {
    [SerializeField] private Slider bar;
    [SerializeField] private Image barImage;
    //[SerializeField] private GameObject barObject;
    private GameObject Enemy; 
    [SerializeField] private Vector3 Offset;

    private Camera cam;
    private RectTransform rectTransform;


    private EnemyBehaviour enemyScript;

    private void Start() {
        //activate healthbar
        gameObject.SetActive(true);

        Enemy = gameObject.transform.parent.parent.gameObject;
        enemyScript = Enemy.GetComponent<EnemyBehaviour>();
        rectTransform = GetComponent<RectTransform>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>() as Camera;
        bar.maxValue = enemyScript.EnemyHP;
        
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
