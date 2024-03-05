using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneRemover : MonoBehaviour
{
    RuneFMODBridge runeManager;
    Transform centre;
    // Start is called before the first frame update
    void Start()
    {
        GameObject managerObject = GameObject.FindGameObjectWithTag("Manager");
        runeManager = managerObject.GetComponent<RuneFMODBridge>();
        centre = GameObject.FindGameObjectWithTag("Centre").transform;
    }

    void Update()
    {
        gameObject.transform.position = centre.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Rune")
        {
            runeManager.RemoveRune(other.gameObject);
        }
    }
}
