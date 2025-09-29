using UnityEngine;

public class PlayerText : MonoBehaviour
{
    public Canvas canvas;           
    public GameObject floatingTextPrefab;
    public Vector3 textSpawnOffset = new Vector3(0, 0.5f, 0); // spawn a bit above checkpoint
    
    public void FloatText(string textToFloat)
    {
        // Spawn floating text at world position (no WorldToScreenPoint!)
        Vector3 spawnPos = transform.position + textSpawnOffset;
        var text = Instantiate(floatingTextPrefab, spawnPos, Quaternion.identity, canvas.transform);

        text.GetComponent<FloatingText>().SetText(textToFloat, Color.black);
    }
}
