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
    public bool travel = true;

    //Set the direction and velocity towards the cursor centre
    public void OnStart()
    {
        direction = LevelManager.pointer.transform.position - transform.position;
        float time = (60 / AudioManager.bpm) * AudioManager.beatLeadUp;
        velocity = direction * time;
        travel = true;
        isPlayable = false;
    }

    //Rune updated position towards cursor centre
    void Update()
    {
        if (travel)
        {
            transform.position += velocity * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Rune hides when it hits the cursor centre
        if (collision.name == "Pointer")
        {
            //Hide rune and hurt score if it's missed
            LevelManager.player.GetComponent<IScore>().DamageScore(0.03f);
            StartCoroutine(ShaderManager.Instance.BadRuneEffect(this.gameObject, LevelManager.pointer.transform.position));
            travel = false;
            isPlayable = false;
        }
        //Rune becomes active when it's played
        else if (collision.name == "Rune Holder") { isPlayable = true; }
    }
}
