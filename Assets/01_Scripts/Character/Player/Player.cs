using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
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

    private Animator animator;

    private void Awake()
    {
        Awake_GetComponent();
        Awake_BindInput();
    }

    private void Awake_GetComponent()
    {
        animator = GetComponent<Animator>();
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
    }

    private void OnAnimatorMove()
    {
        transform.position += animator.deltaPosition;
    }
}
