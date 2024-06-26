using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;
using WiimoteApi;

public class EnemyRune : MonoBehaviour
{

    //script of runes that appear in game

    //variables
    private RuneFMODBridge runeManager;/*
    private RuneTestPlayer playerScript;
    private Camera cam;
    private float runeTimer;*/

    private Vector3 velocity;
    private Vector3 direction;
    private Controls _controlsKnm;
    InputAction controls;
    private bool isActive = false;



    void Awake()
    {
        //get manager script
        runeManager = LevelManager.runeManager.GetComponent<RuneFMODBridge>();
        _controlsKnm = new Controls();


        //get player script
        GameObject playerObject = LevelManager.player;
        //playerScript = playerObject.GetComponent<RuneTestPlayer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //get main camera
        //cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>() as Camera;
        //cam = Camera.main as Camera;

        direction = LevelManager.pointer.transform.position - transform.position;
        velocity = (direction - (direction.normalized*105) * ((60 / LevelManager.bpm) * LevelManager.beatLeadUp));
    }
    private void OnEnable()
    {
        controls = _controlsKnm.GuitarControls.Strum;
        WiiInputManager.GuitarWiiMote.Strummed += DestroyRune;
        _controlsKnm.GuitarControls.Strum.performed += OnStrum;
        _controlsKnm.GuitarControls.Strum.Enable();

    }
    private void OnDisable()
    {
        WiiInputManager.GuitarWiiMote.Strummed -= DestroyRune;
        _controlsKnm.GuitarControls.Strum.performed -= OnStrum;
        _controlsKnm.GuitarControls.Strum.Disable();
    }

    void DestroyRune()
    {
        if (isActive)
        {
            Destroy(gameObject);
        }
        else
        {
            print("nyo >:(");
        }
    }

    void OnStrum(InputAction.CallbackContext obj)
    {
        DestroyRune();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        float step =  runeManager.SpeedOfRune * Time.deltaTime; // calculate distance to move
        Debug.Log(step);
        transform.position = Vector3.MoveTowards(transform.position, LevelManager.pointer.transform.position, step);
        */

        // v=d/t (60/Bpm) * beatnum
        //0.375
        // v = distance / (
        // d = vt
        // distance = velocity

        transform.position += velocity * Time.deltaTime;
        //print("TTTTTTTTTT");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Pointer")
        {
            Destroy(gameObject);
        }
        else if (collision.name == "Rune Holder")
        {
            Debug.Log("Yes");
            isActive = true;
        }
    }

}
