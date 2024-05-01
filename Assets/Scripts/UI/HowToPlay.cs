using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlay : MonoBehaviour
{
    //script that displays the how to play menu + functions for the buttons

    //[SerializeField] private Animator animator;
    [Header("Canvi")]
    [SerializeField] private GameObject MainMenuCanvas;
    [SerializeField] private GameObject HTPCanvas;

    [Header("Displays")]
    [SerializeField] private GameObject DisplayOne;
    [SerializeField] private GameObject DisplayTwo;
    [SerializeField] private GameObject DisplayThree;
    private int currentlyActive;
    private void Start()
    {
        //make sure correct display is set
        currentlyActive = 1;
    }
    public void SwitchCanvi()
    {
        //switch canvi + give time for anim
        //animator.LevelSelectLerp("animate");
        StartCoroutine(Reset(1.0f, 1));
    }

    IEnumerator Reset(float speed, int canvasType)
    {
        //wait for anim
        yield return new WaitForSeconds(0.1f);

        //check which canvas on currently
        //animator.speed = speed;
        if (canvasType == 1)
        {
            //activate/deactivate needed canvas
            HTPCanvas.SetActive(true);
            MainMenuCanvas.SetActive(false);
        }
        else if(canvasType == 2)
        {
            HTPCanvas.SetActive(false);
            MainMenuCanvas.SetActive(true);
        }
    }

public void SwitchDisplayLeft()
{
    //check what is currently active
    switch (currentlyActive)
    {
        case 1:
            //activate what display would be before + switch currently active
            currentlyActive = 3;
            DisplayOne.SetActive(false);
            DisplayThree.SetActive(true);
            break;
        case 2:
            currentlyActive = 1;
            DisplayTwo.SetActive(false);
            DisplayOne.SetActive(true);
            break;
        case 3:
            currentlyActive = 2;
            DisplayThree.SetActive(false);
            DisplayTwo.SetActive(true);
            break;
        default:
            Debug.Log("Something is wrong");
            break;
    }
}
public void SwitchDisplayRight()
{
    //check what is currently active
    switch (currentlyActive)
    {
        //activate what display would be after + switch currently active
        case 1:
            currentlyActive = 2;
            DisplayOne.SetActive(false);
            DisplayTwo.SetActive(true);
            break;
        case 2:
            currentlyActive = 3;
            DisplayTwo.SetActive(false);
            DisplayThree.SetActive(true);
            break;
        case 3:
            currentlyActive = 1;
            DisplayThree.SetActive(false);
            DisplayOne.SetActive(true);
            break;
        default:
            Debug.Log("Something is wrong");
            break;
    }
}
    public void Return()
    {
        //go back to main menu
        float currentSpeed = 1.0f;//animator.speed;
        //animator.speed = -1f;
        //animator.LevelSelectLerp("animate");
        StartCoroutine(Reset(currentSpeed, 2));

    }


}
