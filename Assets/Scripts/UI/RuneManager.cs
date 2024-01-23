using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//what needs to be done:
//at the moment runes will appear in order of array and not on beat
//only current note is playable
//way to check what button player presses against current rune

//change way next rune is called
//put it into this script
//need a way to skip to next rune once hit



public class RuneManager : MonoBehaviour
{
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


    [SerializeField] Transform RuneSpawnpoint;
    [SerializeField] GameObject HUDCanvas;
    public GameObject CurrentRune;
    public float SpeedOfRune = 1.0f;
    public float runeTimer = 3.0f;

    void Awake()
    {
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
    }

    void Start()
    {
        UpdateRune();
    }
    
    public void UpdateRune()
    {
        if(Runes.Count > 0)
        {
            CurrentRune = Runes[0];
            Instantiate(Runes[0],RuneSpawnpoint.position, Quaternion.identity, HUDCanvas.transform);
            Runes.RemoveAt(0);
        }
        

    }

}