using UnityEngine;

public class GreatSword : MeleeWeapon
{
    [SerializeField]
    private string slotName = "Slot_GreatSword";

    [SerializeField]
    private string handName = "Hand_GreatSword";

    private Transform slotTransform;
    private Transform handTransform;

    protected override void Reset()
    {
        base.Reset();

        type = WeaponType.GreatSword;
    }

    protected override void Awake()
    {
        base.Awake();

        slotTransform = rootObject.transform.FindChildByName(slotName);
        Debug.Assert(slotTransform != null);

        handTransform = rootObject.transform.FindChildByName(handName);
        Debug.Assert(handTransform != null);

        transform.SetParent(slotTransform, false);
    }

    public override void Equip()
    {
        base.Equip();

        transform.parent.DetachChildren();
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.SetParent(handTransform, false);
    }

    public override void Unequip()
    {
        base.Unequip();

        transform.parent.DetachChildren();
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.SetParent(slotTransform, false);
    }
}
