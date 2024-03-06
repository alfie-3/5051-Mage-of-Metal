using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlay : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject MainMenuCanvas;
    [SerializeField] private GameObject HTPCanvas;

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

    public void Return()
    {
        float currentSpeed = 1.0f;//animator.speed;
        //animator.speed = -1f;
        //animator.Play("animate");
        StartCoroutine(Reset(currentSpeed, 2));

    }


}
