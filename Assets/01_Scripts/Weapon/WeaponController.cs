using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField]
    protected GameObject[] weaponPrefabs;

    private Animator animator;
    private StateComponent stateComponent;

    private WeaponType currentType = WeaponType.Unarmed;
    private Dictionary<WeaponType, Weapon> weaponTable;

    public event Action OnAnimEquipEnd;
    public event Action OnAnimActionEnd;

    public bool IsUnarmed => currentType == WeaponType.Unarmed;
    public bool IsSword => currentType == WeaponType.Sword;
    public bool IsGreatSword => currentType == WeaponType.GreatSword;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        stateComponent = GetComponent<StateComponent>();
    }

    protected virtual void Start()
    {
        Start_InitWeaponTable();
    }

    private void Start_InitWeaponTable()
    {
        weaponTable = new Dictionary<WeaponType, Weapon>();

        for (int i = 0; i < (int)WeaponType.Max; i++)
        {
            weaponTable.Add((WeaponType)i, null);
        }

        foreach (GameObject t in weaponPrefabs)
        {
            GameObject go = Instantiate(t, transform);
            Weapon weapon = go.GetComponent<Weapon>();

            go.name = weapon.Type.ToString();
            weaponTable[weapon.Type] = weapon;
        }
    }

    #region SetWeapon
    private void SetUnarmed()
    {
        if (stateComponent.IsIdleState == false)
            return;

        animator.SetInteger("WeaponType", (int)WeaponType.Unarmed);

        if (weaponTable[currentType] != null)
        {
            weaponTable[currentType].Unequip();
        }

        currentType = WeaponType.Unarmed;
    }
    
    public void SetSword()
    {
        if (stateComponent.IsIdleState == false)
            return;

        SetWeaponType(WeaponType.Sword);
    }

    public void SetGreatSword()
    {
        if (stateComponent.IsIdleState == false)
            return;

        SetWeaponType(WeaponType.GreatSword);
    }

    public void SetKatana()
    {
        if (stateComponent.IsIdleState == false)
            return;
        
        SetWeaponType(WeaponType.Katana);
    }

    public void SetStaff()
    {
        if (stateComponent.IsIdleState == false)
            return;
        
        SetWeaponType(WeaponType.Staff);
    }
    
    public void SetWarp()
    {
        if (stateComponent.IsIdleState == false)
            return;
        
        SetWeaponType(WeaponType.Warp);
    }

    public void SetWeaponType(WeaponType newType)
    {
        if (currentType == newType)
        {
            animator.SetInteger("WeaponType", (int)newType);
            animator.SetBool("IsUnequipping", true);

            return;
        }
        else if (IsUnarmed == false)
        {
            weaponTable[currentType].Unequip();
        }

        if (weaponTable[newType] == null)
        {
            SetUnarmed();
        }

        animator.SetInteger("WeaponType", (int)newType);
        animator.SetBool("IsEquipping", true);

        currentType = newType;
    }
    #endregion

    public void DoAction()  // Start Combo
    {
        if (weaponTable[currentType] == null)
            return;

        animator.SetBool("IsAction", true);
        weaponTable[currentType].DoAction();
    }

    #region Equip
    private void BeginAnimEquip()
    {
        weaponTable[currentType].Equip();
    }

    private void EndAnimEquip()
    {
        animator.SetBool("IsEquipping", false);

        OnAnimEquipEnd?.Invoke();
    }

    private void BeginAnimUnequip()
    {
        weaponTable[currentType].Unequip();
    }

    private void EndAnimUnequip()
    {
        animator.SetBool("IsUnequipping", false);
        SetUnarmed();
    }
    #endregion

    #region Action
    private void DoAnimNextCombo()
    {
        weaponTable[currentType].DoNextCombo();
    }

    private void EndAnimAction()
    {
        animator.SetBool("IsAction", false);

        weaponTable[currentType].EndAction();

        OnAnimActionEnd?.Invoke();
    }

    private void BeginAnimComboInputSection()
    {
        MeleeWeapon meleeWeapon = weaponTable[currentType] as MeleeWeapon;
        meleeWeapon?.EnableCombo();
    }

    private void EndAnimComboInputSection()
    {
        MeleeWeapon meleeWeapon = weaponTable[currentType] as MeleeWeapon;
        meleeWeapon?.DisableCombo();
    }
    
    private void BeginAnimCollision(AnimationEvent evt)
    {
        MeleeWeapon meleeWeapon = weaponTable[currentType] as MeleeWeapon;

        if (string.IsNullOrEmpty(evt.stringParameter))
        {
            print($"Enable Collider {meleeWeapon}");
            meleeWeapon?.EnableCollision();
        }
        else
        {
            string[] strIndexes = evt.stringParameter.Split(",");
            
            foreach (string strIndex in strIndexes)
            {
                int index = int.Parse(strIndex);

                meleeWeapon?.EnableCollision(index);
            }
        }
    }

    private void EndAnimCollision()
    {
        MeleeWeapon meleeWeapon = weaponTable[currentType] as MeleeWeapon;
        meleeWeapon?.DisableCollision();
    }
    #endregion
    
    private void DoAnimProjectile()
    {
        weaponTable[currentType].ShootProjectile();
    }

    private void DoAnimFX()
    {
        weaponTable[currentType].ShowFX();
    }
}
