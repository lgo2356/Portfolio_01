using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character, IDamagable
{
    [SerializeField]
    private Transform bodyTransform;

    private WeaponController weaponController;

    private void Reset()
    {
        GameObject go = GameObject.Find("Player");
        Debug.Assert(go != null);

        bodyTransform = go.transform.FindChildByName("Body");
        Debug.Assert(bodyTransform != null);
    }

    protected override void Awake()
    {
        base.Awake();

        Awake_GetComponent();
        Awake_BindInput();
    }

    private void Awake_GetComponent()
    {
        weaponController = GetComponent<WeaponController>();
    }

    private void Awake_BindInput()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        InputActionMap actionMap = playerInput.actions.FindActionMap("PlayerActions");

        actionMap.FindAction("Equip_Sword").started += (callback) =>
        {
            weaponController.SetSword();
        };

        actionMap.FindAction("Equip_GreatSword").started += (callback) =>
        {
            weaponController.SetGreatSword();
        };

        actionMap.FindAction("Equip_Katana").started += (callback) =>
        {
            weaponController.SetKatana();
        };

        actionMap.FindAction("Equip_Staff").started += callback =>
        {
            weaponController.SetStaff();
        };
        
        actionMap.FindAction("Equip_Warp").started += callback =>
        {
            weaponController.SetWarp();
        };

        actionMap.FindAction("Action").started += (callback) =>
        {
            weaponController.DoAction();
        };
    }

    public void OnDamaged(GameObject attacker, Weapon causer, Vector3 hitPoint, WeaponData weaponData)
    {
        hpComponent.AddDamage(weaponData.Power);

        if (weaponData.HitParticle != null)
        {
            GameObject go = Instantiate(weaponData.HitParticle);
            {
                go.transform.position = hitPoint + weaponData.HitParticlePositionOffset;
                go.transform.localScale += weaponData.HitParticleScaleOffset;
            }
        }
    }
}
