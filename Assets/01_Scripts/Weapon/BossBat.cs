using UnityEngine;

public class BossBat : MeleeWeapon
{
    [SerializeField]
    private string handName = "Hand_BossBat";

    private Transform handTransform;
    
    protected override void Reset()
    {
        base.Reset();
        
        type = WeaponType.BossBat;
    }
    
    protected override void Awake()
    {
        base.Awake();

        handTransform = rootObject.transform.FindChildByName(handName);
        Debug.Assert(handTransform != null);
        
        transform.SetParent(handTransform, false);
    }
}
