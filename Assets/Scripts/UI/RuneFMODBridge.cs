using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//what needs to be done:
//at the moment runes will appear in order of array and not on beat


//new system for runes:
//instead of single rune going towards centre, needs modularity so multiple can
//runes need to be playable at the same time 
//current way = rune manager summons each rune one by one 
//change = rune manager needs to summon multiple runes towards the centre
//this needs further clarification in the system of dictating what runes  need to be played.
//need classification for which angle rune is coming from towards the centre - 5 lanes so north, west, east, south east and southwest
//function that decides which is the next direction + when to do multiple
//instead of having locations of rune spawnpoints as objects could just get their actual vector positions

public class RuneFMODBridge : MonoBehaviour
{
    [Header("Rune Order")]
    [TextArea]
    public string Rune_Instructions = "Type in the Runes Text box the order of which the runes should appear, B (Blue), O ( Orange), Y (Yellow), G (Green), R (Red) ";

    [Header("Rune Sprites")]
    [TextArea]
    public string Rune_Sprites_Info = "These are temporary placeholders, to be replaced when rune sprites designed";

    [SerializeField] GameObject BlueRune;
    [SerializeField] GameObject OrangeRune;
    [SerializeField] GameObject YellowRune;
    [SerializeField] GameObject GreenRune;
    [SerializeField] GameObject RedRune;

    Vector3 NorthSpawnpoint;
    Vector3 EastSpawnpoint;
    Vector3 WestSpawnpoint;
    Vector3 SouthEastSpawnpoint;
    Vector3 SouthWestSpawnpoint;
    //centre = Vector3(7344.7998,3658.19995,0)

    private GameObject centre;

    [SerializeField] GameObject HUDCanvas;
    public GameObject CurrentRune;
    public float SpeedOfRune = 1.0f;
    public float runeTimer = 3.0f;

    //north 0, west 1, south west 2, south east 3, east 4
    int currentDirection = 0;

    void Awake()
    {
        if (!centre)
        {
            centre = GameObject.FindGameObjectWithTag("Centre");
        }
        if (!centre)
        {
            Debug.Log("Centre not found");
        }

    }

    public void SpawnNote(string noteName)
    {
        RuneLocations();
        switch (noteName)
        {
            case "1":
                CurrentRune = Instantiate(GreenRune, NorthSpawnpoint, Quaternion.identity, centre.transform);
                break;
            case "2":
                CurrentRune = Instantiate(RedRune, NorthSpawnpoint, Quaternion.identity, centre.transform);
                break;
            case "3":
                CurrentRune = Instantiate(YellowRune, NorthSpawnpoint, Quaternion.identity, centre.transform);
                break;
            case "4":
                CurrentRune = Instantiate(BlueRune, NorthSpawnpoint, Quaternion.identity, centre.transform);
                break;
            case "5":
                CurrentRune = Instantiate(OrangeRune, NorthSpawnpoint, Quaternion.identity, centre.transform);
                break;
            default:
                Debug.Log("Note Name Not Found!");
                break;
        }
    }


    void RuneLocations()
    {
        NorthSpawnpoint = centre.transform.position + new Vector3(0.0f, 50.0f, 0.0f);
        EastSpawnpoint = centre.transform.position + new Vector3(50.0f, 0.0f, 0.0f);
        WestSpawnpoint = centre.transform.position + new Vector3(50.0f, 0.0f, 0.0f);
        SouthEastSpawnpoint = centre.transform.position + new Vector3(-30.0f, 30.0f, 0.0f);
        SouthWestSpawnpoint = centre.transform.position + new Vector3(30.0f, 30.0f, 0.0f);
    }

}
