using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        SetAnimation();
    }

    private void SetAnimation()
    {
        anim.SetFloat("speedX", rb.velocityX);
        anim.SetBool("isWalk", true);
    }

}
