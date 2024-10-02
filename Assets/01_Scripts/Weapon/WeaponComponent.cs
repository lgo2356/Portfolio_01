using System.Collections.Generic;
using UnityEngine;

public class WeaponComponent : MonoBehaviour
{
    public enum WeaponType
    {
        Unarmed = 0,
        Fist, Sword, GreatSword,
        Max,
    }

    [SerializeField]
    private GameObject[] weaponPrefabs;

    private Animator animator;
    private StateComponent stateComponent;

    private WeaponType currentWeaponType = WeaponType.Unarmed;
    private Dictionary<WeaponType, Weapon> weaponTable;

    public bool IsUnarmed { get => currentWeaponType == WeaponType.Unarmed; }
    public bool IsSword { get => currentWeaponType == WeaponType.Sword; }
    public bool IsGreateSword { get => currentWeaponType == WeaponType.GreatSword; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        stateComponent = GetComponent<StateComponent>();
    }

    private void Start()
    {
        Start_InitWeaponTable();
    }

    private void Start_InitWeaponTable()
    {
        weaponTable = new();

        for (int i = 0; i < (int)WeaponType.Max; i++)
        {
            weaponTable.Add((WeaponType)i, null);
        }

        for (int i = 0; i < weaponPrefabs.Length; i++)
        {
            GameObject go = Instantiate(weaponPrefabs[i], transform);
            Weapon weapon = go.GetComponent<Weapon>();

            go.name = weapon.WeaponType.ToString();
            weaponTable[weapon.WeaponType] = weapon;
        }
    }

    #region SetWeapon
    private void SetUnarmed()
    {
        if (stateComponent.IsIdleState == false)
            return;

        animator.SetInteger("WeaponType", (int)WeaponType.Unarmed);

        if (weaponTable[currentWeaponType] != null)
        {
            weaponTable[currentWeaponType].Unequip();
        }

        currentWeaponType = WeaponType.Unarmed;
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

    private void SetWeaponType(WeaponType newType)
    {
        if (currentWeaponType == newType)
        {
            animator.SetInteger("WeaponType", (int)newType);
            animator.SetBool("IsUnequipping", true);

            return;
        }
        else if (IsUnarmed == false)
        {
            weaponTable[currentWeaponType].Unequip();
        }

        if (weaponTable[newType] == null)
        {
            SetUnarmed();
        }

        animator.SetInteger("WeaponType", (int)newType);
        animator.SetBool("IsEquipping", true);

        currentWeaponType = newType;
    }
    #endregion

    #region 공격
    public void DoAction()  // Start Combo
    {
        if (weaponTable[currentWeaponType] == null)
            return;

        animator.SetBool("IsAction", true);
        weaponTable[currentWeaponType].DoAction();
    }
    #endregion

    #region 애니메이션 이벤트
    private void BeginAnimEquip()
    {
        weaponTable[currentWeaponType].Equip();
    }

    private void EndAnimEquip()
    {
        animator.SetBool("IsEquipping", false);
    }

    private void BeginAnimUnequip()
    {
        weaponTable[currentWeaponType].Unequip();
    }

    private void EndAnimUnequip()
    {
        animator.SetBool("IsUnequipping", false);
        SetUnarmed();
    }

    private void DoAnimNextCombo()
    {
        weaponTable[currentWeaponType].DoNextCombo();
    }

    private void EndAnimAction()
    {
        animator.SetBool("IsAction", false);
        weaponTable[currentWeaponType].EndAction();
    }

    private void BeginAnimComboInputSection()
    {
        Sword sword = weaponTable[currentWeaponType] as Sword;
        sword?.EnableCombo();
    }

    private void EndAnimComboInputSection()
    {
        Sword sword = weaponTable[currentWeaponType] as Sword;
        sword?.DisableCombo();
    }
    #endregion
}
