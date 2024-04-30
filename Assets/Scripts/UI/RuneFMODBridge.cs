//A bridge script between CursorPointer, AudioManager and EnemyRune for dealing with interaction
//of runes and recieving damage feedback

using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;

public class RuneFMODBridge : MonoBehaviour
{
    public static RuneFMODBridge Instance { get; private set; }

    //[SerializeField] GameObject RuneLimit;
    [SerializeField] int ColliderSize;
    [SerializeField] float DistanceFromCentre = 50.0f;
//public
    public float SpeedOfRune = 1.0f;
    public List<EnemyRune>  RunesInScene = new List<EnemyRune>();

    [Header("Holders")]
    [SerializeField] GameObject GreenRuneHolder;
    [SerializeField] GameObject RedRuneHolder;
    [SerializeField] GameObject YellowRuneHolder;
    [SerializeField] GameObject BlueRuneHolder;
    [SerializeField] GameObject OrangeRuneHolder;
    Vector3 GreenSpawnpoint;
    Vector3 RedSpawnpoint;
    Vector3 BlueSpawnpoint;
    Vector3 YellowSpawnpoint;
    Vector3 OrangeSpawnpoint;

    [Header("Rune button controls")]
    private Controls _controlsKnm;
    InputAction controls;
    InputAction note1;
    InputAction note2;
    InputAction note3;
    InputAction note4;
    InputAction note5;
    bool isNote1;
    bool isNote2;
    bool isNote3;
    bool isNote4;
    bool isNote5;

    [Header("Player score component")]
    IScore playerScore;
    
    void Awake()
    {
        Instance = this;
        //BoxCollider RuneLimitCollider = RuneLimit.GetComponent<BoxCollider>();
        //RuneLimitCollider.size = new Vector3(ColliderSize, ColliderSize, 3);
        _controlsKnm = new Controls();
        playerScore = LevelManager.player.GetComponent<IScore>();
    }
    private void OnEnable()
    {
        controls = _controlsKnm.GuitarControls.Strum;
        note1 = _controlsKnm.GuitarControls.Note1;
        note2 = _controlsKnm.GuitarControls.Note2;
        note3 = _controlsKnm.GuitarControls.Note3;
        note4 = _controlsKnm.GuitarControls.Note4;
        note5 = _controlsKnm.GuitarControls.Note5;
        note1.performed += context => isNote1 = true;
        note2.performed += context => isNote2 = true;
        note3.performed += context => isNote3 = true;
        note4.performed += context => isNote4 = true;
        note5.performed += context => isNote5 = true;
        note1.canceled += context => isNote1 = false;
        note2.canceled += context => isNote2 = false;
        note3.canceled += context => isNote3 = false;
        note4.canceled += context => isNote4 = false;
        note5.canceled += context => isNote5 = false;

        controls.Enable(); note1.Enable(); note2.Enable(); note3.Enable(); note4.Enable(); note5.Enable();

    }
    private void OnDisable()
    {
        controls.Disable(); note1.Disable(); note2.Disable(); note3.Disable(); note4.Disable(); note5.Disable();
    }

    public int RuneAttack()
    {
        ColorCheck runeColors = new ColorCheck();
        runeColors.Blue = false; runeColors.Green = false; runeColors.Red = false; runeColors.Yellow = false; runeColors.Orange = false;

        if (WiiInputManager.CursorWiiMote.HasRemote)
        {
            isNote1 = WiiInputManager.GuitarWiiMote.WiiMote.Guitar.green_fret;
            isNote2 = WiiInputManager.GuitarWiiMote.WiiMote.Guitar.red_fret;
            isNote3 = WiiInputManager.GuitarWiiMote.WiiMote.Guitar.yellow_fret;
            isNote4 = WiiInputManager.GuitarWiiMote.WiiMote.Guitar.blue_fret;
            isNote5 = WiiInputManager.GuitarWiiMote.WiiMote.Guitar.orange_fret;
        }

        int power = 0;
        foreach (EnemyRune rune in RunesInScene)
        {
            if (rune.isPlayable)
            {
                switch (rune.color)
                {
                    case 'g': if (isNote1) { runeColors.Green = true; StartCoroutine(SuccessNote(rune)); power++; } break;
                    case 'r': if (isNote2) { runeColors.Red = true; StartCoroutine(SuccessNote(rune)); power++; } break;
                    case 'y': if (isNote3) { runeColors.Yellow = true; StartCoroutine(SuccessNote(rune)); power++; } break;
                    case 'b': if (isNote4) { runeColors.Blue = true; StartCoroutine(SuccessNote(rune)); power++; } break;
                    case 'o': if (isNote5) { runeColors.Orange = true; StartCoroutine(SuccessNote(rune)); power++; } break;
                }
            }
        }
        if (isNote1 && !runeColors.Green) { power--; playerScore.DamageScore(0.03f); }
        if (isNote2 && !runeColors.Red) { power--; playerScore.DamageScore(0.03f); }
        if (isNote3 && !runeColors.Yellow) { power--; playerScore.DamageScore(0.03f); }
        if (isNote4 && !runeColors.Blue) { power--; playerScore.DamageScore(0.03f); }
        if (isNote5 && !runeColors.Orange) { power--; playerScore.DamageScore(0.03f); }
        if (power==0) { playerScore.DamageScore(0.03f); }
        return power;
    }

    public IEnumerator SuccessNote(EnemyRune rune)
    {
        Vector3 pos = LevelManager.pointer.transform.position;
        rune.travel = false;
        switch (rune.color)
        {
            case 'g': pos = GreenRuneHolder.transform.position; break; 
            case 'r': pos = RedRuneHolder.transform.position; break; 
            case 'y': pos = YellowRuneHolder.transform.position; break; 
            case 'b': pos = BlueRuneHolder.transform.position; break; 
            case 'o': pos = OrangeRuneHolder.transform.position; break;
        }
        StartCoroutine(ShaderManager.Instance.RuneEffect(rune.gameObject, pos));
        rune.isPlayable = false;
        yield return null;
        RunesInScene.Remove(rune);
    }

    public IEnumerator CleanList(EnemyRune rune)
    {
        yield return null;
        rune.isPlayable = false;
        rune.travel = false;
        RunesInScene.Remove(rune);
        rune.gameObject.SetActive(false);
    }

    public void SpawnNote(string noteName)
    {
        RuneLocations(); EnemyRune temp = null;
        switch (noteName)
        {
            case "1":
                
                temp = ObjectPooler.Instance.SpawnPooledObject("Note1", GreenSpawnpoint).GetComponent<EnemyRune>();
                RunesInScene.Add(temp);
                break;
            case "2":
                temp = ObjectPooler.Instance.SpawnPooledObject("Note2", RedSpawnpoint).GetComponent<EnemyRune>();
                break;
            case "3":
                temp = ObjectPooler.Instance.SpawnPooledObject("Note3", YellowSpawnpoint).GetComponent<EnemyRune>();
                break;
            case "4":
                temp = ObjectPooler.Instance.SpawnPooledObject("Note4", BlueSpawnpoint).GetComponent<EnemyRune>();
                break;
            case "5":
                temp = ObjectPooler.Instance.SpawnPooledObject("Note5", OrangeSpawnpoint).GetComponent<EnemyRune>();
                break;
            default:
                Debug.Log("Note Name Not Found!");
                break;
        }
        if (temp != null)
        {
            temp.OnStart();
            temp.travel = true;
            RunesInScene.Add(temp);
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
}


public struct ColorCheck {
    public bool Green;
    public bool Red;
    public bool Yellow;
    public bool Blue;
    public bool Orange;

    /*
    public static bool Compare(ColorCheck c1, ColorCheck c2)
    {
        if (c1.Green!=c2.Green) { return false; }
        if (c1.Red!=c2.Red) { return false; }
        if (c1.Yellow!=c2.Yellow) { return false; }
        if (c1.Blue!=c2.Blue) { return false; }
        if (c1.Orange!=c2.Orange) { return false; }
        return true;
    }

    public static int GetSuccessRate(ColorCheck c1, ColorCheck c2)
    {
        int success = 0;
        if (c1.Green == c2.Green && c1.Green) { success += 1; } else if (c1.Green != c2.Green) { success -= 1; }
        if (c1.Red == c2.Red && c1.Red) { success += 1; } else if (c1.Red != c2.Red) { success -= 1; }
        if (c1.Yellow == c2.Yellow && c1.Yellow) { success += 1; } else if (c1.Yellow != c2.Yellow) { success -= 1; }
        if (c1.Blue == c2.Blue && c1.Blue) { success += 1; } else if (c1.Blue != c2.Blue) { success -= 1; }
        if (c1.Orange == c2.Orange && c1.Orange) { success += 1; } else if (c1.Orange != c2.Orange) { success -= 1; }

        /*
        if (c1.Green == c2.Green) { Debug.Log(1); }
        if (c1.Red == c2.Red) { Debug.Log(2); }
        if (c1.Yellow == c2.Yellow) { Debug.Log(3); }
        if (c1.Blue == c2.Blue) { Debug.Log(4); }
        if (c1.Orange == c2.Orange) { Debug.Log(5); }
        

        return Mathf.Clamp(success,0,5);
    }

    public static bool HasColor(ColorCheck c1)
    {
        if (c1.Green || c1.Red || c1.Yellow || c1.Blue || c1.Orange) { return true; } else { return false; }
    }*/
}
