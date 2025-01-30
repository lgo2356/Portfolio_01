using UnityEngine;

public class Skill_Staff_Cursor : MonoBehaviour
{
    [SerializeField]
    private GameObject marker;

    [SerializeField]
    private GameObject dot;

    private float distance = 100.0f;
    private LayerMask layerMask;
    private bool canMove;

    public Vector3 Position => transform.position;

    private void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("Default");
    }

    private void Update()
    {
        if (canMove == false)
            return;

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

        Vector3 up = Quaternion.Euler(0, 0, 0) * Vector3.up;

        float angle = Vector3.Angle(up, normal);

        if (angle > 30)
        {
            return;
        }

        position += normal * 0.05f;

        transform.position = position;
    }

    public void HoldCursor(bool isHold)
    {
        canMove = !isHold;
    }

    public void SetEnabled()
    {
        canMove = true;

        gameObject.SetActive(true);

        SetEnabledParticle(true);
    }

    public void SetDisabled()
    {
        SetEnabledParticle(false);
    }

    private void SetEnabledParticle(bool isEnabled)
    {
        #region 마커
        ParticleSystem markerParticle = marker.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule markerParticleMain = markerParticle.main;

        Gradient markerGradient = new Gradient();
        GradientAlphaKey[] markerGradientAlphaKeys;

        if (isEnabled)
        {
            markerParticleMain.ringBufferMode = ParticleSystemRingBufferMode.PauseUntilReplaced;
            markerGradientAlphaKeys = new GradientAlphaKey[]
            {
                new GradientAlphaKey(0.0f, 0.0f),
                new GradientAlphaKey(1.0f, 0.1f),
                new GradientAlphaKey(1.0f, 1.0f),
            };
        }
        else
        {
            markerParticleMain.ringBufferMode = ParticleSystemRingBufferMode.Disabled;
            markerGradientAlphaKeys = new GradientAlphaKey[]
            {
                new GradientAlphaKey(0.0f, 0.0f),
                new GradientAlphaKey(1.0f, 0.1f),
                new GradientAlphaKey(1.0f, 0.9f),
                new GradientAlphaKey(0.0f, 1.0f),
            };
        }
        
        markerGradient.SetKeys(markerGradient.colorKeys, markerGradientAlphaKeys);
        #endregion

        #region 닷
        ParticleSystem.ColorOverLifetimeModule markerColorOverLifetime = markerParticle.colorOverLifetime;
        markerColorOverLifetime.color = new ParticleSystem.MinMaxGradient(markerGradient);

        ParticleSystem dotParticle = dot.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule dotParticleMain = dotParticle.main;
        ParticleSystem.SizeOverLifetimeModule dotSizeOverLifetime = dotParticle.sizeOverLifetime;

        AnimationCurve dotAnimationCurve = new AnimationCurve();

        if (isEnabled)
        {
            dotParticleMain.ringBufferMode = ParticleSystemRingBufferMode.PauseUntilReplaced;

            Keyframe keyframe1 = new Keyframe(0.0f, 0.0f, 0.0f, 0.0f);
            Keyframe keyframe2 = new Keyframe(0.05f, 0.6f, 0.0f, 0.0f);
            Keyframe keyframe4 = new Keyframe(1.0f, 0.6f, 0.0f, 0.0f);

            dotAnimationCurve.AddKey(keyframe1);
            dotAnimationCurve.AddKey(keyframe2);
            dotAnimationCurve.AddKey(keyframe4);
        }
        else
        {
            dotParticleMain.ringBufferMode = ParticleSystemRingBufferMode.Disabled;

            Keyframe keyframe1 = new Keyframe(0.0f, 0.0f, 0.0f, 0.0f);
            Keyframe keyframe2 = new Keyframe(0.05f, 0.6f, 0.0f, 0.0f);
            Keyframe keyframe3 = new Keyframe(0.95f, 0.6f, 0.0f, 0.0f);
            Keyframe keyframe4 = new Keyframe(1.0f, 0.0f, 0.0f, 0.0f);

            dotAnimationCurve.AddKey(keyframe1);
            dotAnimationCurve.AddKey(keyframe2);
            dotAnimationCurve.AddKey(keyframe3);
            dotAnimationCurve.AddKey(keyframe4);
        }

        dotSizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1.0f, dotAnimationCurve);
        #endregion
    }
}
