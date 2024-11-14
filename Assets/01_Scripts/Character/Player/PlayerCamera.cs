using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private float mouseSpeed = 0.3f;

    [SerializeField]
    private Vector2 pitchLimit = new(60, 340);

    private Vector2 playerInputLook;
    private Quaternion newRotation;

    private void Reset()
    {
        playerTransform = GameObject.Find("Player").transform;
        Debug.Assert(playerTransform != null);
    }

    private void Awake()
    {
        PlayerInput playerInput = playerTransform.GetComponent<PlayerInput>();
        InputActionMap actionMap = playerInput.actions.FindActionMap("PlayerActions");

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

    private void Update()
    {
        if (GameManager.isPlayerInput == false)
            return;
        
        // 카메라 이동
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + 1.3f, playerTransform.position.z);
        
        // 카메라 회전
        {
            newRotation = transform.rotation;
            
            newRotation *= Quaternion.Euler(-playerInputLook.y * mouseSpeed, playerInputLook.x * mouseSpeed, 0);
            
            // Pitch 회전각 제한
            {
                Vector3 angle = newRotation.eulerAngles;
                angle.z = 0f;
        
                if (angle.x < 180f && angle.x > pitchLimit.x)
                {
                    angle.x = pitchLimit.x;
                }
                else if (angle.x > 180f && angle.x < pitchLimit.y)
                {
                    angle.x = pitchLimit.y;
                }
            
                newRotation.eulerAngles = new Vector3(angle.x, angle.y, 0.0f);
            }
            
            transform.rotation = newRotation;
        }
    }
}
