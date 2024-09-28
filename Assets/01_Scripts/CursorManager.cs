using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField]
    private bool isLocked = true;

    private void Start()
    {
        if (isLocked)
        {
            Lock();
        }
        else
        {
            Unlock();
        }
    }

    private void Lock()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Unlock()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
