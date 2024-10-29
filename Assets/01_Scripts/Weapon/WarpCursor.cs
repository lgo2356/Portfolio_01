using UnityEngine;

public class WarpCursor : MonoBehaviour
{
    private float distance = 100.0f;
    private LayerMask layerMask;

    public Vector3 Position => transform.position;

    private void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("Default");
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 position;
        Vector3 normal;

        if (Physics.Raycast(ray, out RaycastHit raycastHit, distance, layerMask))
        {
            position = raycastHit.point;
            normal = raycastHit.normal;
        }
        else
        {
            return;
        }
        
        position += normal * 0.05f;

        transform.position = position;

        Vector3 up = Quaternion.Euler(-90, 0, 0) * Vector3.up;
        Quaternion q = Quaternion.FromToRotation(up, normal);

        transform.rotation = q;
    }
}
