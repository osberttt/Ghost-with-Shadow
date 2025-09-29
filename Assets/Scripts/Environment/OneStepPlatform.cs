using UnityEngine;
using System.Collections;

/// <summary>
/// One-step platform (Celeste-like):
/// - When the player steps ON TOP of it, it disappears.
/// - After a delay, it respawns only if no player is inside its space.
/// 
/// Requirements:
/// - Put this on a GameObject with a BoxCollider2D (non-trigger).
/// - Optionally add a SpriteRenderer (or children) for visuals.
/// - Tag your player "Player" OR set the playerLayer mask.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class OneStepPlatform : MonoBehaviour
{
    [Header("Triggering")]
    [Tooltip("Tag used to identify the player (optional if using Player Layer).")]
    public string playerTag = "Player";

    [Tooltip("Layer(s) considered as player for overlap checks (recommended).")]
    public LayerMask playerLayer;

    [Header("Timings")]
    [Tooltip("Delay before the platform disappears after being stepped on.")]
    public float disappearDelay = 0.05f;

    [Tooltip("Base delay before attempting to respawn (will still wait until clear).")]
    public float respawnDelay = 1.25f;

    [Header("Respawn Safety Check")]
    [Tooltip("Extra padding around the platform bounds when checking if player is inside on respawn.")]
    public Vector2 respawnCheckPadding = new Vector2(0.05f, 0.05f);

    [Tooltip("If true, the platform will ignore subsequent triggers while disappearing/respawning.")]
    public bool lockWhileProcessing = true;

    [Header("Visuals (optional)")]
    [Tooltip("Optional set of renderers to hide/show. If empty, will search children.")]
    public Renderer[] renderersToToggle;

    [Tooltip("Optional: play this when disappearing.")]
    public AudioSource disappearSFX;

    [Tooltip("Optional: play this when reappearing.")]
    public AudioSource respawnSFX;

    private BoxCollider2D _box;
    private Collider2D _solidCollider; // main collider to disable
    private bool _isProcessing;
    private Vector2 _worldCenter;
    private Vector2 _worldSize;
    
    private PlayerLife _playerLife;

    void Awake()
    {
        // respawn on death
        _playerLife = GameObject.FindGameObjectWithTag(playerTag).GetComponent<PlayerLife>();
        _playerLife.onDeath.AddListener(Respawn);
        
        _box = GetComponent<BoxCollider2D>();
        _solidCollider = _box;

        if (renderersToToggle == null || renderersToToggle.Length == 0)
            renderersToToggle = GetComponentsInChildren<Renderer>(true);
    }

    void OnEnable()
    {
        CacheWorldBounds();
    }

    // Recompute world-space center/size from BoxCollider2D (works even if we later disable collider)
    private void CacheWorldBounds()
    {
        // Convert box offset/size to world space
        _worldCenter = (Vector2)transform.TransformPoint(_box.offset);
        var lossy = transform.lossyScale;
        _worldSize = new Vector2(_box.size.x * Mathf.Abs(lossy.x), _box.size.y * Mathf.Abs(lossy.y));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (lockWhileProcessing && _isProcessing) return;

        // Quick player checks
        bool isPlayerByTag = !string.IsNullOrEmpty(playerTag) && collision.collider.CompareTag(playerTag);
        bool isPlayerByLayer = ((1 << collision.collider.gameObject.layer) & playerLayer) != 0;

        if (!isPlayerByTag && !isPlayerByLayer) return;

        var topY = _box.bounds.max.y;
        var bottomY = collision.collider.bounds.min.y;

// Use a small tolerance to account for contact offset & penetration.
        float tol = Physics2D.defaultContactOffset + 0.03f;  // tweak as needed
        bool landedOnTop = bottomY >= topY - tol;

        Debug.Log($"bottomY={bottomY:F3}, topY={topY:F3}, tol={tol:F3}, landed={landedOnTop}");
        if (!landedOnTop) return;

        
        StartCoroutine(DisappearAndRespawn());
    }

    private IEnumerator DisappearAndRespawn()
    {
        Debug.Log("Disappear");
        _isProcessing = true;

        if (disappearDelay > 0f)
            yield return new WaitForSeconds(disappearDelay);

        // Hide & disable solidity
        SetVisible(false);
        SetSolid(false);
        if (disappearSFX) disappearSFX.Play();
        
        _isProcessing = false;
    }

    public void Respawn()
    {
        // Respawn
        SetSolid(true);
        SetVisible(true);
        if (respawnSFX) respawnSFX.Play();
    }
    private void SetSolid(bool solid)
    {
        if (_solidCollider) _solidCollider.enabled = solid;
    }

    private void SetVisible(bool visible)
    {
        if (renderersToToggle == null) return;
        foreach (var r in renderersToToggle)
            if (r) r.enabled = visible;
    }

    // Optional: visualize the respawn check volume in the editor
    void OnDrawGizmosSelected()
    {
        if (!_box) _box = GetComponent<BoxCollider2D>();
        CacheWorldBounds();
        Vector2 checkSize = _worldSize + respawnCheckPadding * 2f;

        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.color = new Color(0f, 1f, 0.7f, 0.25f);
        Gizmos.DrawCube(_worldCenter, checkSize);
        Gizmos.color = new Color(0f, 1f, 0.7f, 1f);
        Gizmos.DrawWireCube(_worldCenter, checkSize);
    }
}
