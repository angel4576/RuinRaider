using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Animator anim;

    [Header("Explosion Setting")]
    public float explodeTime;
    public float destroyTime;

    private void Awake() 
    {
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Explode());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Explode()
    {
        yield return new WaitForSeconds(explodeTime);

        anim.SetTrigger("explode");

        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }

    public void PlayBombSFX() { AudioManager.Instance.PlayMusic("boom", false); }

}
