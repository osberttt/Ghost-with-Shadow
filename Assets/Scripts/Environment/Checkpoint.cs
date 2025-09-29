using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Canvas canvas;           
    public GameObject floatingTextPrefab;
    public Vector3 textSpawnOffset = new Vector3(0, 0f, 0); // spawn a bit above checkpoint

    public AudioClip checkpointSound;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var playerLife = other.GetComponent<PlayerLife>();
            if (playerLife.checkpoint == transform) return;
            AudioManager.instance.PlaySFX(checkpointSound);
            playerLife.checkpoint = transform;

            // Spawn floating text at world position (no WorldToScreenPoint!)
            Vector3 spawnPos = transform.position + textSpawnOffset;
            var text = Instantiate(floatingTextPrefab, spawnPos, Quaternion.identity, canvas.transform);

            text.GetComponent<FloatingText>().SetText("Checkpoint saved!", Color.black);
        }
    }
}