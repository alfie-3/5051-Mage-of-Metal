using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class RuneFMODBridge : MonoBehaviour
{
    public static RuneFMODBridge Instance { get; private set; }

    //inspector display stuff
    [Header("Rune Order")]
    [TextArea]
    public string Rune_Instructions = "Type in the Runes Text box the order of which the runes should appear, B (Blue), O ( Orange), Y (Yellow), G (Green), R (Red) ";

    [Header("Rune Sprites")]
    [TextArea]
    public string Rune_Sprites_Info = "These are temporary placeholders, to be replaced when rune sprites designed";

//serialized
    [SerializeField] EnemyRune BlueRunePrefab;
    [SerializeField] EnemyRune OrangeRunePrefab;
    [SerializeField] EnemyRune YellowRunePrefab;
    [SerializeField] EnemyRune GreenRunePrefab;
    [SerializeField] EnemyRune RedRunePrefab;

    //[SerializeField] GameObject RuneLimit;
    [SerializeField] int ColliderSize;
    [SerializeField] float DistanceFromCentre = 50.0f;
//public
    public float SpeedOfRune = 1.0f;
    public List<EnemyRune>  RunesInScene = new List<EnemyRune>();

    [Header("Holders")]
    [SerializeField] GameObject BlueRuneHolder;
    [SerializeField] GameObject OrangeRuneHolder;
    [SerializeField] GameObject YellowRuneHolder;
    [SerializeField] GameObject GreenRuneHolder;
    [SerializeField] GameObject RedRuneHolder;

    Vector3 GreenSpawnpoint;
    Vector3 RedSpawnpoint;
    Vector3 BlueSpawnpoint;
    Vector3 YellowSpawnpoint;
    Vector3 OrangeSpawnpoint;
    //centre = Vector3(7344.7998,3658.19995,0)

    //private
    //int currentDirection = 0;

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

    void Awake()
    {
        Instance = this;
        //BoxCollider RuneLimitCollider = RuneLimit.GetComponent<BoxCollider>();
        //RuneLimitCollider.size = new Vector3(ColliderSize, ColliderSize, 3);
        _controlsKnm = new Controls();

    }
    private void OnEnable()
    {
        controls = _controlsKnm.GuitarControls.Strum;
        note1 = _controlsKnm.GuitarControls.Note1;
        note2 = _controlsKnm.GuitarControls.Note2;
        note3 = _controlsKnm.GuitarControls.Note3;
        note4 = _controlsKnm.GuitarControls.Note4;
        note5 = _controlsKnm.GuitarControls.Note5;
        controls.performed += OnStrum;
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
        controls.performed -= OnStrum;
        controls.Disable();
        note2.Disable();
    }

    private void OnStrum(InputAction.CallbackContext context)
    {
        //Debug.Log("Activated");
        ColorCheck runeColors = new ColorCheck();
        runeColors.Blue = false; runeColors.Green = false; runeColors.Red = false; runeColors.Yellow = false; runeColors.Orange = false;
        ColorCheck guitarRunes = new ColorCheck();
        runeColors.Green = isNote1; runeColors.Red = isNote2; runeColors.Yellow = isNote3; runeColors.Blue = isNote4; runeColors.Orange = isNote5;

        int power = 0;
        foreach (EnemyRune rune in RunesInScene)
        {
            EnemyRune er = rune.GetComponent<EnemyRune>();
            if (er.isPlayable)
            {
                switch (er.color)
                {
                    case 'g': if (isNote1) { runeColors.Green = true; SuccessfulRune(rune); power++; } break;
                    case 'r': if (isNote2) { runeColors.Red = true; SuccessfulRune(rune); power++; } break;
                    case 'y': if (isNote3) { runeColors.Yellow = true; SuccessfulRune(rune); power++; } break;
                    case 'b': if (isNote4) { runeColors.Blue = true; SuccessfulRune(rune); power++; } break;
                    case 'o': if (isNote5) { runeColors.Orange = true; SuccessfulRune(rune); power++; } break;
                }
            }
        }
        if (isNote1 && !runeColors.Green) { power--; }
        if (isNote2 && !runeColors.Red) { power--; }
        if (isNote3 && !runeColors.Yellow) { power--; }
        if (isNote4 && !runeColors.Blue) { power--; }
        if (isNote5 && !runeColors.Orange) { power--; }

        Debug.Log(power);

        CursorPointer.Instance.Shoot(power);

    }

    void SuccessfulRune(EnemyRune rune) {

        Destroy(rune);
    }

    public void CleanList(EnemyRune rune)
    {
        RunesInScene.Remove(rune);
        Destroy(rune.gameObject);
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
    public void RemoveRune(EnemyRune rune)
    {
        if(rune)
        {
            RunesInScene.Remove(rune);
            Destroy(rune);
        }
    }

}


public struct ColorCheck {
    public bool Green;
    public bool Red;
    public bool Yellow;
    public bool Blue;
    public bool Orange;

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
        */

        return Mathf.Clamp(success,0,5);
    }

    public static bool HasColor(ColorCheck c1)
    {
        if (c1.Green || c1.Red || c1.Yellow || c1.Blue || c1.Orange) { return true; } else { return false; }
    }
}
