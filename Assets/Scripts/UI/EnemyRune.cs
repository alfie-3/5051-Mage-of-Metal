//Script of runes that appear in game

using System.Data;
using UnityEngine;

public class EnemyRune : MonoBehaviour
{
    //Base rune characteristics
    private Vector3 velocity;
    private Vector3 direction;
    public bool isPlayable = false;
    public char color;

    //Set the direction and velocity towards the cursor centre
    void Start()
    {
        direction = LevelManager.pointer.transform.position - transform.position;
        velocity = (direction - (direction.normalized*105) * ((60 / AudioManager.bpm) * AudioManager.beatLeadUp));
    }

    //Rune updated position towards cursor centre
    void Update()
    {
        transform.position += velocity * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Rune hides when it hits the cursor centre
        if (collision.name == "Pointer")
        {
            //Hide rune and hurt score if it's missed
            LevelManager.player.GetComponent<IScore>().DamageScore(0.03f, Color.gray, 0.3f);
            StartCoroutine(RuneFMODBridge.Instance.CleanList(this));
        }
        //Rune becomes active when it's played
        else if (collision.name == "Rune Holder") { isPlayable = true; }
    }
}
