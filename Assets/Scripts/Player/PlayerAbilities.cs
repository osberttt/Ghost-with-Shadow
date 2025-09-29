using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilities : MonoBehaviour
{
    public bool canGoLeft = true;
    public bool canGoRight = true;
    public bool canJump = true;
    public bool hasKey = false;

    public Transform iconHolder;
    public GameObject icon;
    
    public Sprite leftSprite;
    public Sprite rightSprite;
    public Sprite jumpSprite;
    public Sprite keySprite;
    
    private GameObject leftIcon;
    private GameObject rightIcon;
    private GameObject jumpIcon;
    private GameObject keyIcon;

    public void Jump()
    {
        if (jumpIcon == null)
        {
            var i = Instantiate(icon, iconHolder);
            i.GetComponent<Image>().sprite = jumpSprite;
            jumpIcon = i;
        }
        else
        {
            Destroy(jumpIcon);
        }
        canJump = !canJump;
    }

    public void Left()
    {
        if (leftIcon == null)
        {
            var i = Instantiate(icon, iconHolder);
            i.GetComponent<Image>().sprite = leftSprite;
            leftIcon = i;
        }
        else
        {
            Destroy(leftIcon);
        }
        canGoLeft = !canGoLeft;
    }

    public void Right()
    {
        if (rightIcon == null)
        {
            var i = Instantiate(icon, iconHolder);
            i.GetComponent<Image>().sprite = rightSprite;
            rightIcon = i;
        }
        else
        {
            Destroy(rightIcon);
        }
        canGoRight = !canGoRight;
    }

    public void Key()
    {
        if (keyIcon == null)
        {
            var i = Instantiate(icon, iconHolder);
            i.GetComponent<Image>().sprite = keySprite;
            keyIcon = i;
        }
        else
        {
            Destroy(keyIcon);
        }
        hasKey = !hasKey;
    }
}
