using System.Collections;
using System.Collections.Generic;
using System.Data;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using WiimoteApi;



public class RuneTestPlayer : MonoBehaviour
{
    //script for testing player implementation

    //serialized
    [SerializeField] private RuneFMODBridge runeManager;

    [SerializeField] private GameObject BlueEffect;
    [SerializeField] private GameObject GreenEffect;
    [SerializeField] private GameObject YellowEffect;
    [SerializeField] private GameObject OrangeEffect;
    [SerializeField] private GameObject RedEffect;
    [SerializeField] private bool KeyboardInput;
    [SerializeField] private bool GuitarInput;
    
    //private
    private Transform currentEnemy;
    private bool canFire = false;
    private List<GameObject> StrumRunes = new List<GameObject>();
    private EnemyBehaviour enemyScript;
    private GuitarRemoteInput guitardata;

    void Start()
    {
        guitardata = WiiInputManager.GuitarWiiMote;

        //gets camera + rune manager
        GameObject managerObject = GameObject.FindGameObjectWithTag("Manager");
        runeManager = managerObject.GetComponent<RuneFMODBridge>();
        if(runeManager) //checks if manager found
        {
            Debug.Log("manager found");        
        }
        else
        {
            Debug.Log("manager not found");
        }

    }

    // Update is called once per frame
    void Update()
    {

        //creates raycast stuff
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 6000))
        {
            //if looking at enemy
            if (hit.transform.gameObject.tag == "Enemy")
            {
                //get the enemy script of the enemy being looked at
                enemyScript = hit.transform.gameObject.GetComponent<EnemyBehaviour>();
                //if player looking at enemy can fire
                canFire = true;
                //get transform for spawning shooting effects
                currentEnemy = hit.transform;
                Debug.Log("can fire");
            }
            else
            {
                //if the player isnt looking at an enemy - cant fire
                canFire = false;
                Debug.Log("cant fire");
            }
        }

        if(KeyboardInput)
        {
            StrumGunKeyboard();
        }
        else if(GuitarInput)
        {
            StrumGunGuitar();
        }
    }


    //special effects function
    void FancyEffect(string effectType)
    {
        //depending on string inputted - spawns effect on enemy
        if(effectType == "blue") {
            Instantiate(BlueEffect, currentEnemy);
        }
        else if(effectType == "green") {
            Instantiate(GreenEffect, currentEnemy);
        }
        else if(effectType == "yellow"){
            Instantiate(YellowEffect, currentEnemy);
        } 
        else if(effectType == "orange"){
            Instantiate(OrangeEffect, currentEnemy);
        }
         else if(effectType == "red"){
            Instantiate(RedEffect, currentEnemy);
        }
        //destroy after a little
        Destroy(GameObject.FindGameObjectWithTag("effect"), 0.3f);

    }


   public void StrumGunGuitar()
    {
        // If there are no runes in the scene or player can't fire, exit
        if (runeManager.RunesInScene.Count == 0 || !canFire)
            return;

        // Check key inputs for corresponding runes
        foreach (GameObject rune in runeManager.RunesInScene)
        {
            char runeColor = rune.name[0]; // First character of rune name represents color

            if (guitardata.ColorPressedThisFrame(GUITAR_COLORS.BLUE) && runeColor == 'B') 
            {
                StrumRunes.Add(rune);
            }
            else if (guitardata.ColorPressedThisFrame(GUITAR_COLORS.YELLOW) && runeColor == 'Y')
            {
                StrumRunes.Add(rune);
            }
            else if (guitardata.ColorPressedThisFrame(GUITAR_COLORS.ORANGE) && runeColor == 'O')
            {
                StrumRunes.Add(rune);
            }
            else if (guitardata.ColorPressedThisFrame(GUITAR_COLORS.GREEN) && runeColor == 'G')
            {
                StrumRunes.Add(rune);
            }
            else if (guitardata.ColorPressedThisFrame(GUITAR_COLORS.RED) && runeColor == 'R')
            {
                StrumRunes.Add(rune);
            }

            UnRunerGuitar(rune);
        }

        if (guitardata.CheckStrummedThisFrame() && StrumRunes.Count > 0)
        {
            int damage = StrumRunes.Count;
            FancyEffect("red");
            enemyScript.Damage(damage);
            Debug.Log(damage);
            foreach(GameObject rune in StrumRunes)
            {
                runeManager.RemoveRune(rune);
            }
            StrumRunes.Clear();
        }
    }

    public void StrumGunKeyboard()
    {

        // If there are no runes in the scene or player can't fire, exit
        if (runeManager.RunesInScene.Count == 0 || !canFire)
            return;

        // Check key inputs for corresponding runes
        foreach (GameObject rune in runeManager.RunesInScene)
        {
            char runeColor = rune.name[0]; // First character of rune name represents color

            if (Input.GetKeyDown(KeyCode.B) && runeColor == 'B') 
            {
                StrumRunes.Add(rune);
            }
            else if (Input.GetKeyDown(KeyCode.Y) && runeColor == 'Y')
            {
                StrumRunes.Add(rune);
            }
            else if (Input.GetKeyDown(KeyCode.O) && runeColor == 'O')
            {
                StrumRunes.Add(rune);
            }
            else if (Input.GetKeyDown(KeyCode.G) && runeColor == 'G')
            {
                StrumRunes.Add(rune);
            }
            else if (Input.GetKeyDown(KeyCode.R) && runeColor == 'R')
            {
                StrumRunes.Add(rune);
            }

            UnRunerKeyboard(rune);
        }

        if (Input.GetKeyDown(KeyCode.Space) && StrumRunes.Count > 0)
        {
            int damage = StrumRunes.Count;
            FancyEffect("red");
            enemyScript.Damage(damage);
            Debug.Log(damage);
            foreach(GameObject rune in StrumRunes)
            {
                runeManager.RemoveRune(rune);
            }
            StrumRunes.Clear();
        }
    }

void UnRunerGuitar(GameObject rune)
{
    char runeColor = rune.name[0]; // First character of rune name represents color

    if (guitardata.ColorReleasedThisFrame(GUITAR_COLORS.BLUE) && runeColor == 'B') 
    {
        StrumRunes.Remove(rune);
        runeManager.RemoveRune(rune);
        
    }
    else if (guitardata.ColorReleasedThisFrame(GUITAR_COLORS.YELLOW) && runeColor == 'Y')
    {
        StrumRunes.Remove(rune);
        runeManager.RemoveRune(rune);
    }
    else if (guitardata.ColorReleasedThisFrame(GUITAR_COLORS.ORANGE) && runeColor == 'O')
    {
        StrumRunes.Remove(rune);
        runeManager.RemoveRune(rune);

    }
    else if (guitardata.ColorReleasedThisFrame(GUITAR_COLORS.GREEN) && runeColor == 'G')
    {
        StrumRunes.Remove(rune);
        runeManager.RemoveRune(rune);
    
    }
    else if (guitardata.ColorReleasedThisFrame(GUITAR_COLORS.RED) && runeColor == 'R')
    {
        StrumRunes.Remove(rune);
        runeManager.RemoveRune(rune);    
    }
}

void UnRunerKeyboard(GameObject rune)
{

    char runeColor = rune.name[0]; // First character of rune name represents color

    if (Input.GetKeyUp(KeyCode.B) && runeColor == 'B') 
    {
        StrumRunes.Remove(rune);
        runeManager.RemoveRune(rune);
        
    }
    else if (Input.GetKeyUp(KeyCode.Y) && runeColor == 'Y')
    {
        StrumRunes.Remove(rune);
        runeManager.RemoveRune(rune);
    }
    else if (Input.GetKeyUp(KeyCode.O) && runeColor == 'O')
    {
        StrumRunes.Remove(rune);
        runeManager.RemoveRune(rune);

    }
    else if (Input.GetKeyUp(KeyCode.G) && runeColor == 'G')
    {
        StrumRunes.Remove(rune);
        runeManager.RemoveRune(rune);
    
    }
    else if (Input.GetKeyUp(KeyCode.R) && runeColor == 'R')
    {
        StrumRunes.Remove(rune);
        runeManager.RemoveRune(rune);    
    }
}


    //Adriens function for guitar things
    public void Strummed()
    {
        if (canFire == true)
        {
            GuitarRemoteInput guitardata = WiiInputManager.GuitarWiiMote;
            List<GameObject> RunesInScene = runeManager.RunesInScene;
            foreach (GameObject rune in RunesInScene)
            {
                char[] runeName = rune.name.ToCharArray();
                if (guitardata.ColorPressedThisFrame(GUITAR_COLORS.GREEN))
                {
                    FancyEffect("green");
                    Debug.Log("Green Hit!");
                    enemyScript.Damage(1);
                    runeManager.RemoveRune(rune);

                }
                if (guitardata.ColorPressedThisFrame(GUITAR_COLORS.RED))
                {
                    FancyEffect("red");
                    Debug.Log("Red Hit!");
                    enemyScript.Damage(1);
                    runeManager.RemoveRune(rune);
                    
                }
                if (guitardata.ColorPressedThisFrame(GUITAR_COLORS.YELLOW))
                {
                    FancyEffect("yellow");
                    Debug.Log("Yellow Hit!");
                    enemyScript.Damage(1);
                    runeManager.RemoveRune(rune);

                }
                if (guitardata.ColorPressedThisFrame(GUITAR_COLORS.BLUE))
                {
                    FancyEffect("blue");
                    Debug.Log("Blue Hit!");
                    enemyScript.Damage(1);
                    runeManager.RemoveRune(rune);

                }
                if (guitardata.ColorPressedThisFrame(GUITAR_COLORS.ORANGE))
                {
                    FancyEffect("orange");
                    Debug.Log("Orange Hit!");
                    enemyScript.Damage(1);
                    runeManager.RemoveRune(rune);

                }
            }
        }
    }

}
