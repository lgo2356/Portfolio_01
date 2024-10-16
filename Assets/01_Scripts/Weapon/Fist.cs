using UnityEngine;
using WeaponType = WeaponComponent.WeaponType;

public class Fist : MeleeWeapon
{
    private enum FistPart
    {
        LeftHand, RightHand, LeftFoot, RightFoot,
        Max,
    }

    protected override void Reset()
    {
        base.Reset();

        weaponType = WeaponType.Fist;
    }

    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < (int)FistPart.Max; i++)
        {
            transform.DetachChildren();

            Transform childrenTransform = colliders[i].transform;
            {
                childrenTransform.localPosition = Vector3.zero;
                childrenTransform.localRotation = Quaternion.identity;
            }

            string partName = ((FistPart)i).ToString();
            Transform parent = rootObject.transform.FindChildByName(partName);

            Debug.Assert(parent != null);

            childrenTransform.SetParent(parent, false);
        }
    }
}
