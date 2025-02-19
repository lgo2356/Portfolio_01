using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PerceptionComponent : MonoBehaviour
{
    [SerializeField]
    private float perceptionDistance = 5.0f;

    [SerializeField]
    private float perceptionAngle = 45.0f;

    [SerializeField]
    private float lostTime = 2.0f;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private bool isDebugging;

    private void Reset()
    {
        layerMask = 1 << LayerMask.NameToLayer("Player");
        isDebugging = true;
    }

    private Collider[] colliderBuffer;
    private Dictionary<GameObject, float> perceivedTable;
    private bool bPerceivedTableClearFlag;

    private Coroutine checkPerceivedTimeCoroutine;

    public event Action<GameObject> OnFoundAction;
    public event Action<GameObject> OnLostAction;

    private void Awake()
    {
        colliderBuffer = new Collider[10];
        perceivedTable = new Dictionary<GameObject, float>();
    }

    private void Start()
    {
        checkPerceivedTimeCoroutine = StartCoroutine(Coroutine_CheckPerceivedTime());
    }

    private void Update()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, perceptionDistance, colliderBuffer, layerMask);
        List<Collider> candidates = new();

        for (int i = 0; i < count; i++)
        {
            Vector3 direction = colliderBuffer[i].transform.position - transform.position;
            float signedAngle = Vector3.SignedAngle(transform.forward, direction.normalized, Vector3.up);

            if (Mathf.Abs(signedAngle) < perceptionAngle / 2f)
            {
                candidates.Add(colliderBuffer[i]);
            }
        }

        foreach (Collider candidate in candidates)
        {
            if (perceivedTable.ContainsKey(candidate.gameObject))
            {
                perceivedTable[candidate.gameObject] = Time.realtimeSinceStartup;
            }
            else
            {
                perceivedTable.Add(candidate.gameObject, Time.realtimeSinceStartup);

                OnFoundAction?.Invoke(candidate.gameObject);
            }
        }
    }

    public void StopPerception()
    {
        StopCoroutine(checkPerceivedTimeCoroutine);
        ClearPerceivedObject();

        enabled = false;
    }

    public void ClearPerceivedObject()
    {
        bPerceivedTableClearFlag = true;
    }

    private IEnumerator Coroutine_CheckPerceivedTime()
    {
        List<GameObject> removeReservationList = new();

        while (true)
        {
            // Loop문에서 Collection을 조회하는 중에 Collection의 변화가 생기면 InvalidOperationException이 발생한다.
            // 열거자의 일관성을 지키기 위해서 예외를 던진다.
            // 따라서 Collection을 클리어하는 플래그를 두고 Loop문에 진입하기 전에 클리어한다.
            if (bPerceivedTableClearFlag)
            {
                perceivedTable.Clear();
                bPerceivedTableClearFlag = false;
            }
            
            foreach (var perceivedData in perceivedTable)
            {
                if ((Time.realtimeSinceStartup - perceivedData.Value) >= lostTime)
                {
                    OnLostAction?.Invoke(perceivedData.Key);

                    removeReservationList.Add(perceivedData.Key);
                }
            }

            removeReservationList.RemoveAll((go) => perceivedTable.Remove(go));

            yield return null;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (isDebugging == false)
            return;

        #region 시야 범위
        Handles.color = Color.white;
        Handles.DrawWireArc(transform.position, Vector3.up, transform.forward, 360, perceptionDistance);
        #endregion

        #region 시야 각도
        Vector3 direction = Quaternion.AngleAxis(+perceptionAngle / 2f, Vector3.up) * transform.forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + direction.normalized * perceptionDistance);

        direction = Quaternion.AngleAxis(-perceptionAngle / 2f, Vector3.up) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + direction.normalized * perceptionDistance);
        #endregion

        #region 감지 표시
        if (Application.isPlaying == false)
            return;

        foreach (var perceived in perceivedTable)
        {
            Vector3 position = transform.position;
            position.y += 1.0f;

            Vector3 perceivedPosition = perceived.Key.transform.position;
            perceivedPosition.y += 1.0f;
            
            Vector3 perceivedDirection = perceivedPosition - position;
            
            Gizmos.DrawLine(position, position + perceivedDirection.normalized * perceivedDirection.magnitude);
            Gizmos.DrawWireSphere(perceivedPosition, 0.25f);
        }
        #endregion
    }
#endif
}
