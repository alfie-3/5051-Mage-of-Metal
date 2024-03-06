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
        runeManager = LevelManager.runeManager.GetComponent<RuneFMODBridge>();


        //get player script
        GameObject playerObject = LevelManager.player;
        playerScript = playerObject.GetComponent<RuneTestPlayer>();

        //get point runes need to head towards
        CentrePoint = GameObject.FindGameObjectWithTag("Centre").transform;



    }

    // Start is called before the first frame update
    void Start()
    {
        //get main camera
        //cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>() as Camera;
        cam = Camera.main as Camera;
    }

    // Update is called once per frame
    void Update()
    {

        float step =  runeManager.SpeedOfRune * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, CentrePoint.position, step);
    }

    

}
