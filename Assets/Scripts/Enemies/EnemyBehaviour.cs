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
    //public
    public int EnemyHP = 10;
    public int EnemyMaxHP = 10;
    public float EnemyDeathTime = 1f;
    public Vector3 Offset;
    [TextArea]
    public string UIInfo = "Make sure UILocation game object is linked in the field below";
    //serialized
    //[SerializeField] private GameObject DieEffect;
    [SerializeField] private GameObject outlineComponent;
    [SerializeField] private Transform UILocation;
    private bool hasAttacked = false;

    [Header("Enemy Score Worth")]
    [SerializeField] private int scoreWorth = 20;
    [SerializeField] private float scoreMultWorth = 0.05f;
    [SerializeField] private float damageMultScore = 0.1f;

    [Header("Material values")]
    float Dissolve;
    Material mat;
    [SerializeField] Shader dissolveShader;
    Renderer rend;
    string m_MatDissolveName = "_Alpha";
    [SerializeField] Texture2D m_MatTexture;
    string m_MatTextureName = "_BaseTexture";


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
            StartCoroutine(Kill());
        }
    }

    //If enemy dies
    IEnumerator Kill()
    {
        LevelManager.player.GetComponent<IScore>().AddScore(scoreMultWorth, scoreWorth);
        outlineComponent.layer = LayerMask.NameToLayer("Default");
        mat = new Material(dissolveShader);
        rend.material = mat;

        rend.material.SetTexture(m_MatTextureName, m_MatTexture);
        rend.material.SetFloat(m_MatDissolveName, 1);
        rend.material.SetColor("_Color", new Color(Random.Range(0,255), Random.Range(0,255), Random.Range(0,255)));

        float delta = EnemyDeathTime;
        while (delta > 0)

        {
            delta -= Time.deltaTime;
            rend.material.SetFloat(m_MatDissolveName, delta / EnemyDeathTime);
            yield return null;
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Player" && !hasAttacked)
        {
            if (collision.gameObject.TryGetComponent(out IScore playerScore))
            {
                playerScore.DamageScore(damageMultScore,Color.red,0.6f);
                hasAttacked=true;
                Debug.Log("???");
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
