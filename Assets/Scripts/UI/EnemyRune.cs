using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyRune : MonoBehaviour
{

    // this needs to lerp towards the centre point 
    // also fade in time - timer
    // if player plays certain key they disappear
 

    private RuneManager managerScript; 
    private RuneTestPlayer playerScript;
    private Camera cam;
    private float runeTimer;
    private Transform CentrePoint;

    void Awake()
    {
        GameObject managerObject = GameObject.FindGameObjectWithTag("Manager");
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        managerScript = managerObject.GetComponent<RuneManager>();
        playerScript = playerObject.GetComponent<RuneTestPlayer>();
        runeTimer = managerScript.runeTimer;
        CentrePoint = GameObject.FindGameObjectWithTag("Centre").transform;
    }

    // Start is called before the first frame update
    void Start()
    {

        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>() as Camera;
    }

    // Update is called once per frame
    void Update()
    {
        RuneLifespan();

        float step =  managerScript.SpeedOfRune * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, CentrePoint.position, step);
    }

    void RuneLifespan()
    {
        runeTimer -= Time.deltaTime;
          if(playerScript.runePlayed == true)
            {
                managerScript.UpdateRune();
                playerScript.runePlayed = false;
                Destroy(gameObject);
            }
            
        else if(runeTimer <= 0.0f)
        {
            managerScript.UpdateRune(); 
            Destroy(gameObject);

        }
    }

}
