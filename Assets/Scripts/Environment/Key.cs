using System;
using UnityEngine;

public class Key : MonoBehaviour
{
    public bool isActive = true;
    private SpriteRenderer spriteRenderer;
    private PlayerLife _playerLife;
    private PlayerAbilities _playerAbilities;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // respawn on death
        var player = GameObject.FindGameObjectWithTag("Player");
        _playerLife = player.GetComponent<PlayerLife>();
        _playerLife.onDeath.AddListener(EnableKey);
        _playerAbilities = player.GetComponent<PlayerAbilities>();
    }

    private void EnableKey()
    {
        spriteRenderer.enabled = true;
        isActive = true;
        if (_playerAbilities.hasKey) _playerAbilities.Key();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isActive)
        {
            var playerAbilities = other.GetComponent<PlayerAbilities>();
            playerAbilities.Key();
            isActive = false;
            spriteRenderer.enabled = false;
        }
    }
}
