using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public CapsuleCollider2D col;
    public LayerMask groundMask;

    public bool isGrounded = false;

    [Header("Gizmo parameters:")]
    [Range(-2f, 2f)]
    public float boxCastYOffset = -0.1f;
    [Range(-2f, 2f)]
    public float boxCastXOffset = 0f;
    [Range(0, 2)]
    public float boxCastWidth = 0.5f, boxCastHeight = 0.5f;
    public Color gizmoColorNotGrounded = Color.red, gizmoColorIsGrounded = Color.green;


    private void Awake()
    {
        col = GetComponent<CapsuleCollider2D>();
    }

    public void CheckIsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(col.bounds.center + new Vector3(boxCastXOffset, boxCastYOffset, 0), new Vector2(boxCastWidth, boxCastHeight), 0, Vector2.down, 0, groundMask);
        if (raycastHit.collider != null)
        {

            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (col == null)
            return;
        Gizmos.color = gizmoColorNotGrounded;
        if (isGrounded == true)
            Gizmos.color = gizmoColorIsGrounded;

        Gizmos.DrawWireCube(col.bounds.center + new Vector3(boxCastXOffset, boxCastYOffset, 0),
            new Vector3(boxCastWidth, boxCastHeight));

    }

    private void Update()
    {
        CheckIsGrounded();
    }
}
