using System;
using System.Collections;
using UnityEngine;

public class OneStepSpike : MonoBehaviour
{
    public string playerTag = "Player";
    public GameObject fakeSpike;
    public GameObject actualSpike;
    public float spikeDelay = 1f;
    private PlayerLife _playerLife;
    private void Awake()
    {
        // respawn on death
        _playerLife = GameObject.FindGameObjectWithTag(playerTag).GetComponent<PlayerLife>();
        _playerLife.onDeath.AddListener(DisableSpike);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            if (fakeSpike.activeSelf && actualSpike.activeSelf) return;
            StartCoroutine(EnableSpikeCo());
        }
    }

    private IEnumerator EnableSpikeCo()
    {
        yield return new WaitForSeconds(spikeDelay);
        fakeSpike.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        actualSpike.SetActive(true);
    }
    private void DisableSpike()
    {
        StartCoroutine(DisableSpikeCo());
    }
    
    private IEnumerator DisableSpikeCo()
    {
        yield return new WaitForSeconds(spikeDelay);
        fakeSpike.SetActive(false);
        actualSpike.SetActive(false);
    }
}
