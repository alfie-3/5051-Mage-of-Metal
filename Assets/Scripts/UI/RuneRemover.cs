using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneRemover : MonoBehaviour
{
    //temp script that gets rid of runes that collide with centre - this script is on an object with a collider
    RuneFMODBridge runeManager;
    //centre point we're checking for collisions
    Transform centre;
    // Start is called before the first frame update
    void Start()
    {
        //get relevant objects
        GameObject managerObject = GameObject.FindGameObjectWithTag("Manager");
        runeManager = managerObject.GetComponent<RuneFMODBridge>();
        centre = GameObject.FindGameObjectWithTag("Centre").transform;
    }

    void Update()
    {
        //move current object to centre
        gameObject.transform.position = centre.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        //if a rune collides
        if (other.gameObject.tag == "Rune")
        {
            //yeet it
            //runeManager.RemoveRune(other.gameObject);
        }
    }
}
