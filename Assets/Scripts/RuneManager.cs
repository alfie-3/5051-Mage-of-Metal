using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script requirements:
//have set array of runes that need to be played - done
//runes displayed in order of first enemy to last - done
//not in this script but - enemies need to appear before their rune needs to be played
//rune has limited amount of time to be played
//this script needs to have access to order of enemies spawning

//what needs to be done:
//each rune needs to be delegated to an enemy in order
//runes need to fade with time - timer variable
// 
public class RuneManager : MonoBehaviour
{
    List<Sprite> Runes = new List<Sprite>();
    [Header("Rune Order")]
    [TextArea]
    public string Rune_Instructions = "Type in the Runes Text box the order of which the runes should appear, B (Blue), O ( Orange), Y (Yellow), G (Green), R (Red) ";
    
    [SerializeField] string RunesText;

    [Header("Rune Sprites")]
    [TextArea]
    public string Rune_Sprites_Info = "These are temporary placeholders, to be replaced when rune sprites designed";

    [SerializeField] Sprite BlueRune;
    [SerializeField] Sprite OrangeRune;
    [SerializeField] Sprite YellowRune;
    [SerializeField] Sprite GreenRune;
    [SerializeField] Sprite RedRune;

    void Awake()
    {
        for(int i = 0; i < RunesText.Length; i++)
        {
            char CurrentRune = RunesText[i];
            switch (CurrentRune) {
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
        GameObject[] Enemies =  GameObject.FindGameObjectsWithTag("Enemy");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
