using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerLife : MonoBehaviour
{
    public Transform checkpoint; 
    public bool isAlive = true;
    public float respawnDelay = 2f;
    public Vector3 shadowOffset = new Vector3(1f, 0f, 0f);
    
    private SpriteRenderer sr;
    private PlayerController playerController;
    private PlayerAbilities playerAbilities;
    public Transform shadow;

    public GameObject explodeParticles;

    public UnityEvent onDeath;
    public AudioClip explodeSound;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();
        playerAbilities = GetComponent<PlayerAbilities>();
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.R))
        {
            Die();
        }*/
    }

    public void Die()
    {
        // Spawn player explosion
        var playerParticles = Instantiate(explodeParticles, playerController.transform.position, Quaternion.identity);
        var playerMain = playerParticles.GetComponent<ParticleSystem>().main;
        playerMain.startColor = new Color(238f/255f, 219f/255f, 209f/255f, 1f); 
        Destroy(playerParticles, playerMain.duration);
        
        // Explode sound
        AudioManager.instance.PlaySFX(explodeSound, randomizePitch: true, pitch1:0.9f, pitch2:1.1f);
        
        // Spawn shadow explosion
        var shadowParticles = Instantiate(explodeParticles, shadow.transform.position, Quaternion.identity);
        var shadowMain = shadowParticles.GetComponent<ParticleSystem>().main;
        shadowMain.startColor = Color.black; // opaque black
        Destroy(shadowParticles, shadowMain.duration);
        
        onDeath?.Invoke();
        isAlive = false;
        sr.enabled = false;
        shadow.GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(RespawnCo());
    }

    public IEnumerator RespawnCo()
    {
        yield return new WaitForSecondsRealtime(respawnDelay);
        isAlive = true;
        sr.enabled = true;
        shadow.GetComponent<SpriteRenderer>().enabled = true;
        var respawnPosition = checkpoint.position + new Vector3(0f, -1f, 0f);
        transform.position = respawnPosition;
        shadow.position = respawnPosition + shadowOffset;
        playerController.latestPosition = respawnPosition;
        
        // abilities reset
        if (!playerAbilities.canJump) playerAbilities.Jump();
        if (!playerAbilities.canGoLeft) playerAbilities.Left();
        if (!playerAbilities.canGoRight) playerAbilities.Right();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isAlive) return;
        if (other.CompareTag("Spike"))
        {
            Die();
        }
    }
}
