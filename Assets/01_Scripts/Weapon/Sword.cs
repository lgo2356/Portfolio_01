using UnityEngine;
using WeaponType = WeaponComponent.WeaponType;

public class Sword : Weapon
{
    [SerializeField]
    private string slotName = "Slot_Sword";

    [SerializeField]
    private string handName = "Hand_Sword";

    private Transform slotTransform;
    private Transform handTransform;

    protected override void Reset()
    {
        base.Reset();

        weaponType = WeaponType.Sword;
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

    public override void Equip()  // 등 -> 손
    {
        base.Equip();

        transform.parent.DetachChildren();
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.SetParent(handTransform, false);
    }

    public override void Unequip()  // 손 -> 등
    {
        base.Unequip();

        transform.parent.DetachChildren();
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.SetParent(slotTransform, false);
    }
}
