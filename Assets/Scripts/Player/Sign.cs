using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sign : MonoBehaviour
{
    public Transform player;
    public GameObject signSprite;
    private bool canPress;
    private PlayerInputControl playerInput;
    private IInteractable targetItem;

    private void Awake() 
    {
        playerInput = new PlayerInputControl();
        playerInput.Enable();

        playerInput.Gameplay.Confirm.started += OnComfirm;
    }

    private void OnEnable() 
    {
        
    }

    private void OnDisable() 
    {
        // canPress = false;
    }

    private void OnComfirm(InputAction.CallbackContext context)
    {
        if(canPress)
        {
            targetItem.TriggerAction(); // trigger corresponding action in different scripts
        }
    }

    // Update is called once per frame
    void Update()
    {
        signSprite.SetActive(canPress);
        signSprite.transform.localScale = player.localScale;
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if(other.CompareTag("Interactable"))
        {
            canPress = true;
            targetItem = other.GetComponent<IInteractable>();
        }

        if(other.CompareTag("Transition"))
        {
            targetItem = other.GetComponent<IInteractable>();
            targetItem.TriggerAction();
        }

    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        canPress = false;
    }

}
