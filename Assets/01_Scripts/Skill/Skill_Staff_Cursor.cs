using Unity.VisualScripting;
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

    private void Awake()
    {
        
    }

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

        position += normal * 0.05f;

        transform.position = position;

        Vector3 up = Quaternion.Euler(0, 0, 0) * Vector3.up;
        Quaternion q = Quaternion.FromToRotation(up, normal);

        transform.rotation = q;
    }

    public void HoldCursor(bool isHold)
    {
        canMove = !isHold;
    }

    public void SetEnabled()
    {
        canMove = true;

        gameObject.SetActive(true);
    }

    public void SetDisabled()
    {
        ParticleSystem markerParticle = marker.GetComponent<ParticleSystem>();
        ParticleSystem dotParticle = dot.GetComponent<ParticleSystem>();

        ParticleSystem.MainModule markerParticleMain = markerParticle.main;
        markerParticleMain.ringBufferMode = ParticleSystemRingBufferMode.Disabled;

        Gradient markerGradient = new Gradient();
        markerGradient.SetKeys(markerGradient.colorKeys,
            new GradientAlphaKey[]
            {
                new GradientAlphaKey(0.0f, 0.0f),
                new GradientAlphaKey(1.0f, 0.1f),
                new GradientAlphaKey(1.0f, 0.9f),
                new GradientAlphaKey(0.0f, 1.0f),
            }
        );

        ParticleSystem.ColorOverLifetimeModule markerColorOverLifetime = markerParticle.colorOverLifetime;
        markerColorOverLifetime.color = new ParticleSystem.MinMaxGradient(markerGradient);

        ParticleSystem.MainModule dotParticleMain = dotParticle.main;
        ParticleSystem.SizeOverLifetimeModule dotSizeOverLifetime = dotParticle.sizeOverLifetime;

        dotParticleMain.ringBufferMode = ParticleSystemRingBufferMode.Disabled;

        AnimationCurve dotAnimationCurve = new AnimationCurve();
        Keyframe keyframe1 = new Keyframe(0.0f, 0.0f, 0.0f, 0.0f);
        Keyframe keyframe2 = new Keyframe(0.05f, 0.6f, 0.0f, 0.0f);
        Keyframe keyframe3 = new Keyframe(0.95f, 0.6f, 0.0f, 0.0f);
        Keyframe keyframe4 = new Keyframe(1.0f, 0.0f, 0.0f, 0.0f);

        dotAnimationCurve.AddKey(keyframe1);
        dotAnimationCurve.AddKey(keyframe2);
        dotAnimationCurve.AddKey(keyframe3);
        dotAnimationCurve.AddKey(keyframe4);

        dotSizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1.0f, dotAnimationCurve);
    }
}
