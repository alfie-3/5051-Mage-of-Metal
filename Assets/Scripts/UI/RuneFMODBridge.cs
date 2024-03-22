    using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


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
    [Header("Prefabs")]
    [SerializeField] GameObject BlueRunePrefab;
    [SerializeField] GameObject OrangeRunePrefab;
    [SerializeField] GameObject YellowRunePrefab;
    [SerializeField] GameObject GreenRunePrefab;
    [SerializeField] GameObject RedRunePrefab;

    [Header("Holders")]
    [SerializeField] GameObject BlueRuneHolder;
    [SerializeField] GameObject OrangeRuneHolder;
    [SerializeField] GameObject YellowRuneHolder;
    [SerializeField] GameObject GreenRuneHolder;
    [SerializeField] GameObject RedRuneHolder;

    [SerializeField] GameObject RuneLimit;
    [SerializeField] int ColliderSize;
    [SerializeField] float DistanceFromCentre = 50.0f;
//public
    public float SpeedOfRune = 1.0f;
    public List<GameObject>  RunesInScene = new List<GameObject>();

    Vector3 GreenSpawnpoint;
    Vector3 RedSpawnpoint;
    Vector3 BlueSpawnpoint;
    Vector3 YellowSpawnpoint;
    Vector3 OrangeSpawnpoint;
    //centre = Vector3(7344.7998,3658.19995,0)

    //private
    int currentDirection = 0;

    

    void Awake()
    {
        BoxCollider RuneLimitCollider = RuneLimit.GetComponent<BoxCollider>();
        RuneLimitCollider.size = new Vector3(ColliderSize, ColliderSize, 3);

    }

    public void SpawnNote(string noteName)
    {
        RuneLocations();
        switch (noteName)
        {
            case "1":
                RunesInScene.Add(Instantiate(GreenRunePrefab, GreenSpawnpoint, Quaternion.identity, LevelManager.pointer.transform));
                break;
            case "2":
                RunesInScene.Add(Instantiate(RedRunePrefab, RedSpawnpoint, Quaternion.identity, LevelManager.pointer.transform));
                break;
            case "3":
                RunesInScene.Add(Instantiate(YellowRunePrefab, YellowSpawnpoint, Quaternion.identity, LevelManager.pointer.transform));
                break;
            case "4":
                RunesInScene.Add(Instantiate(BlueRunePrefab, BlueSpawnpoint, Quaternion.identity, LevelManager.pointer.transform));
                break;
            case "5":
                RunesInScene.Add(Instantiate(OrangeRunePrefab, OrangeSpawnpoint, Quaternion.identity, LevelManager.pointer.transform));
                break;
            default:
                Debug.Log("Note Name Not Found!");
                break;
        }
    }


//gets spawn points of runes relative to centre
    void RuneLocations()
    {
        Vector3 pointerLoc = LevelManager.pointer.transform.position;

        GreenSpawnpoint = ((GreenRuneHolder.transform.position - pointerLoc).normalized * DistanceFromCentre) + pointerLoc;
        RedSpawnpoint = ((RedRuneHolder.transform.position - pointerLoc).normalized * DistanceFromCentre) + pointerLoc;
        YellowSpawnpoint = ((YellowRuneHolder.transform.position - pointerLoc).normalized * DistanceFromCentre) + pointerLoc;
        BlueSpawnpoint = ((BlueRuneHolder.transform.position - pointerLoc).normalized * DistanceFromCentre) + pointerLoc;
        OrangeSpawnpoint = ((OrangeRuneHolder.transform.position - pointerLoc).normalized * DistanceFromCentre) + pointerLoc;
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
