using UnityEngine;

public class RightLock : Lock
{
    public override void ProcessAbility()
    {
        base.ProcessAbility();
        _playerAbilities.Right();
    }
}