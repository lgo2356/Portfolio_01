using UnityEngine;
using UnityEngine.InputSystem;
using StateType = StateComponent.StateType;

public class PlayerMoveComponent : MonoBehaviour
{
    [SerializeField, Header("마우스")]
    private Transform cameraTargetTransform;

    [SerializeField]
    private float walkSpeed = 2.2f;

    [SerializeField]
    private float runSpeed = 5.5f;

    [SerializeField]
    private float sensitivity = 50f;

    [SerializeField]
    private float deadZone = 0.001f;

    private void Reset()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        Debug.Assert(go != null);

        cameraTargetTransform = go.transform.FindChildByName("PlayerCameraTarget");
        Debug.Assert(cameraTargetTransform != null);
    }

    #region Component
    private Animator animator;
    private StateComponent stateComponent;
    #endregion
    
    private bool isRunning = false;
    private bool canMove = true;
    private Vector2 playerInputMove;

    public void Hold() => canMove = false;
    public void Release() => canMove = true;

    private void Awake()
    {
        Awake_GetComponents();
        Awake_BindInput();
    }

    private void Awake_GetComponents()
    {
        animator = GetComponent<Animator>();
        stateComponent = GetComponent<StateComponent>();
    }

    private void Awake_BindInput()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        InputActionMap actionMap = playerInput.actions.FindActionMap("PlayerActions");

        #region Move
        {
            InputAction action = actionMap.FindAction("Move");
            
            action.performed += (context) =>
            {
                playerInputMove = context.ReadValue<Vector2>();
            };

            action.canceled += (context) => playerInputMove = Vector2.zero;
            action.Enable();
        }
        #endregion

        #region Run
        {
            InputAction action = actionMap.FindAction("Run");

            action.started += (context) => isRunning = true;
            action.canceled += (context) => isRunning = false;
            action.Enable();
        }
        #endregion

        #region Evade
        {
            InputAction action = actionMap.FindAction("Evade");

            action.started += (context) =>
            {
                if (stateComponent.IsIdleState == false)
                    return;

                stateComponent.SetEvadeState();
            };
            action.Enable();
        }
        #endregion
    }

    private void Start()
    {
        Start_BindEvent();
    }

    private void Start_BindEvent()
    {
        stateComponent.OnStateTypeChanged += (prevState, newState) =>
        {
            if (newState == StateType.Evade)
            {
                ExecuteEvade();
            }
        };
    }

    #region Update 변수
    private Vector2 currentPlayerInputMove;
    private Vector2 currentVelocity = Vector2.zero;
    #endregion

    private void Update()
    {
        if (canMove == false)
            return;

        currentPlayerInputMove = Vector2.SmoothDamp(currentPlayerInputMove, playerInputMove, ref currentVelocity, 1f / sensitivity);
        
        float moveSpeed = isRunning ? runSpeed : walkSpeed;

        if (currentPlayerInputMove.magnitude > deadZone)
        {
            Quaternion cameraRotation = Quaternion.Euler(0, cameraTargetTransform.eulerAngles.y, 0);
            Vector3 inputDirection = new(currentPlayerInputMove.x, 0, currentPlayerInputMove.y);
            Quaternion inputRotation = Quaternion.LookRotation(inputDirection);
            Quaternion lookRotation =  cameraRotation * inputRotation;

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 0.2f);
        }

        float newSpeed = currentPlayerInputMove.magnitude * moveSpeed;
        transform.position += transform.forward * newSpeed * Time.deltaTime;

        animator.SetFloat("SpeedZ", newSpeed);
    }

    private void ExecuteEvade()
    {
        canMove = false;
        animator.SetTrigger("Evade");
    }

    private void Anim_EndEvade()
    {
        stateComponent.SetIdleState();
        canMove = true;
    }
}
