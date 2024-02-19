using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyRune : MonoBehaviour
{

    //script of runes that appear in game

    //variables
    private RuneFMODBridge managerScript; 
    private RuneTestPlayer playerScript;
    private Camera cam;
    private float runeTimer;
    private Transform CentrePoint;

    void Awake()
    {
        //get manager script
        GameObject managerObject = GameObject.FindGameObjectWithTag("Manager");
        managerScript = managerObject.GetComponent<RuneFMODBridge>();

        //get player script
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        playerScript = playerObject.GetComponent<RuneTestPlayer>();

        //get point runes need to head towards
        CentrePoint = GameObject.FindGameObjectWithTag("Centre").transform;

        //rune lifespan - as defined by manager
        runeTimer = managerScript.runeTimer;


    }

    // Start is called before the first frame update
    void Start()
    {
        //get main camera
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
                playerScript.runePlayed = false;
                Destroy(gameObject);
            }
            
        else if(runeTimer <= 0.0f)
        {
            Destroy(gameObject);

        }
    }

}
