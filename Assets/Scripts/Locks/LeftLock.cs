using UnityEngine;

public class LeftLock : Lock
{
    public override void ProcessAbility()
    {
        base.ProcessAbility();
        _playerAbilities.Left();
    }
}