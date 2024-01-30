using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    [Header("Song values")]
    [SerializeField] float SongBPM;
    [SerializeField] float heightSpawn; //Height that note spawns
    [SerializeField] int beatNumberLeadUp; // How many 'beats' between spawn and reaching the destination
    [SerializeField] float distanceActivation;

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

    float noteVelocity;
    [SerializeField] List<GameObject> noteList1;
    [SerializeField] List<GameObject> noteList2;
    [SerializeField] List<GameObject> noteList3;
    [SerializeField] List<GameObject> noteList4;
    [SerializeField] List<GameObject> noteList5;

    private void Start()
    {
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
        noteList1 = NoteChecker(noteList1,1);
        noteList2 = NoteChecker(noteList2,2);
        noteList3 = NoteChecker(noteList3,3);
        noteList4 = NoteChecker(noteList4,4);
        noteList5 = NoteChecker(noteList5,5);
    }

    private List<GameObject> NoteChecker(List<GameObject> noteList, int thing)
    {
        if (noteList.Count != 0)
        {
            if (notePlace1.transform.position.y - noteList[0].transform.position.y > distanceActivation)
            {
                Debug.Log("Note " + thing + " has proceeded past thing.");
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

    public void OnNote1()
    {
        if (Vector3.Distance(noteList1[0].transform.position, notePlace1.transform.position) < distanceActivation)
        {
            noteList1[0].SetActive(false);
            noteList1.Remove(noteList1[0]);
        }
    }
    public void OnNote2()
    {
        if (Vector3.Distance(noteList2[0].transform.position, notePlace2.transform.position) < distanceActivation)
        {
            noteList2[0].SetActive(false);
            noteList2.Remove(noteList2[0]);
        }
    }
    public void OnNote3()
    {
        if (Vector3.Distance(noteList3[0].transform.position, notePlace3.transform.position) < distanceActivation)
        {
            noteList3[0].SetActive(false);
            noteList3.Remove(noteList3[0]);
        }
    }
    public void OnNote4()
    {
        if (Vector3.Distance(noteList4[0].transform.position, notePlace4.transform.position) < distanceActivation)
        {
            noteList4[0].SetActive(false);
            noteList4.Remove(noteList4[0]);
        }
    }
    public void OnNote5()
    {
        if (Vector3.Distance(noteList5[0].transform.position, notePlace5.transform.position) < distanceActivation)
        {
            noteList5[0].SetActive(false);
            noteList5.Remove(noteList5[0]);
        }
    }
}
