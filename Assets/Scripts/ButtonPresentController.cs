using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPresentController : MonoBehaviour
{

    [SerializeField] private Animator presentAnim = null;

    private bool presentAnimation = false;

    [SerializeField] private string openAnimationName = "PresentAnimation";

    [SerializeField] private int waitTimer = 1;
    [SerializeField] private bool pauseInteraction = false;


    private IEnumerator PausePresentInteraction()
    {
        pauseInteraction = true;
        yield return new WaitForSeconds(waitTimer);
        pauseInteraction = false;
    }

    public void PlayAnimation()
    {
        if(!presentAnimation && !pauseInteraction)
        {
            presentAnim.Play(openAnimationName, 0, 0.0f);
            presentAnimation = true;
            StartCoroutine(PausePresentInteraction());
        }
    }

}
