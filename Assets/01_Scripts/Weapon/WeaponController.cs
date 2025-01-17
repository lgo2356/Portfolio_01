using System;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField]
    protected GameObject weaponPrefab;

    private Animator animator;
    private StateComponent stateComponent;

    private Weapon currentWeapon;
    private bool isEquipping;

    public event Action<Weapon> OnWeaponChanged;

    public Weapon CurrentWeapon => currentWeapon;
    public WeaponType currentType = WeaponType.Unarmed;
    public bool IsUnarmed => currentType == WeaponType.Unarmed;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        stateComponent = GetComponent<StateComponent>();
    }

    protected virtual void Start()
    {
        InitWeapon();
    }

    private void InitWeapon()
    {
        GameObject go = Instantiate(weaponPrefab, transform);
        Weapon weapon = go.GetComponent<Weapon>();
        {
            go.name = weapon.Type.ToString();
        }

        currentWeapon = weapon;

        OnWeaponChanged?.Invoke(currentWeapon);
    }

    private void SetUnarmed()
    {
        if (stateComponent.IsIdleState == false)
            return;

        animator.SetInteger("WeaponType", (int)WeaponType.Unarmed);

        currentWeapon.Unequip();
        currentType = WeaponType.Unarmed;
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
            currentWeapon.Unequip();
        }

        animator.SetInteger("WeaponType", (int)newType);
        animator.SetBool("IsEquipping", true);

        isEquipping = true;
        currentType = newType;
    }

    public void Equip()
    {
        if (stateComponent.IsIdleState == false)
            return;

        SetWeaponType(currentWeapon.Type);
    }

    public void Unequip()
    {

    }
    
    public void DoAction()  // Start Combo
    {
        if (currentType == WeaponType.Unarmed)
            return;

        if (isEquipping)
            return;

        if (stateComponent.IsDamagedState)
            return;

        animator.SetBool("IsAction", true);
        currentWeapon.DoAction();
    }

    public void EndAction()
    {
        animator.SetBool("IsAction", false);

        currentWeapon.EndAction();
    }

    #region Animation Equip
    private void BeginAnimEquip()
    {
        currentWeapon.Equip();
    }

    private void EndAnimEquip()
    {
        // 애니메이션 파라미터 초기화
        animator.SetBool("IsEquipping", false);
        isEquipping = false;
    }

    private void BeginAnimUnequip()
    {
        currentWeapon.Unequip();
    }

    private void EndAnimUnequip()
    {
        animator.SetBool("IsUnequipping", false);
        SetUnarmed();
    }
    #endregion

    #region Animation Action
    private void DoAnimNextCombo()
    {
        currentWeapon.DoNextCombo();
    }

    private void EndAnimAction()
    {
        EndAction();
    }

    private void BeginAnimComboInputSection()
    {
        MeleeWeapon meleeWeapon = currentWeapon as MeleeWeapon;
        meleeWeapon?.EnableCombo();
    }

    private void EndAnimComboInputSection()
    {
        MeleeWeapon meleeWeapon = currentWeapon as MeleeWeapon;
        meleeWeapon?.DisableCombo();
    }
    
    private void BeginAnimCollision(AnimationEvent evt)
    {
        MeleeWeapon meleeWeapon = currentWeapon as MeleeWeapon;

        if (evt.intParameter != 0)
        {
            meleeWeapon.ComboIndex = evt.intParameter;
        }

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

        meleeWeapon.ShowSlashFX();
    }

    private void EndAnimCollision()
    {
        MeleeWeapon meleeWeapon = currentWeapon as MeleeWeapon;
        meleeWeapon?.DisableCollision();

        meleeWeapon.ComboIndex = 0;
    }
    #endregion
    
    private void DoAnimProjectile()
    {
        currentWeapon.ShootProjectile();
    }

    private void DoAnimFX()
    {
        currentWeapon.ShowFX();
    }
}
