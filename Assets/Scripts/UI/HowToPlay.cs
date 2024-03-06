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
        animator.Play("animate");
        StartCoroutine(Reset(animator.speed, 1, 0.2f));
    }

    IEnumerator Reset(float speed, int canvasType, float seconds)
    {
        yield return new WaitForSeconds(seconds);

        animator.speed = speed;
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
        float currentSpeed = animator.speed;
        animator.speed = -1f;
        animator.Play("animate");
        StartCoroutine(Reset(currentSpeed, 2, 0.24f));

    }


}
