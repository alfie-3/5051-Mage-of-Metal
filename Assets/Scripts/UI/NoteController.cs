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
    [SerializeField] List<List<GameObject>> noteObjects;

    private void Start()
    {
        noteObjects = new List<List<GameObject>>();
        for (int i = 0; i < 5; i++) {
            noteObjects.Add(new List<GameObject>());
        }
        Debug.Log(noteObjects.Count);
        noteVelocity = heightSpawn / ((60 / SongBPM) * beatNumberLeadUp);
    }

    public void SpawnNote(string eventTitle)
    {
        switch (eventTitle)
        {
            case "1":
                noteObjects[0].Add(_objectPooler.SpawnNewNote("Note1", notePlace1.transform.position + new Vector3(0, heightSpawn, 0)));
                Debug.Log("Haha 1");
                break;
            case "2":
                noteObjects[1].Add(_objectPooler.SpawnNewNote("Note2", notePlace2.transform.position + new Vector3(0, heightSpawn, 0)));
                Debug.Log("JAJA 2");
                break;
            case "3":
                noteObjects[2].Add(_objectPooler.SpawnNewNote("Note3", notePlace3.transform.position + new Vector3(0, heightSpawn, 0)));
                Debug.Log("OMG 3");
                break;
            case "4":
                noteObjects[3].Add(_objectPooler.SpawnNewNote("Note4", notePlace4.transform.position + new Vector3(0, heightSpawn, 0)));
                Debug.Log("Yes it's 4");
                break;
            case "5":
                noteObjects[4].Add(_objectPooler.SpawnNewNote("Note5", notePlace5.transform.position + new Vector3(0, heightSpawn, 0)));
                Debug.Log("By Jove 5");
                break;
        }
        
    }

    private void Update()
    {
        for (int i = 0; i < noteObjects.Count; i++)
        {
            for (int j = 0; j < noteObjects[i].Count; j++)
            {
                Vector3 pos = noteObjects[i][j].GetComponent<RectTransform>().localPosition;
                noteObjects[i][j].GetComponent<RectTransform>().localPosition = new Vector3(pos.x,pos.y - (Time.deltaTime * noteVelocity),pos.z);
            }
        }
    }

}
