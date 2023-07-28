using TMPro;
using UnityEngine;

public class ThrustTimer : MonoBehaviour
{
    public float ThrustInterval = 3;

    [SerializeField] private TextMeshPro VisualCounter;
    public LevelController LevelController;

    [ReadOnly]
    public float LastThrustTime = 0;

    void Start()
    {
        
    }

    void Update()
    {
        CheckForThrust();
    }

    private void CheckForThrust()
    {
        float elapsedTime = Time.time - LastThrustTime;
        if ( VisualCounter != null )
        {
            VisualCounter.text = (ThrustInterval - (int)elapsedTime).ToString();
        }

        if ( elapsedTime >= ThrustInterval )
        {
            SignalForThrust();
            LastThrustTime = Time.time;
        }
    }

    private void SignalForThrust()
    {
        LevelController.InitiatePlayerThrust();
    }
}
