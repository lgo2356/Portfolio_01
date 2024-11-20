using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField]
    protected GameObject weaponPrefab;

    private Animator animator;
    private StateComponent stateComponent;

    public WeaponType currentType = WeaponType.Unarmed;
    private Weapon currentWeapon;

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
        animator.SetBool("IsEquipping", false);
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
        MeleeWeapon meleeWeapon = currentWeapon as MeleeWeapon;
        meleeWeapon?.DisableCollision();
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
