using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyRune : MonoBehaviour
{

    //script of runes that appear in game

    //variables
    private RuneFMODBridge runeManager; 
    private RuneTestPlayer playerScript;
    private Camera cam;
    private float runeTimer;
    private Transform CentrePoint;


    void Awake()
    {
        //get manager script
        GameObject managerObject = GameObject.FindGameObjectWithTag("Manager");
        runeManager = managerObject.GetComponent<RuneFMODBridge>();

        //get player script
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        playerScript = playerObject.GetComponent<RuneTestPlayer>();

        //get point runes need to head towards
        CentrePoint = GameObject.FindGameObjectWithTag("Centre").transform;



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

        float step =  runeManager.SpeedOfRune * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, CentrePoint.position, step);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (GetComponent<Collider>().gameObject.tag == "RuneLimit")
        {
            runeManager.RemoveRune(gameObject);
        }
        Debug.Log("collided");
    }

}
