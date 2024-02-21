using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class RuneTestPlayer : MonoBehaviour
{
    //needs logic for hitting correct key with the rune displayed or not correct
    //deals damage on correct hit - healthbar





    //quick script for testing
    public Camera playerCam;
    public float lookSpeed = 2.0f;
    public float lookLimitX = 45.0f;
    public bool runePlayed = false;
    private bool canFire = false;
    Transform currentEnemy;

    [SerializeField] private RuneFMODBridge runeManager;

    [SerializeField] private GameObject BlueEffect;
    [SerializeField] private GameObject GreenEffect;
    [SerializeField] private GameObject YellowEffect;
    [SerializeField] private GameObject OrangeEffect;
    [SerializeField] private GameObject RedEffect;
    float rotX = 0;


    private EnemyBehaviour enemyScript;

    void Start()
    {
        playerCam = Camera.main;
        GameObject managerObject = GameObject.FindGameObjectWithTag("Manager");
        runeManager = managerObject.GetComponent<RuneFMODBridge>();
        if(runeManager)
        {
            Debug.Log("manager found");        
        }
        else
        {
            Debug.Log("manager not found");
        }

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

        //attacking logic
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 6000))
        {
            if(hit.transform.gameObject)
            {
                Debug.Log(hit.transform.gameObject.name);
            }

            if (hit.transform.gameObject.tag == "Enemy")
            {
                Debug.Log("Enemy Hit");
                enemyScript = hit.transform.gameObject.GetComponent<EnemyBehaviour>();
                canFire = true;
                currentEnemy = hit.transform;
            }
            else
            {
                canFire = false;
            }
        }


    }


    void FancyEffect(string effectType)
        {
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
            Destroy(GameObject.FindGameObjectWithTag("effect"), 0.3f);

        }

    
    public void Gun(string buttonColor)
    {
        //look through runes in array, if any rune matches the one played play it + eliminate from array
        Debug.Log("Button Hit!");
        List<GameObject> RunesInScene = runeManager.RunesInScene;
        foreach(GameObject rune in RunesInScene)
        {
            char[] runeName = rune.name.ToCharArray();
            if(canFire == true)
            {
                if(buttonColor == "blue")
                {
                    if(runeName[0] == 'B')
                    {
                        FancyEffect("blue");
                        Debug.Log("Blue Hit!");
                        enemyScript.Damage(1);
                        runePlayed = true;
                        RunesInScene.Remove(rune);
                    }
                }

                if(buttonColor == "yellow")
                {
                    if(runeName[0] == 'Y')
                    {
                        FancyEffect("yellow");
                        Debug.Log("Yellow Hit!");
                        enemyScript.Damage(1);
                        runePlayed = true;
                        RunesInScene.Remove(rune);
                    }
                }

                if(buttonColor == "orange")
                {
                    if(runeName[0] == 'O')
                    {
                        FancyEffect("orange");
                        Debug.Log("Orange Hit!");
                        enemyScript.Damage(1);
                        runePlayed = true;
                        RunesInScene.Remove(rune);
                    }
                }

                if(buttonColor == "red")
                {
                    if(runeName[0] == 'R')
                    {
                        FancyEffect("red");
                        Debug.Log("Red Hit!");
                        enemyScript.Damage(1);
                        runePlayed = true;
                        RunesInScene.Remove(rune);
                    }
                }

                if(buttonColor == "green")
                {
                    if(runeName[0] == 'G')
                    {
                        FancyEffect("green");
                        Debug.Log("Green Hit!");
                        enemyScript.Damage(1);
                        runePlayed = true;
                        RunesInScene.Remove(rune);
                    }
                }
            }
        }
        

    }
    
}
