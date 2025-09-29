using System;
using UnityEngine;

public class Lock : MonoBehaviour
{
    public string playerTag = "Player";
    private SpriteRenderer spriteRenderer;
    private PlayerLife _playerLife;
    protected PlayerAbilities _playerAbilities;
    private bool active = true;

    public AudioClip lockSound;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // respawn on death
        var player = GameObject.FindGameObjectWithTag(playerTag);
        _playerLife = player.GetComponent<PlayerLife>();
        _playerLife.onDeath.AddListener(EnableLock);
        _playerAbilities = player.GetComponent<PlayerAbilities>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && active && _playerLife.isAlive)
        {
            active = false;
            spriteRenderer.enabled = false;
            ProcessAbility();
        }
    }

    public virtual void ProcessAbility()
    {
        if (lockSound) AudioManager.instance.PlaySFX(lockSound, randomizePitch: true);
    }
    private void EnableLock()
    {
        active = true;
        spriteRenderer.enabled = true;
    }
}
