using System;
using UnityEngine;

public class ShadowController : MonoBehaviour
{
    public PlayerController playerController;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!playerController.lockShadow) spriteRenderer.color = new Color(0f, 0f, 0f, 1f);
        else spriteRenderer.color = new Color(0f, 0f, 0f, 0.5f);
        
        transform.localScale = playerController.transform.localScale;
    }

    public void MoveAlong(Vector2 moveAmount)
    {
        transform.position += (Vector3)moveAmount;
    }
}