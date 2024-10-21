using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Robot : MonoBehaviour, IInteractable
{
    public Dialogue dialogue;

    public void TriggerAction()
    {
        UIManager.Instance.NextDialogue(dialogue);
        // AudioManager.Instance.PlayMusic("fake speech", false);
    }
}
