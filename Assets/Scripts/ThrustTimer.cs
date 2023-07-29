using TMPro;
using UnityEngine;

public class ThrustTimer : MonoBehaviour
{
    public float ThrustInterval = 3;

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
        
        if ( VisualCounter != null )
        {
            VisualCounter.text = (ThrustInterval - (int)elapsedTime).ToString();
        }

        if(lastTime != ThrustInterval - (int)elapsedTime) {
            lastTime = ThrustInterval - (int)elapsedTime;
            AudioController.PlaySound(Sounds.Countdown);
        }

        if ( elapsedTime >= ThrustInterval )
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
