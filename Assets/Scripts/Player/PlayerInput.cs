
using UnityEngine;

public struct FrameInput
{
    public Vector2 move;
    public bool jump;
    public bool lockShadow;
    public bool swap;
}

public class PlayerInput : MonoBehaviour
{
    public FrameInput frameInput;
    private KeyCode JumpKey = KeyCode.C;
    private KeyCode LockKey = KeyCode.X;
    private KeyCode SwapKey = KeyCode.Z;
    
    // Update is called once per frame
    void Update()
    {
        GatherInput();
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

    private void GatherInput()
    {
        frameInput = new FrameInput
        {
            move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")),
            jump = Input.GetKeyDown(JumpKey) || Input.GetKeyDown(KeyCode.Space),
            lockShadow = Input.GetKeyDown(LockKey)|| Input.GetKeyDown(KeyCode.E),
            swap = Input.GetKeyDown(SwapKey)|| Input.GetKeyDown(KeyCode.Q),
        };
    }

}
