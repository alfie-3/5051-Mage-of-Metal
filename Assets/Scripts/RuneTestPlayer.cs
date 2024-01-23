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
    [SerializeField] private RuneManager runeManager;
    float rotX = 0;

    private EnemyBehaviour enemyScript;
    void Start()
    {
        playerCam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //looking around logic
        rotX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotX = Mathf.Clamp(rotX, -lookLimitX, lookLimitX);
        playerCam.transform.localRotation = Quaternion.Euler(rotX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

        //attacking logic
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2000))
        {
            if (hit.transform.gameObject.tag == "Enemy")
            {
                Debug.Log("Enemy Hit");
                enemyScript = hit.transform.gameObject.GetComponent<EnemyBehaviour>();
                Gun();
            }
        }


    }

    void Gun()
    {
        char[] runeName = runeManager.CurrentRune.name.ToCharArray();

        if(Input.GetKeyDown(KeyCode.B))
        {
            if(runeName[0] == 'B')
            {
                Debug.Log("Blue Hit!");
                enemyScript.Damage(1);
            }
        }

        if(Input.GetKeyDown(KeyCode.Y))
        {
            if(runeName[0] == 'Y')
            {
                Debug.Log("Yellow Hit!");
                enemyScript.Damage(1);
            }
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            if(runeName[0] == 'O')
            {
                Debug.Log("Orange Hit!");
                enemyScript.Damage(1);
            }
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            if(runeName[0] == 'R')
            {
                Debug.Log("Red Hit!");
                enemyScript.Damage(1);
            }
        }

        if(Input.GetKeyDown(KeyCode.G))
        {
            if(runeName[0] == 'G')
            {
                Debug.Log("Green Hit!");
                enemyScript.Damage(1);
            }
        }

    }
    
}
