using System.Collections;
using UnityEngine;

public class GuidedProjectile : Projectile
{
    private GameObject target;
    private float speed;

    private Coroutine guideCoroutine;

    public GameObject Target
    {
        set => target = value;
    }

    private void Update()
    {
        float dot = Vector3.Dot(target.transform.position - transform.position, transform.forward);

        if (dot < 0.0f)
        {
            StopCoroutine(guideCoroutine);
        }
        
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private IEnumerator Coroutine_Guide()
    {
        while (true)
        {
            Vector3 position = target.transform.position;
            position.y += 1f;
            
            Vector3 direction = position - transform.position;
            
            Quaternion q = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = q;

            yield return new WaitForSeconds(0.5f);
        }
    }
    
    // 위치는 계속 이동하고 로테이션만 쿨타임 주기
    // AddForce 제거
    // 트레일 발사체로 변경하기

    public override void Shoot(GameObject target, float speed)
    {
        base.Shoot(target, speed);
        
        this.target = target;
        this.speed = speed;
        
        guideCoroutine = StartCoroutine(Coroutine_Guide());
    }
}
