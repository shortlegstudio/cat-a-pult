using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Vector3 InputVector = Vector3.zero;
    public float RotationSpeed = 25f;
    public float JumpForce = 250f;
    public float XVelocityMin = 10f;

    Rigidbody2D OurRb;

    internal void AddPowerUp(PowerUpKind kind, int quantity)
    {
    }

    public SpriteRenderer OurSprite;

    [ReadOnly]
    public float CurrentSpeed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {
        OurRb = GetComponent<Rigidbody2D>();

        var lm = 1 << LayerMask.NameToLayer(GameConstants.Goop);
        var clm = GameConstants.LayerMaskGoop;

        ColliderFilter = new ContactFilter2D()
        {
            layerMask = lm,
            useLayerMask = true,
            useTriggers = true
        };
    }
    void Update()
    {
        HandleMovement();
    }


    void OnReload(InputValue value)
    {
        //GameController.TheGameController.ReloadCurrentScene();
        var go = GameObject.FindGameObjectWithTag(GameConstants.TagLevelController);
        if (go.TryGetComponent<LevelController>(out LevelController controller))
        {
            controller.ShowPauseUi();
        }
    }


    ContactFilter2D ColliderFilter;
    Collider2D[] Hits = new Collider2D[20];

    private void OnDrawGizmos()
    {
    }

    void HandleMovement()
    {
        if (Mathf.Abs(OurRb.velocity.x) < XVelocityMin)
            OurRb.velocity = new Vector2(0, OurRb.velocity.y);

        var pointerPos = Pointer.current.position.ReadValue();
        Vector3 wposition = Camera.main.ScreenToWorldPoint(pointerPos);
        wposition.z = 0;

        var dir = wposition - transform.position;
        var rot = Quaternion.LookRotation(Vector3.forward, dir);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.RotateTowards(transform.up, dir, RotationSpeed * Time.deltaTime, 1));


        if (CurrentSpeed != 0)
        {
            OurRb.AddForce(transform.up * CurrentSpeed, ForceMode2D.Impulse);
            CurrentSpeed = 0;
        }
    }

    void OnJump(InputValue value)
    {
    }

    void OnMove(InputValue value)
    {
        if (value != null)
        {
            CurrentSpeed = JumpForce;
        }
        else
        {
            InputVector = Vector3.zero;
            CurrentSpeed = 0;
        }
    }

    void OnFire(InputValue value)
    {
    }
}
