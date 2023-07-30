using TMPro;
using UnityEngine;

public class ThrustTimer : MonoBehaviour
{
    public float ThrustInterval = 3;
    public float countdownSpeed = 1;

    [SerializeField] private TextMeshProUGUI VisualCounter;
    public LevelController LevelController;

    [ReadOnly]
    public float LastThrustTime = 0;

    public float lastTime;

    private PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        LastThrustTime = Time.time;
    }

    void Update()
    {
        CheckForThrust();
    }

    private void CheckForThrust()
    {
        if(!player.isOnGround) {
            LastThrustTime = Time.time;
            return;
        }

        float elapsedTime = Time.time - LastThrustTime;
        float currentCount = ThrustInterval - (int)(elapsedTime * countdownSpeed);
        
        if ( VisualCounter != null )
        {
            VisualCounter.text = currentCount.ToString();
        }

        if(lastTime != currentCount) {
            lastTime = currentCount;
            AudioController.PlaySound(Sounds.Countdown);
        }

        if ( currentCount <= 0 )
        {
            AudioController.PlaySound(Sounds.CountdownFinish);
            SignalForThrust();
            LastThrustTime = Time.time;
        }

    }

    private void SignalForThrust()
    {
        LevelController.InitiatePlayerThrust();
    }
}
