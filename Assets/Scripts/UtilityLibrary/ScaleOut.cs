using Assets.Scripts.Extensions;
using UnityEngine;
using TMPro;

public class ScaleOut : MonoBehaviour
{
    [Tooltip("The final scale of the item.")]
    public float EndScale = 0;
    public float StartScale = 0;
    public bool SetStartingScale = false;

    [Tooltip("A delay after activated.")]
    public float BeginAfter = 5;
    [Tooltip("The time the fade/transition portion will take.")]
    public float Duration = 4;
    public bool AutoStart = false;
    public bool DestroyWhenDone = false;
    public bool DisableWhenDone = true;
    private float StartTime { get; set; } = 0;
    public bool IsActive = false;

    float LerpFrom = 1;

    public void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (SetStartingScale)
            LerpFrom = StartScale;

        if (AutoStart)
            Activate();
    }
    public void Activate()
    {
        StartTime = Time.time;
        IsActive = true;

        if (SetStartingScale)
            LerpFrom = StartScale;
        else
            LerpFrom = transform.localScale.x;
    }


    void Update()
    {
        if (!IsActive)
            return;

        var endAt = StartTime + BeginAfter + Duration;

        if (Time.time - StartTime > BeginAfter)
        {
            float lerpTo = EndScale;
            float scale = Mathf.Lerp(LerpFrom, lerpTo, (Time.time - (StartTime + BeginAfter)) / Duration);

            transform.localScale = new Vector3(scale, scale, 0);

            if (Time.time > endAt)
            {
                IsActive = false;
                if (DestroyWhenDone)
                {
                    Destroy(gameObject);
                }
                else if (DisableWhenDone)
                {
                    gameObject.SafeSetActive(false);
                }
            }
        }
    }
}

