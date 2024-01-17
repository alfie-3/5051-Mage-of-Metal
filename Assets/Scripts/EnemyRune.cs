using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyRune : MonoBehaviour
{

    // this needs to lerp towards the centre point 
    // also fade in time - timer
    // if player plays certain key they disappear
 

    public RuneManager managerScript; 
    private Camera cam;
    private float runeTimer;
    private Transform CentrePoint;

    void Awake()
    {
        GameObject managerObject = GameObject.FindGameObjectWithTag("Manager");
        managerScript = managerObject.GetComponent<RuneManager>();
        runeTimer = managerScript.runeTimer;
        CentrePoint = GameObject.FindGameObjectWithTag("Centre");
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

    }

    void RuneLifespan()
    {
        runeTimer -= Time.deltaTime;

        if(runeTimer <= 0.0f)
        {
            Destroy(gameObject);
            //on destroy assign new rune to be focus rune in runemanager    
        }
    }

}
