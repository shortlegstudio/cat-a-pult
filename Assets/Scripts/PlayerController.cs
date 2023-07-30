using Assets.Scripts.Extensions;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlayerController : MonoBehaviour
{
    public Vector3 InputVector = Vector3.zero;
    public float RotationSpeed = 25f;
    public float BaseJumpForce = 20000f;
    public float AdditionalJumpForcePerSecond = 10000f;
    public float XVelocityMin = 10f;
    public float YVelocityMin = 2f;
    public float XVelocityMax = 100f;
    public float YVelocityMax = 2000f;

    public GameObject NavRing;
    public GameObject Arrow;
    public GameObject JumpEffect;

    public Animator AnimatorController;

    public GameObject CatSplosionSpatProto;
    //public PlayerInputScheme InputActions;
    //public InputActionAsset InputAsset;

    public BoxCollider2D groundCollider;
    public float beatsOnGround = 0;
    public float beatsToLand = 0.1f;

    public float airMovementForce = 10f;
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

    InputAction FireInputAction;

    void Init()
    {
        //InputActions = (PlayerInputScheme)InputAsset;
        //InputActions.Player.Fire.started += FireAlt;
        //InputActions.Player.Fire.canceled += FireAltB;
        //FireInputAction = InputActions.FindAction("Fire");

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
        if (GameDataHolder.Current.GameData.GameInProgress)
        {
            HandleDeath();
            HandleThrustBuildUp();
            HandleProgressTracking();
        }
    }

    void FixedUpdate()
    {
        if (GameDataHolder.Current.GameData.GameInProgress)
        {
            HandleMovement();
        }
    }

    private void HandleDeath()
    {
        if (GameDataHolder.Current.GameData.Health <= 0)
        {
            if (GameDataHolder.Current.GameData.IsDead)
                return;

            if (GameDataHolder.Current.GameData.InDeathThrows)
                return;

            OurSprite.enabled = false;
            NavRing.SafeSetActive(false);
            OurRb.gravityScale = 0;
            GameDataHolder.Current.GameData.InDeathThrows = true;
            Instantiate(CatSplosionSpatProto, transform.position, transform.rotation);
        }
    }

    private void HandleProgressTracking()
    {
        GameDataHolder.Current.GameData.MaxHeightReached = Mathf.Max(GameDataHolder.Current.GameData.MaxHeightReached, transform.position.y);
    }

    private void HandleThrustBuildUp()
    {
        if (BuildThrust)
            TimeThrustBuildUpEnded = Time.time;

        Arrow.transform.localPosition = new Vector3(0, 12 + 5 * GetCurrentThrustMultiplier(), 0);
        Arrow.transform.localScale = new Vector3(0.6f, 0.8f * (1 + GetCurrentThrustMultiplier()), 1);
    }

    float TimeThrustBuildUpStarted = 0;
    float TimeThrustBuildUpEnded = 0;
    bool BuildThrust = false;
    public bool isOnGround = true;
    private Vector2 airForce = new Vector2(0, 0);

    public void Fire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            BuildThrust = true;
            ResetThrustBuildUp();
        }

        if (context.canceled)
        {
            BuildThrust = false;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!isOnGround)
        {
            if(context.started)
            {
                var dir = context.ReadValue<Vector2>();
                airForce = new Vector2(dir.x, 0);
            }
        } 
        
        if (context.canceled)
            airForce = new Vector2(0, 0);
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

    void OnTriggerEnter2D(Collider2D col) 
    {
        if(col.tag == "Platform")
        {
            //Try to stick the landing
            OurRb.velocity = new Vector2(0, 0);
        }
    }

    void OnTriggerStay2D(Collider2D col) 
    {
        if(col.tag == "Platform")
        {
            beatsOnGround+= Time.deltaTime;
            if(beatsOnGround >= beatsToLand) 
            {
                isOnGround = true;
            }
        }
    }

    void HandleMovement()
    {

        if (isOnGround)
        {
            AnimatorController.SetBool("IsJumping", false);
            AnimatorController.SetBool("IsFlying", false);
            AnimatorController.SetBool("IsLanding", false);
        }

        if(!isOnGround) 
        {
            if (OurRb.velocity.x < 0)
                OurSprite.transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            else if (OurRb.velocity.x > 0)
                OurSprite.transform.rotation = Quaternion.identity;

            OurRb.AddForce(airForce * airMovementForce, ForceMode2D.Impulse);

            OurRb.velocity = new Vector2(
                Mathf.Clamp(OurRb.velocity.x, -XVelocityMax, XVelocityMax),
                Mathf.Clamp(OurRb.velocity.y, -YVelocityMax, YVelocityMax)
            );
        }

        var pointerPos = Pointer.current.position.ReadValue();
        Vector3 wposition = Camera.main.ScreenToWorldPoint(pointerPos);
        wposition.z = 0;

        var dir = wposition - NavRing.transform.position;
        var rot = Quaternion.LookRotation(Vector3.forward, dir);
        NavRing.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.RotateTowards(NavRing.transform.up, dir, RotationSpeed * Time.deltaTime, 1));

        if (CurrentSpeed != 0)
        {
            OurRb.AddForce(NavRing.transform.up * CurrentSpeed, ForceMode2D.Impulse);
            //AnimatorController.SetBool("IsIdle", false);
            AnimatorController.SetBool("IsJumping", true);
            CurrentSpeed = 0;
        }


    }

    void ResetThrustBuildUp()
    {
        if (BuildThrust)
        {
            TimeThrustBuildUpStarted = Time.time;
            TimeThrustBuildUpEnded = Time.time;
        }
        else
        {
            TimeThrustBuildUpStarted = 0;
            TimeThrustBuildUpEnded = 0;
        }
    }

    private float GetCurrentThrustMultiplier()
    {
        return TimeThrustBuildUpEnded - TimeThrustBuildUpStarted;
    }

    internal void InitiateThrust()
    {
        CurrentSpeed = BaseJumpForce + (AdditionalJumpForcePerSecond * GetCurrentThrustMultiplier());
        isOnGround = false;
        beatsOnGround = 0;
        Instantiate(JumpEffect, transform.position, transform.rotation);
        ResetThrustBuildUp();
    }
}
