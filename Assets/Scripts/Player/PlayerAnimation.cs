using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private PlayerControl playerControl;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;

    private void Awake() 
    {
        anim = GetComponent<Animator>();
        playerControl = GetComponent<PlayerControl>();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetAnimation();
    }

    private void SetAnimation()
    {
        anim.SetFloat("speedX", math.abs(rb.velocity.x));
        anim.SetFloat("speedY", rb.velocity.y);
        
        anim.SetBool("isOnGround", physicsCheck.isLeftOnGround || physicsCheck.isRightOnGround);
        anim.SetBool("isAttack", playerControl.isAttack);
        anim.SetBool("isDash", playerControl.isDash);

        anim.SetBool("isHurt", playerControl.isHurt);
        anim.SetBool("isDead", playerControl.isDead);

    }

    public void PlayHurt(){anim.SetTrigger("hurt");}

    public void PlayAttack(){anim.SetTrigger("attack");}

    public void PlayHeavyAttack(){anim.SetTrigger("heavyAttack");}

    public void PlayDash(){anim.SetTrigger("dash");}


    public void HurtSFX(){AudioManager.Instance.PlayMusic("player hurt", false);}

    public void LightAttackSFX()
    {   
        //if(!playerControl.isHit)
        AudioManager.Instance.PlayMusic("attack1", false);
    }
    public void HeavyAttackSFX(){AudioManager.Instance.PlayMusic("attack2", false);}

    public void DashSFX(){AudioManager.Instance.PlayMusic("dash", false);}

    public void JumpSFX(){AudioManager.Instance.PlayMusic("jump", false);}

    public void DeathSFX(){AudioManager.Instance.PlayMusic("bruh", false);}
}
