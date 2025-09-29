using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 1f;       // world units per second
    public float lifetime = 1f;
    public Vector3 moveDirection = new Vector3(0, 1, 0); // upward in world space
    public float fadeDuration = 0.5f;

    private TextMeshProUGUI textMesh;
    private float timer;

    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // Move upward in world space
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // Count lifetime
        timer += Time.deltaTime;

        // Fade out near the end
        if (timer > lifetime - fadeDuration)
        {
            float fadeAmount = 1 - ((timer - (lifetime - fadeDuration)) / fadeDuration);
            Color c = textMesh.color;
            c.a = fadeAmount;
            textMesh.color = c;
        }

        // Destroy after lifetime
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    public void SetText(string message, Color color)
    {
        if (textMesh == null) textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.text = message;
        textMesh.color = color;
    }
}