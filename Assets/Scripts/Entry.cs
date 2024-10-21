using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entry : MonoBehaviour
{
    private bool bgmTriggerred;
    public string bgmName;
    public bool isRadiationZone;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if(!bgmTriggerred)
            {
                AudioManager.Instance.PlayMusic(bgmName, true);
                bgmTriggerred = true;
            }
            other.gameObject.GetComponent<PlayerStatus>().inRadiationZone = isRadiationZone;
        }
    }
}
