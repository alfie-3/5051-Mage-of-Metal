using System.Collections;
using UnityEngine;

//enemy logic for health to test health display


//system that randomly generates points along the spline
//same script gets enemies within range and shoves em into a list inside a dictionary
//enemy script movement towards player set on a bool
// if player reaches point(trigger) script sets all enemies grouped in the list to true and they start running at the player 
//not sure if this is what we need though

//alternatively - enemies set to always be running at player 
//spawn points set within level and when player gets to certain points on spline enemies spawn in from spawn points - enemies have navmeshes to navigate surroundings


public class EnemyBehaviour : MonoBehaviour, IDamage {

    [Header("Enemy stats")]
    public int EnemyHP = 10;
    public int EnemyMaxHP = 10;
    public float EnemyDeathTime = 1f;

    [Header("Enemy Object information")]
    public Vector3 Offset;
    [TextArea]
    public string UIInfo = "Make sure UILocation game object is linked in the field below";
    [SerializeField] private Transform UILocation;
    [SerializeField] private GameObject outlineComponent;
    private bool canAttack = false;

    [Header("Enemy Score Worth")]
    [SerializeField] private int scoreWorth = 20;
    [SerializeField] private float scoreMultWorth = 0.05f;
    [SerializeField] private float damageMultScore = 0.1f;

    [Header("Material values")]
    Material mat;
    [SerializeField] Shader dissolveShader;
    Renderer rend;
    string dissolveMatName = "_Alpha";
    [SerializeField] Texture2D mainMaterialTexture;
    string mainMaterialTextureName = "_BaseTexture";


    void Awake() 
    {
        //checks each child to find ui location point
        foreach (Transform child in gameObject.transform)
        {
            if (child.tag == "UIReference") {
                UILocation = child;
            }
            else{
                //Debug.Log("UI Location not found");
            }
        }
        //offset for ui
        Offset = UILocation.localPosition * 80;
    }

    void Start() {
        //set health to max
        EnemyHP = EnemyMaxHP;
        rend = outlineComponent.GetComponent<Renderer>();
    }
    //function that damages enemy when called
    public void Damage(int damage)
    {
        EnemyHP -= damage;
        if (EnemyHP <= 0)
        {
            StartCoroutine(KilledByPlayer());
        }
    }

    //When enemy is killed by player
    IEnumerator KilledByPlayer()
    {
        canAttack = false;

        //Add score to player
        LevelManager.player.GetComponent<IScore>().AddScore(scoreMultWorth, scoreWorth);

        //Change enemy layer so outline disappears
        outlineComponent.layer = LayerMask.NameToLayer("Default");

        //Create and set new materials
        mat = new Material(dissolveShader);
        rend.material = mat;
        rend.material.SetTexture(mainMaterialTextureName, mainMaterialTexture);
        rend.material.SetFloat(dissolveMatName, 1);
        rend.material.SetColor("_Color", new Color(Random.Range(0,255), Random.Range(0,255), Random.Range(0,255)));

        //Set the enemy dissolve amount over time until the enemy disappears
        float delta = EnemyDeathTime;
        while (delta > 0)
        {
            delta -= Time.deltaTime;
            rend.material.SetFloat(dissolveMatName, delta / EnemyDeathTime);
            yield return null;
        }

        //Defeat enemy
        Destroy(gameObject);
    }

    //Triggers attack on player if within reach and hasn't attacked yet
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Player" && !canAttack)
        {
            if (collision.gameObject.TryGetComponent(out IScore playerScore))
            {
                //Damage player score and affect vignette
                playerScore.DamageScore(damageMultScore,Color.red,0.6f);
                canAttack=true;
                Destroy(gameObject, 0.3f);
            }
        }
    }


    //function for testing healthbar
    /*
    IEnumerator DieEnemy() {
        while (EnemyHP != 0)
        {
            yield return new WaitForSeconds(1f);
            EnemyHP = EnemyHP - 1; // testing healthbar 
            print(EnemyHP);
        }

    }
    */
}
