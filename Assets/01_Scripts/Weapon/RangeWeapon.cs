using UnityEngine;

public class RangeWeapon : Weapon
{
    public override void DoAction()
    {
        base.DoAction();
        
        SetPlayerMove();
    }
}
