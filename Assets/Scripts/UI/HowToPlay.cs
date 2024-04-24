using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlay : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject MainMenuCanvas;
    [SerializeField] private GameObject HTPCanvas;
    [SerializeField] private GameObject DisplayOne;
    [SerializeField] private GameObject DisplayTwo;
    [SerializeField] private GameObject DisplayThree;
    private int currentlyActive;
    private void Start()
    {
        currentlyActive = 1;
    }
    public void SwitchCanvi()
    {
        //animator.Play("animate");
        StartCoroutine(Reset(1.0f, 1));
    }

    IEnumerator Reset(float speed, int canvasType)
    {
        yield return new WaitForSeconds(0.3f);

        //animator.speed = speed;
        if (canvasType == 1)
        {
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
    switch (currentlyActive)
    {
        case 1:
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
    switch (currentlyActive)
    {
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
        float currentSpeed = 1.0f;//animator.speed;
        //animator.speed = -1f;
        //animator.Play("animate");
        StartCoroutine(Reset(currentSpeed, 2));

    }


}
