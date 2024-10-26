using UnityEngine;

public class Warp : RangeWeapon
{
    [SerializeField]
    private string handPositionName = "Hand_Warp";
    
    [SerializeField]
    private float cursorDistance = 100.0f;

    [SerializeField]
    private GameObject cursorPrefab;

    [SerializeField]
    private GameObject fxPrefab;

    private Transform handPositionTransform;
    private GameObject cursorObject;
    private GameObject fxObject;

    protected override void Reset()
    {
        base.Reset();

        type = WeaponType.Warp;
    }

    protected override void Awake()
    {
        base.Awake();
        
        handPositionTransform = rootObject.transform.FindChildByName(handPositionName);
        {
            Debug.Assert(handPositionTransform != null, "Warp handPositionTransform is null.");
        
            transform.SetParent(handPositionTransform, false);
        }
    }

    protected override void Start()
    {
        base.Start();
        
        gameObject.SetActive(false);

        if (cursorPrefab != null)
        {
            cursorObject = Instantiate(cursorPrefab);
            cursorObject.SetActive(false);
        }

        if (fxPrefab != null)
        {
            fxObject = Instantiate(fxPrefab, rootObject.transform);
            fxObject.SetActive(false);
        }
    }

    public override void Equip()
    {
        base.Equip();
        
        gameObject.SetActive(true);
        
        cursorObject.SetActive(true);
    }

    public override void Unequip()
    {
        base.Unequip();
        
        gameObject.SetActive(false);
        
        cursorObject.SetActive(false);
    }

    public override void DoAction()
    {
        base.DoAction();
        
        cursorObject.SetActive(false);
        
        rootObject.transform.LookAt(cursorObject.transform.position, Vector3.up);
    }

    public override void EndAction()
    {
        base.EndAction();
        
        cursorObject.SetActive(true);
        fxObject.SetActive(false);
    }

    public override void ShowFX()
    {
        base.ShowFX();

        if (fxObject != null)
        {
            fxObject.SetActive(true);
        }

        WarpCursor warpCursor = cursorObject.GetComponent<WarpCursor>();
        rootObject.transform.position = warpCursor.Position;
    }
}
