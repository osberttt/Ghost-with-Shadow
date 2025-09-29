using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject room;
    public GameObject rescueText;
    public AudioClip sucessSound;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var playerLife = other.GetComponent<PlayerLife>();
            if (!playerLife.isAlive) return;
            var playerAbilities = other.GetComponent<PlayerAbilities>();
            playerAbilities.Key();
            Destroy(room);
            rescueText.SetActive(true);
            AudioManager.instance.PlaySFX(sucessSound);
            Destroy(gameObject);
        }
    }
}
