using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyRune : MonoBehaviour
{
    private Vector3 Offset;

    private Camera cam;
    private RectTransform rectTransform;

    private GameObject Enemy;

    private EnemyBehaviour enemyScript;

    // Start is called before the first frame update
    void Start()
    {
        //this only works if the hierarchy makes the enemy object the 2nd parent above
        Enemy = gameObject.transform.parent.parent.gameObject;
        enemyScript = Enemy.GetComponent<EnemyBehaviour>();
        Offset = enemyScript.Offset;
        rectTransform = GetComponent<RectTransform>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>() as Camera;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPosition = cam.WorldToScreenPoint(Enemy.transform.position)+ Offset;

        // Set the UI object's position to the screen coordinates
        rectTransform.position = screenPosition;
    }
}
