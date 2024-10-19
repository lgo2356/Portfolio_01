using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    [SerializeField]
    private Transform bodyTransform;

    private WeaponComponent weaponComponent;

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
        weaponComponent = GetComponent<WeaponComponent>();
    }

    private void Awake_BindInput()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        InputActionMap actionMap = playerInput.actions.FindActionMap("PlayerActions");

        actionMap.FindAction("Equip_Sword").started += (callback) =>
        {
            weaponComponent.SetSword();
        };

        actionMap.FindAction("Equip_GreatSword").started += (callback) =>
        {
            weaponComponent.SetGreatSword();
        };

        actionMap.FindAction("Equip_Katana").started += (callback) =>
        {
            weaponComponent.SetKatana();
        };

        actionMap.FindAction("Action").started += (callback) =>
        {
            weaponComponent.DoAction();
        };
    }
}
