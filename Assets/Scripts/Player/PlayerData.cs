using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public float GroundSpeed = 7f;
    public float AirSpeed = 5f;
    public float GroundAcceleration = 3f;
    public float AirAccerlation = 2.5f;
    public float JumpPower = 12f;
    public float JumpBuffer = 0.2f;
    public float CoyoteTime = 0.15f;
    public float GravityScale = 3f;
    public float FallMultiplier = 1.5f; //gravity multiplier when falling
    public float maxFallSpeed = 3f;
}