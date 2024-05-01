using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//OLD RUNE MANAGER SCRIPT - USE RUNEFMODBRIDGE

public class RuneManager : MonoBehaviour
{
    [Header("Runes In Scene")]
    public List<GameObject> Runes = new List<GameObject>();
    [Header("Rune Order")]
    [TextArea]
    public string Rune_Instructions = "Type in the Runes Text box the order of which the runes should appear, B (Blue), O ( Orange), Y (Yellow), G (Green), R (Red) ";
    
    [SerializeField] string RunesText;

    [Header("Rune Sprites")]
    [TextArea]
    public string Rune_Sprites_Info = "These are temporary placeholders, to be replaced when rune sprites designed";

    [SerializeField] GameObject BlueRune;
    [SerializeField] GameObject OrangeRune;
    [SerializeField] GameObject YellowRune;
    [SerializeField] GameObject GreenRune;
    [SerializeField] GameObject RedRune; 

    //Rune Spawnpoints
    Vector3 NorthSpawnpoint;
    Vector3 EastSpawnpoint;
    Vector3 WestSpawnpoint;
    Vector3 SouthEastSpawnpoint;
    Vector3 SouthWestSpawnpoint;
    //centre = Vector3(7344.7998,3658.19995,0)
    
    //where runes are headed towards
    private GameObject centre;
    
    //Player canvas
    [SerializeField] GameObject HUDCanvas;
    public GameObject CurrentRune;
    public float SpeedOfRune = 1.0f;
    public float runeTimer = 3.0f;

    //north 0, west 1, south west 2, south east 3, east 4
    int currentDirection = 0;

    void Awake()
    {
        //check the string and add runes in string to the runes that should be played
        for(int i = 0; i < RunesText.Length; i++)
        {
            char CurrentChar = RunesText[i];
            switch (CurrentChar) {
                case 'B':
                    Runes.Add(BlueRune);
                    break;
                case 'O':
                    Runes.Add(OrangeRune);
                    break;
                case 'Y':
                    Runes.Add(YellowRune);
                    break;
                case 'R':
                    Runes.Add(RedRune);
                    break;
                case 'G':
                    Runes.Add(GreenRune);
                    break;
                default:
                    Debug.Log("Incorrect Input");
                    break;
            }
        }
        //if centre isn't found try to find it
        if (!centre) {
            centre = GameObject.FindGameObjectWithTag("Centre");
        }
        //if it's still not found display error
        if (!centre) {
            Debug.Log("Centre not found");
        }

    }

    void Start()
    {
        RuneLocations();
        UpdateRune();
    }
    
    void Update()
    {
        //get centre point
        //get points around centre - distance + angle from centre - runes already move towards pointer
        RuneLocations();


    }

    public void UpdateRune()
    {
        //if there is runes
        if(Runes.Count > 0)
        {
            //temporarily the direction is random from where the rune spawns
            currentDirection = Random.Range(0, 4);
            CurrentRune = Runes[0]; //current rune is always first rune in list
            //instantiate depending on direction
            switch(currentDirection) {
                case 0:
                    Instantiate(CurrentRune,NorthSpawnpoint, Quaternion.identity, centre.transform);
                    Debug.Log("north");
                    break;
                case 1:
                    Instantiate(CurrentRune,WestSpawnpoint, Quaternion.identity, centre.transform);
                    Debug.Log("west");
                    break;
                case 2:
                    Instantiate(CurrentRune,SouthWestSpawnpoint, Quaternion.identity, centre.transform);
                    Debug.Log("southwest");
                    break;
                case 3:
                    Instantiate(CurrentRune,SouthEastSpawnpoint, Quaternion.identity, centre.transform);
                    Debug.Log("southeast");
                    break;
                case 4:
                    Instantiate(CurrentRune,EastSpawnpoint, Quaternion.identity, centre.transform);
                    Debug.Log("east");
                    break;
                default:
                    Debug.Log("Path literally doesn't exist");
                    break;
            }
            //remove rune once it's been instantiated
            Runes.RemoveAt(0);
        }
        

    }

    void RuneLocations() {
        //calculate rune spawnpoint depending on centre pos
        NorthSpawnpoint  = centre.transform.position + new Vector3(0.0f, 50.0f, 0.0f);
        EastSpawnpoint = centre.transform.position + new Vector3(50.0f, 0.0f, 0.0f);
        WestSpawnpoint = centre.transform.position + new Vector3(50.0f, 0.0f, 0.0f);
        SouthEastSpawnpoint = centre.transform.position + new Vector3(-30.0f, 30.0f, 0.0f);
        SouthWestSpawnpoint = centre.transform.position + new Vector3(30.0f, 30.0f, 0.0f);
    }

}
