using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    private PlayerControl player;

    [Header("Player Health")]
    public float maxHealth;
    public float currentHealth;

    [Header("Player Sanity")]
    public bool inRadiationZone = false;
    public Slider sanityBar;
    public int maxSanity;
    public int currentSanity;
    public float sanityTime;
    private float baseSpeed;
    private float timer;

    [Header("Invulnerability Setting")]
    public bool isInvulnerable;
    public float invulnerableTime;
    public float invulnerableCounter;

    public GameObject deathScene;
    public VoidEventSO cameraShakeSO;
    public VoidEventSO newGameEventSO;


    private void Awake() 
    {
        player = GetComponent<PlayerControl>();
        currentHealth = maxHealth;
        deathScene.SetActive(false);
        baseSpeed = player.speed;
        SetSanity();
        timer = 0;
    }

    private void OnEnable() 
    {
        newGameEventSO.OnEventRaised += NewGame;
    }

    private void OnDisable() 
    {
        newGameEventSO.OnEventRaised -= NewGame;
    }

    void Update()
    {   
        // count invulnerable time
        if(isInvulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if(invulnerableCounter <= 0)
            {
                isInvulnerable = false;
            }
        }
        if(inRadiationZone)
        {
            timer += Time.deltaTime;
            if(timer >= sanityTime)
            {
                SetSanity(-5);
                timer = 0;
            }
        }
    }

    public void NewGame()
    {
        Debug.Log("NEW GAME");
        currentHealth = maxHealth;
        // sanity
        currentSanity = maxSanity - 10;

        player.isDead = false;
        
    }

    public void TakeDamage(Vector2 attackerPos, int damage) // can use unity event later
    {   
        if(isInvulnerable)
            return;

        Debug.Log(damage);
        // decrease health
        if(currentHealth - damage > 0)
        {
            currentHealth -= damage;
            AudioManager.Instance.PlayMusic("player hurt", false);
            player.GetHurt(attackerPos);
        }
        else
        {
            currentHealth = 0;
            player.Die();
            AudioManager.Instance.StopBGM();
            AudioManager.Instance.PlayMusic("bruh", false);
            
            // Time.timeScale = 0f;
            deathScene.SetActive(true);
        }

        cameraShakeSO.RaiseEvent();

        TriggerInvulnerability();

        UIManager.Instance.SetText("Health", currentHealth.ToString());
    }

    public void TriggerInvulnerability()
    {
        if(!isInvulnerable)
        {
            isInvulnerable = true;
            invulnerableCounter = invulnerableTime;
        }
        
    }

    public void HealPlayer(int health = 1)
    {
        currentHealth += health;
        currentHealth = (int) Mathf.Clamp(currentHealth, 0, maxHealth);
        UIManager.Instance.SetText("Health", currentHealth.ToString());
    }

    public void SetSanity(int sanity = 10)
    {
        currentSanity += sanity;

        if(currentSanity < 0)
        {
            currentSanity = 0;
            if(currentHealth >= 1)
                currentHealth -= 1;
        }
        if(currentSanity > 100)
        {
            currentSanity = 100;
        }
        
        if(currentSanity > 90)
        {
            player.speed = baseSpeed * 1.5f;
            
        }
        if(currentSanity > 60 && timer >= sanityTime)
        {
            HealPlayer();
        }
        if(currentSanity < 30)
        {
            player.speed = baseSpeed * 0.7f;
        }
        // reset to base speed for normal sanity
        if(currentSanity <= 90 && currentSanity >= 30)
        {
            player.speed = baseSpeed;
        }
        sanityBar.GetComponent<SanityBar>().SetValue(currentSanity);
    }
}
