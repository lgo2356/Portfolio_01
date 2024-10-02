using UnityEngine;
using WeaponType = WeaponComponent.WeaponType;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    protected WeaponType weaponType;

    protected Animator animator;
    protected StateComponent stateComponent;

    protected GameObject rootObject;

    public WeaponType WeaponType => weaponType;

    protected virtual void Reset()
    {

    }

    protected virtual void Awake()
    {
        rootObject = transform.root.gameObject;
        Debug.Assert(rootObject != null);

        Awake_GetComponent();
    }

    private void Awake_GetComponent()
    {
        animator = rootObject.GetComponent<Animator>();
        stateComponent = rootObject.GetComponent<StateComponent>();
    }

    public virtual void Equip()
    {

    }

    public virtual void Unequip()
    {
        
    }
}
