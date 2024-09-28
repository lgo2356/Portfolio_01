using UnityEngine;
using UnityEngine.InputSystem;
using StateType = StateComponent.StateType;

public class PlayerMoveComponent : MonoBehaviour
{
    [SerializeField, Header("이동")]
    private float walkSpeed = 2f;

    [SerializeField]
    private float runSpeed = 4f;

    [SerializeField]
    private float sensitivity = 50f;

    [SerializeField]
    private float deadZone = 0.001f;

    [SerializeField, Header("마우스 회전")]
    private Transform cameraTargetTransform;

    [SerializeField]
    private float mouseSpeed = 0.3f;

    [SerializeField]
    private Vector2 pitchLimit = new(10, 340);

    private void Reset()
    {
        GameObject go = GameObject.Find("Player");
        Debug.Assert(go != null);

        cameraTargetTransform = go.transform.FindChildByName("CameraTarget");
        Debug.Assert(cameraTargetTransform != null);
    }

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
    #endregion

    private void Update()
    {
        #region 마우스 회전
        {
            mouseRotation *= Quaternion.AngleAxis(playerInputLook.x * mouseSpeed, Vector3.up);
            mouseRotation *= Quaternion.AngleAxis(-playerInputLook.y * mouseSpeed, Vector3.right);
            cameraTargetTransform.rotation = mouseRotation;

            mouseRotation = Quaternion.Lerp(cameraTargetTransform.rotation, mouseRotation, mouseSpeed * Time.deltaTime);

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
        }
        #endregion

        #region 캐릭터 이동
        {
            currentPlayerInputMove = Vector2.SmoothDamp(currentPlayerInputMove, playerInputMove, ref currentVelocity, 1f / sensitivity);
            Vector3 moveDirection = Vector3.zero;
            float moveSpeed = isRunning ? runSpeed : walkSpeed;

            if (currentPlayerInputMove.magnitude > deadZone)
            {
                moveDirection = (Vector3.right * currentPlayerInputMove.x) + (Vector3.forward * currentPlayerInputMove.y);
                moveDirection = moveDirection.normalized * moveSpeed;
            }

            transform.Translate(moveDirection * Time.deltaTime);
        }
        #endregion
    }

    private void OnGUI()
    {
        GUI.color = Color.red;
        GUILayout.Label(playerInputLook.ToString());
    }
}
