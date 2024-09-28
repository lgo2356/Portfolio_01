using UnityEngine;
using UnityEngine.InputSystem;
using StateType = StateComponent.StateType;

public class PlayerMoveComponent : MonoBehaviour
{
    #region 설정 값
    [SerializeField, Header("이동")]
    private Transform bodyTransform;

    [SerializeField]
    private float walkSpeed = 2.2f;

    [SerializeField]
    private float runSpeed = 5.5f;

    [SerializeField]
    private float sensitivity = 50f;

    [SerializeField]
    private float deadZone = 0.001f;

    [SerializeField, Header("마우스 회전")]
    private Transform cameraTargetTransform;

    [SerializeField]
    private float mouseSpeed = 0.3f;

    [SerializeField]
    private Vector2 pitchLimit = new(60, 340);

    private void Reset()
    {
        GameObject go = GameObject.Find("Player");
        Debug.Assert(go != null);

        bodyTransform = go.transform.FindChildByName("Body");
        Debug.Assert(bodyTransform != null);

        cameraTargetTransform = go.transform.FindChildByName("CameraTarget");
        Debug.Assert(cameraTargetTransform != null);
    }
    #endregion

    #region 컴포넌트
    private Animator animator;
    private StateComponent stateComponent;
    #endregion

    #region 전역 변수
    private bool isRunning = false;
    private Vector2 playerInputMove;
    private Vector2 playerInputLook;
    #endregion

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

        #region Look
        {
            InputAction action = actionMap.FindAction("Look");

            action.performed += (context) =>
            {
                playerInputLook = context.ReadValue<Vector2>();
            };

            action.canceled += (context) => playerInputLook = Vector2.zero;
            action.Enable();
        }
        #endregion
    }

    private void Start()
    {

    }

    #region Update 변수
    private Vector2 currentPlayerInputMove;
    private Vector2 currentVelocity = Vector2.zero;
    private Quaternion mouseRotation;
    private float currentMoveSpeed;
    #endregion

    private void Update()
    {
        #region 마우스 회전
        {
            mouseRotation *= Quaternion.Euler(-playerInputLook.y * mouseSpeed, playerInputLook.x * mouseSpeed, 0);
            cameraTargetTransform.rotation = mouseRotation;

            Vector3 angle = cameraTargetTransform.localEulerAngles;
            angle.z = 0f;

            if (angle.x < 180f && angle.x > pitchLimit.x)
            {
                angle.x = pitchLimit.x;
            }
            else if (angle.x > 180f && angle.x < pitchLimit.y)
            {
                angle.x = pitchLimit.y;
            }

            cameraTargetTransform.localEulerAngles = new(angle.x, angle.y, 0f);
            mouseRotation = Quaternion.Lerp(cameraTargetTransform.rotation, mouseRotation, mouseSpeed * Time.deltaTime);
        }
        #endregion

        #region 캐릭터 이동
        {
            currentPlayerInputMove = Vector2.SmoothDamp(currentPlayerInputMove, playerInputMove, ref currentVelocity, 1f / sensitivity);            
            float moveSpeed = isRunning ? runSpeed : walkSpeed;

            if (currentPlayerInputMove.magnitude > deadZone)
            {
                Quaternion cameraRotation = Quaternion.Euler(0, cameraTargetTransform.eulerAngles.y, 0);                
                Vector3 inputDirection = new(currentPlayerInputMove.x, 0, currentPlayerInputMove.y);
                Quaternion inputRotation = Quaternion.LookRotation(inputDirection);
                Quaternion lookRotation =  cameraRotation * inputRotation;

                bodyTransform.rotation = Quaternion.Slerp(bodyTransform.rotation, lookRotation, 0.2f);

                transform.Translate(bodyTransform.forward * moveSpeed * Time.deltaTime);
            }

            currentMoveSpeed = Mathf.Lerp(currentMoveSpeed, playerInputMove.magnitude * moveSpeed, 1f / sensitivity * 10);

            animator.SetFloat("SpeedX", currentPlayerInputMove.x * moveSpeed);
            animator.SetFloat("SpeedY", currentPlayerInputMove.y * moveSpeed);
            animator.SetFloat("SpeedZ", currentMoveSpeed);
        }
        #endregion
    }

    private void OnGUI()
    {
        GUI.color = Color.red;
        GUILayout.Label(cameraTargetTransform.forward.ToString());

        GUI.color = Color.blue;
        GUILayout.Label(bodyTransform.forward.ToString());
    }
}
