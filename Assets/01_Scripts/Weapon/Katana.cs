using UnityEngine;

public class Katana : MeleeWeapon
{
    [SerializeField] 
    private string slotName = "Slot_Katana";

    [SerializeField]
    private string handName = "Hand_Katana";

    [SerializeField]
    private GameObject slashFX;

    private Transform slotTransform;
    private Transform handTransform;

    protected override void Reset()
    {
        base.Reset();

        type = WeaponType.Katana;
    }

    protected override void Awake()
    {
        base.Awake();

        slotTransform = rootObject.transform.FindChildByName(slotName);
        Debug.Assert(slotTransform != null, "slotTransform can be null");

        transform.SetParent(slotTransform, false);

        handTransform = rootObject.transform.FindChildByName(handName);
        Debug.Assert(handTransform != null, "handTransform can be null");
    }

    public override void Equip()
    {
        base.Equip();
        
        transform.parent.DetachChildren();
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.SetParent(handTransform,false);
    }

    public override void Unequip()
    {
        base.Unequip();
        
        transform.parent.DetachChildren();
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.SetParent(slotTransform,false);
    }

    public override void ShowSlashFX()
    {
        base.ShowSlashFX();

        GameObject go = Instantiate(slashFX, transform);
        go.transform.parent.DetachChildren();
    }
}
