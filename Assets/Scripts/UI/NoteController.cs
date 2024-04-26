//Older test script for seeing if FMOD would spawn runes into the level

using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.InputSystem;

public class NoteController : MonoBehaviour
{
    /*
    [Header("Song values")]
    [SerializeField] float SongBPM;
    [SerializeField] float heightSpawn; //Height that note spawns
    [SerializeField] int beatNumberLeadUp; // How many 'beats' between spawn and reaching the destination
    [SerializeField] float distanceActivation;
    [SerializeField] EventReference beepNoise;

    [Space]
    [Header("Pooler ref")]
    [SerializeField] ObjectPooler _objectPooler;

    [Space]
    [Header("Note destinations")]
    [SerializeField] GameObject notePlace1;
    [SerializeField] GameObject notePlace2;
    [SerializeField] GameObject notePlace3;
    [SerializeField] GameObject notePlace4;
    [SerializeField] GameObject notePlace5;

    [Space]
    [Header("Note objects")]
    [SerializeField] GameObject note1;
    [SerializeField] GameObject note2;
    [SerializeField] GameObject note3;
    [SerializeField] GameObject note4;
    [SerializeField] GameObject note5;

    [Space]
    [Header("Current existing notes")]
    float noteVelocity;
    [SerializeField] List<GameObject> noteList1;
    [SerializeField] List<GameObject> noteList2;
    [SerializeField] List<GameObject> noteList3;
    [SerializeField] List<GameObject> noteList4;
    [SerializeField] List<GameObject> noteList5;

    [Space]
    [Header("Inputs")]
    //Action references
    [SerializeField] InputActionReference strumInput;
    PlayerInput _playerInput;
    InputAction note1PressAct;
    bool note1Press;
    InputAction note2PressAct;
    bool note2Press;
    InputAction note3PressAct;
    bool note3Press;
    InputAction note4PressAct;
    bool note4Press;
    InputAction note5PressAct;
    bool note5Press;
    bool successfulNotes=true;

    private void Awake()
    {
        strumInput.action.performed += OnStrum;
    }

    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        note1PressAct = _playerInput.actions["Note1"];
        note2PressAct = _playerInput.actions["Note2"];
        note3PressAct = _playerInput.actions["Note3"];
        note4PressAct = _playerInput.actions["Note4"];
        note5PressAct = _playerInput.actions["Note5"];
        noteList1 = new List<GameObject>();
        noteList2 = new List<GameObject>();
        noteList3 = new List<GameObject>();
        noteList4 = new List<GameObject>();
        noteList5 = new List<GameObject>();
        noteVelocity = heightSpawn / ((60 / SongBPM) * beatNumberLeadUp);
    }

    public void SpawnNote(string eventTitle)
    {
        switch (eventTitle)
        {
            case "1":
                noteList1.Add(_objectPooler.SpawnNewNote("Note1", notePlace1.transform.position + new Vector3(0, heightSpawn, 0)));
                break;
            case "2":
                noteList2.Add(_objectPooler.SpawnNewNote("Note2", notePlace2.transform.position + new Vector3(0, heightSpawn, 0)));
                break;
            case "3":
                noteList3.Add(_objectPooler.SpawnNewNote("Note3", notePlace3.transform.position + new Vector3(0, heightSpawn, 0)));
                break;
            case "4":
                noteList4.Add(_objectPooler.SpawnNewNote("Note4", notePlace4.transform.position + new Vector3(0, heightSpawn, 0)));
                break;
            case "5":
                noteList5.Add(_objectPooler.SpawnNewNote("Note5", notePlace5.transform.position + new Vector3(0, heightSpawn, 0)));
                break;
        }
        
    }

    private void Update()
    {
        noteList1 = NoteUpdate(noteList1,1);
        noteList2 = NoteUpdate(noteList2,2);
        noteList3 = NoteUpdate(noteList3,3);
        noteList4 = NoteUpdate(noteList4,4);
        noteList5 = NoteUpdate(noteList5,5);
    }

    public void OnStrum(InputAction.CallbackContext ctx)
    {
        bool success=true;
        if (note1PressAct.IsPressed()) { if (CheckNote1()) { success = false; } }
        if (note2PressAct.IsPressed()) { if (CheckNote2()) { success = false; } }
        if (note3PressAct.IsPressed()) { if (CheckNote3()) { success = false; } }
        if (note4PressAct.IsPressed()) { if (CheckNote4()) { success = false; } }
        if (note5PressAct.IsPressed()) { if (CheckNote5()) { success = false; } }
        if (success)
        {
            Debug.Log("Success");
        }
    }

    //Notes lower until they go out of bounds and despawn
    private List<GameObject> NoteUpdate(List<GameObject> noteList, int index)
    {
        if (noteList.Count != 0)
        {
            if (notePlace1.transform.position.y - noteList[0].transform.position.y > distanceActivation)
            {
                //Debug.Log("Note " + index + " has proceeded past thing.");
                noteList[0].SetActive(false);
                noteList.Remove(noteList[0]);
            }

            for (int j = 0; j < noteList.Count; j++)
            {
                Vector3 pos = noteList[j].GetComponent<RectTransform>().localPosition;
                noteList[j].GetComponent<RectTransform>().localPosition = new Vector3(pos.x, pos.y - (Time.deltaTime * noteVelocity), pos.z);
            }
        }
        return noteList;
    }

    //Check what notes on wii guitar are pressed
    public bool CheckGuitarNotes()
    {
        GuitarRemoteInput guitardata = WiiInputManager.GuitarWiiMote;
        bool success = true;
        if (guitardata.ColorPressedThisFrame(GUITAR_COLORS.GREEN))
            if (!CheckNote1()) { success = false; }
        if (guitardata.ColorPressedThisFrame(GUITAR_COLORS.RED))
            if (!CheckNote2()) { success = false; }
        if (guitardata.ColorPressedThisFrame(GUITAR_COLORS.YELLOW))
            if (!CheckNote3()) { success = false; }
        if (guitardata.ColorPressedThisFrame(GUITAR_COLORS.BLUE)) //I like blue :)
            if (!CheckNote4()) { success = false; }
        if (guitardata.ColorPressedThisFrame(GUITAR_COLORS.ORANGE))
            if (!CheckNote5()) { success = false; }

        if (success) Debug.Log("Success");
        return success;
    }


    //GREEN
    public bool CheckNote1()
    {
        if (noteList1.Count != 0)
        {
            if (Vector3.Distance(noteList1[0].transform.position, notePlace1.transform.position) < distanceActivation)
            {
                noteList1[0].SetActive(false);
                noteList1.Remove(noteList1[0]);
                FMODUnity.RuntimeManager.PlayOneShotAttached(beepNoise, GameObject.Find("Player"));

                return true;
            }
        }
        successfulNotes = false;
        return false;

    }

    //RED
    public bool CheckNote2()
    {
        if (noteList2.Count != 0)
        {
            if (Vector3.Distance(noteList2[0].transform.position, notePlace2.transform.position) < distanceActivation)
            {
                noteList2[0].SetActive(false);
                noteList2.Remove(noteList2[0]);
                FMODUnity.RuntimeManager.PlayOneShotAttached(beepNoise, GameObject.Find("Player"));

                return true;
            }
        }
        successfulNotes = false;
        return false;

    }

    //YELLOW
    public bool CheckNote3()
    {
        if (noteList3.Count != 0)
        {
            if (Vector3.Distance(noteList3[0].transform.position, notePlace3.transform.position) < distanceActivation)
            {
                noteList3[0].SetActive(false);
                noteList3.Remove(noteList3[0]);
                FMODUnity.RuntimeManager.PlayOneShotAttached(beepNoise, GameObject.Find("Player"));

                return true;
            }
        }
        successfulNotes = false;
        return false;
    }

    //BLUE
    public bool CheckNote4()
    {
        if (noteList4.Count != 0)
        {
            if (Vector3.Distance(noteList4[0].transform.position, notePlace4.transform.position) < distanceActivation)
            {
                noteList4[0].SetActive(false);
                noteList4.Remove(noteList4[0]);
                FMODUnity.RuntimeManager.PlayOneShotAttached(beepNoise, GameObject.Find("Player"));

                return true;
            }
        }
        successfulNotes = false;
        return false;
    }

    //ORANGE
    public bool CheckNote5()
    {
        if (noteList5.Count != 0)
        {
            if (Vector3.Distance(noteList5[0].transform.position, notePlace5.transform.position) < distanceActivation)
            {
                noteList5[0].SetActive(false);
                noteList5.Remove(noteList5[0]);
                FMODUnity.RuntimeManager.PlayOneShotAttached(beepNoise, GameObject.Find("Player"));

                return true;
            }
        }
        successfulNotes = false;
        return false;
    }*/
}
