using System.Collections;
using System.Collections.Generic;
using System.Data;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;



public class RuneTestPlayer : MonoBehaviour
{
    //script for testing player implementation

    //public vars
    public Camera playerCam;
    public float lookSpeed = 2.0f;
    public float lookLimitX = 45.0f;
    public bool runePlayed = false;

    //serialized
    [SerializeField] private RuneFMODBridge runeManager;

    [SerializeField] private GameObject BlueEffect;
    [SerializeField] private GameObject GreenEffect;
    [SerializeField] private GameObject YellowEffect;
    [SerializeField] private GameObject OrangeEffect;
    [SerializeField] private GameObject RedEffect;
    
    //private
    private float rotX = 0;
    private Transform currentEnemy;
    private EnemyBehaviour enemyScript;
    private bool canFire = false;


    void Start()
    {
        //gets camera + rune manager
        playerCam = Camera.main;
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

        //looking around logic
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //looking around logic
        //rotX += -Input.GetAxis("Mouse Y") * lookSpeed;
        //rotX = Mathf.Clamp(rotX, -lookLimitX, lookLimitX);
        //playerCam.transform.localRotation = Quaternion.Euler(rotX, 0, 0);
        //transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

        //creates raycast stuff
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 6000))
        {
            //if looking at enemy
            if (hit.transform.gameObject.tag == "Enemy")
            {
                //debug log for testing
                Debug.Log("Enemy Hit");
                //get the enemy script of the enemy being looked at
                enemyScript = hit.transform.gameObject.GetComponent<EnemyBehaviour>();
                //if player looking at enemy can fire
                canFire = true;
                //get transform for spawning shooting effects
                currentEnemy = hit.transform;
            }
            else
            {
                //if the player isnt looking at an enemy - cant fire
                canFire = false;
            }
        }
        StrumGun();


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

    //function for input (for now with keyboard for testing)
    //for strumming:
    //player needs to hold buttons of runes they want to play 
    //when strum is pressed runes that are pressed are released
    //when keys are held, runes are added to array/list
    //when keys are let go of those runes are removed from the list - separate function
    //when strum key is pressed and the keys are still being pressed those runes disappear from screen and attack is fired

    public void StrumGun()
    {
        List<GameObject>  StrumRunes = new List<GameObject>();
        //if there is a rune in scene
        if (runeManager.RunesInScene.Count != 0)
        {
            //if the player is looking at an enemy
            if (canFire == true)
            {
                //for each rune in scene - not optimised
                //goes through each rune in scene checking whether its playable
                foreach (GameObject rune in runeManager.RunesInScene)
                {
                    //get rune name
                    char[] runeName = rune.name.ToCharArray();
                    //blue
                    if(Input.GetKeyDown(KeyCode.B))
                    {
                        if (runeName[0] == 'B')
                        {
                            StrumRunes.Add(rune);
                        }
                        
                    }

                    //yellow
                    if(Input.GetKeyDown(KeyCode.Y))
                    {
                        if (runeName[0] == 'Y')
                        {
                            StrumRunes.Add(rune);
                        }
                    }

                    //orange
                    if(Input.GetKeyDown(KeyCode.O))
                    {
                        if (runeName[0] == 'O')
                        {
                            StrumRunes.Add(rune);
                        }
                    }
                    //green
                    if(Input.GetKeyDown(KeyCode.G))
                    {
                        if (runeName[0] == 'G')
                        {
                            StrumRunes.Add(rune);
                        }
                    }
                    //red
                    if(Input.GetKeyDown(KeyCode.R))
                    {
                        if (runeName[0] == 'R')
                        {
                            StrumRunes.Add(rune);
                        }
                    }

                    //strum
                    if(Input.GetKeyDown(KeyCode.Space))
                    {
                        int damage = StrumRunes.Count;
                        FancyEffect("red");
                        enemyScript.Damage(damage);
                        Debug.Log(damage);
                        StrumRunes.Clear();
                    }
                    
                }
            Debug.Log(StrumRunes);
            }
        }


    }

//functions for ui buttons    
    public void Gun(string buttonColor)
    {
        //function to play for attacking enemies
        Debug.Log("Button Hit!");
        //if there is a rune in scene
        if (runeManager.RunesInScene.Count != 0)
        {
            //if the player is looking at an enemy
            if (canFire == true)
            {
                //for each rune in scene - not optimised
                //goes through each rune in scene checking whether its playable
                foreach (GameObject rune in runeManager.RunesInScene)
                {
                    //get rune name
                    char[] runeName = rune.name.ToCharArray();
                    //if blue is pressed
                    if (buttonColor == "blue")
                    {
                        //if first letter is B of name (probably means the rune is Blue)
                        if (runeName[0] == 'B')
                        {
                            //spawn special effect
                            FancyEffect("blue");
                            //debug log for testing
                            Debug.Log("Blue Hit!");
                            //damage enemy
                            enemyScript.Damage(1);
                            //remove rune using manager function (it destroys the rune and removes it from RunesInScene)
                            runeManager.RemoveRune(rune);
                        }
                    }

                    if (buttonColor == "yellow")
                    {
                        if (runeName[0] == 'Y')
                        {
                            FancyEffect("yellow");
                            Debug.Log("Yellow Hit!");
                            enemyScript.Damage(1);
                            runeManager.RemoveRune(rune);
                        }
                    }

                    if (buttonColor == "orange")
                    {
                        if (runeName[0] == 'O')
                        {
                            FancyEffect("orange");
                            Debug.Log("Orange Hit!");
                            enemyScript.Damage(1);
                            runeManager.RemoveRune(rune);
                        }
                    }

                    if (buttonColor == "red")
                    {
                        if (runeName[0] == 'R')
                        {
                            FancyEffect("red");
                            Debug.Log("Red Hit!");
                            enemyScript.Damage(1);
                            runeManager.RemoveRune(rune);
                        }
                    }

                    if (buttonColor == "green")
                    {
                        if (runeName[0] == 'G')
                        {
                            FancyEffect("green");
                            Debug.Log("Green Hit!");
                            enemyScript.Damage(1);
                            runeManager.RemoveRune(rune);
                        }
                    }
                }
            }
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
