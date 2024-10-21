// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Events;

// public class CharacterStatus : MonoBehaviour
// {
//     // [Header("Invulnerability Setting")]
//     // public bool isInvulnerable;
//     // public float invulnerableTime;
//     // public float invulnerableCounter;

//     private SpriteRenderer sr;
//     private Color color;

//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }

//     public void TakeDamage()
//     {
//         ColorFlash(0.2f);
//         // AudioManager.Instance.PlayMusic("punch", false);

//     }

//     public void ColorFlash(float time)
//     {
//         Color flashColor = Color.red;
//         flashColor.a = 0.5f;
//         sr.color = flashColor;
//         Invoke("ResetColor", time);
//     }

//     public void ResetColor()
//     {
//         sr.color = color;
//     }
// }
