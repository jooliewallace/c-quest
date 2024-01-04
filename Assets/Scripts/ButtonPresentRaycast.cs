using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPresentRaycast : MonoBehaviour
{
    [SerializeField] private int rayLength = 5;
    [SerializeField] private LayerMask layerMaskInteract;
    [SerializeField] private string excludeLayerName = null;

    private ButtonPresentController raycastedObj;
    private bool doOnce;

    private const string interactableTag = "PresentButton";

    private void Start()
    {
        // You can perform any initialization here if needed
    }

    private void Update()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        int mask = 1 << LayerMask.NameToLayer(excludeLayerName) | layerMaskInteract.value;

        if (Physics.Raycast(transform.position, fwd, out hit, rayLength, mask))
        {
            if (hit.collider.CompareTag(interactableTag))
            {
                if (!doOnce)
                {
                    raycastedObj = hit.collider.gameObject.GetComponent<ButtonPresentController>();
                    Debug.Log("Button detected");
                }

                doOnce = true;

                CharacterController characterController = GetComponent<CharacterController>();
                if (characterController != null && characterController.isGrounded)
                {
                    raycastedObj.PlayAnimation();
                    Debug.Log("Animation is playing");
                }
            }
        }
        else
        {
            doOnce = false;
        }
    }
}
