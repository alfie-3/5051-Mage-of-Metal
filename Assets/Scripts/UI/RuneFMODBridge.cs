    using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//what needs to be done:
//at the moment runes will appear in order of array and not on beat


//new system for runes:
//runes need to be playable at the same time 
//need classification for which angle rune is coming from towards the centre - 5 lanes so north, west, east, south east and southwest
//function that decides which is the next direction + when to do multiple
//instead of having locations of rune spawnpoints as objects could just get their actual vector positions

public class RuneFMODBridge : MonoBehaviour
{
    //inspector display stuff
    [Header("Rune Order")]
    [TextArea]
    public string Rune_Instructions = "Type in the Runes Text box the order of which the runes should appear, B (Blue), O ( Orange), Y (Yellow), G (Green), R (Red) ";

    [Header("Rune Sprites")]
    [TextArea]
    public string Rune_Sprites_Info = "These are temporary placeholders, to be replaced when rune sprites designed";

//serialized
    [SerializeField] GameObject BlueRunePrefab;
    [SerializeField] GameObject OrangeRunePrefab;
    [SerializeField] GameObject YellowRunePrefab;
    [SerializeField] GameObject GreenRunePrefab;
    [SerializeField] GameObject RedRunePrefab;

    [SerializeField] GameObject HUDCanvas;
    [SerializeField] GameObject RuneLimit;
    [SerializeField] int ColliderSize;
    [SerializeField] float DistanceFromCentre = 50.0f;
//public
    public float SpeedOfRune = 1.0f;
    public List<GameObject>  RunesInScene = new List<GameObject>();

    Vector3 NorthSpawnpoint;
    Vector3 EastSpawnpoint;
    Vector3 WestSpawnpoint;
    Vector3 SouthEastSpawnpoint;
    Vector3 SouthWestSpawnpoint;
    //centre = Vector3(7344.7998,3658.19995,0)

//private
    private GameObject centre;
    int currentDirection = 0;

    

    void Awake()
    {
        centre = GameObject.FindGameObjectWithTag("Centre");
        BoxCollider RuneLimitCollider = RuneLimit.GetComponent<BoxCollider>();
        RuneLimitCollider.size = new Vector3(ColliderSize, ColliderSize, 3);

    }

    public void SpawnNote(string noteName)
    {
        RuneLocations();
        switch (noteName)
        {
            case "1":
                RunesInScene.Add(Instantiate(GreenRunePrefab, NorthSpawnpoint, Quaternion.identity, centre.transform));
                break;
            case "2":
                RunesInScene.Add(Instantiate(RedRunePrefab, EastSpawnpoint, Quaternion.identity, centre.transform));
                break;
            case "3":
                RunesInScene.Add(Instantiate(YellowRunePrefab, SouthEastSpawnpoint, Quaternion.identity, centre.transform));
                break;
            case "4":
                RunesInScene.Add(Instantiate(BlueRunePrefab, WestSpawnpoint, Quaternion.identity, centre.transform));
                break;
            case "5":
                RunesInScene.Add(Instantiate(OrangeRunePrefab, SouthWestSpawnpoint, Quaternion.identity, centre.transform));
                break;
            default:
                Debug.Log("Note Name Not Found!");
                break;
        }
    }


//gets spawn points of runes relative to centre
    void RuneLocations()
    {
        NorthSpawnpoint = centre.transform.position + new Vector3(0.0f, DistanceFromCentre, 0.0f);
        EastSpawnpoint = centre.transform.position + new Vector3(DistanceFromCentre, 0.0f, 0.0f);
        WestSpawnpoint = centre.transform.position + new Vector3(-DistanceFromCentre, 0.0f, 0.0f);
        SouthEastSpawnpoint = centre.transform.position + new Vector3(-DistanceFromCentre/2, DistanceFromCentre, 0.0f);
        SouthWestSpawnpoint = centre.transform.position + new Vector3(DistanceFromCentre, DistanceFromCentre, 0.0f);
    }

///remove rune 
    public void RemoveRune(GameObject rune)
    {
        if(rune)
        {
            RunesInScene.Remove(rune);
            Destroy(rune);
        }
    }

}
