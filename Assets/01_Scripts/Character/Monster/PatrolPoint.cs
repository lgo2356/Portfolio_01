using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
    [SerializeField]
    private bool isLoop;

    [SerializeField]
    private bool isReverse;

    [SerializeField]
    private int nextIndex;

    [SerializeField]
    private float drawHeightOffset = 0.1f;

    [SerializeField]
    private Color debuggingSphereColor = Color.green;

    [SerializeField]
    private Color debuggingLineColor = Color.magenta;

    [Header("디버깅")]
    [SerializeField]
    private bool isDebuggingMode;

    private void Reset()
    {
        isLoop = false;
        isReverse = false;
        nextIndex = 0;
        isDebuggingMode = true;
    }

    public Vector3 GetNextPatrolPosition()
    {
        Debug.Assert(nextIndex >= 0 && nextIndex < transform.childCount);

        return transform.GetChild(nextIndex).position;
    }

    public void UpdateNextIndex()
    {
        int count = transform.childCount;

        if (isReverse)
        {
            if (nextIndex > 0)
            {
                nextIndex--;

                return;
            }

            if (isLoop)
            {
                nextIndex = count - 1;

                return;
            }

            isReverse = false;
            nextIndex = 1;

            return;
        }

        if (nextIndex < count - 1)
        {
            nextIndex++;

            return;
        }

        if (isLoop)
        {
            nextIndex = 0;

            return;
        }

        isReverse = true;
        nextIndex = count - 2;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (isDebuggingMode == false)
            return;

        int waypointCount = transform.childCount;

        if (waypointCount == 0)
        {
            return;
        }

        Debugging_DrawWaypointSphere(0, Color.blue, 0.25f);
        Debugging_DrawWaypointSphere(waypointCount - 1, Color.red, 0.25f);

        for (int i = 0; i < waypointCount; i++)
        {
            Debugging_DrawWaypointSphere(i, debuggingSphereColor, 0.15f);

            if (i < waypointCount - 1)
            {
                Debugging_DrawPathLine(i, i + 1);
            }
        }

        if (isLoop)
        {
            Debugging_DrawPathLine(waypointCount - 1, 0);
        }
    }

    private void Debugging_DrawWaypointSphere(int index, Color color, float size)
    {
        Vector3 waypointPosition = transform.GetChild(index).position + new Vector3(0, drawHeightOffset, 0);

        Gizmos.color = color;
        Gizmos.DrawSphere(waypointPosition, size);
    }

    private void Debugging_DrawPathLine(int startIndex, int endIndex)
    {
        Transform startTransform = transform.GetChild(startIndex);
        Transform endTransform = transform.GetChild(endIndex);
        Vector3 startPosition = startTransform.position + new Vector3(0, drawHeightOffset, 0);
        Vector3 endPosition = endTransform.position + new Vector3(0, drawHeightOffset, 0);

        Gizmos.color = debuggingLineColor;
        Gizmos.DrawLine(startPosition, endPosition);
    }
#endif
}
