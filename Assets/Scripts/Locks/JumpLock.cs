using UnityEngine;

public class JumpLock : Lock
{
    public override void ProcessAbility()
    {
        base.ProcessAbility();
        _playerAbilities.Jump();
    }
}
