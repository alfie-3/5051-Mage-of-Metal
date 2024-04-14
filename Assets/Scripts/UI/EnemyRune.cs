using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyRune : MonoBehaviour
{

    //script of runes that appear in game

    //variables
    private RuneFMODBridge runeManager;
    private Vector3 velocity;
    private Vector3 direction;
    public bool isPlayable = false;
    public char color;

    void Awake()
    {
        //get manager script
        runeManager = LevelManager.runeManager.GetComponent<RuneFMODBridge>();

        //get player script
        GameObject playerObject = LevelManager.player;
    }

    // Start is called before the first frame update
    void Start()
    {
        direction = LevelManager.pointer.transform.position - transform.position;
        velocity = (direction - (direction.normalized*105) * ((60 / LevelManager.bpm) * LevelManager.beatLeadUp));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += velocity * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Pointer")
        {
            Destroy(gameObject);
        }
        else if (collision.name == "Rune Holder")
        {
            isPlayable = true;
        }
    }

    private void OnDestroy()
    {
        RuneFMODBridge.Instance.CleanList(this);
    }
}
