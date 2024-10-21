using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    private PlayerControl playerControl;

    [Header("Check Foot")]
    public bool isOnGround;
    public bool isLeftOnGround;
    public bool isRightOnGround;

    [Header("Check Hand")]
    public bool isTouchLeft;
    public bool isTouchRight;
    public bool isTouchForward;

    [Header("Check Parameters")]
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public Vector2 forwardOffset;
    public float checkRadius;

    public LayerMask groundLayer;


    // Start is called before the first frame update
    void Start()
    {
        playerControl = GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        Check();
    }

    private void Check()
    {
        isLeftOnGround = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset * playerControl.faceDirection, checkRadius, groundLayer);
        isRightOnGround = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset * playerControl.faceDirection, checkRadius, groundLayer);

        isTouchForward = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(forwardOffset.x * playerControl.faceDirection, forwardOffset.y), checkRadius, groundLayer);
    
    }

    private void OnDrawGizmosSelected() 
    {
        // physics check circle
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset * playerControl.faceDirection, checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset * playerControl.faceDirection, checkRadius);

        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(forwardOffset.x * playerControl.faceDirection, forwardOffset.y), checkRadius);
    }

}
