using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    
    void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 pos = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        transform.position = pos;
    }

}
