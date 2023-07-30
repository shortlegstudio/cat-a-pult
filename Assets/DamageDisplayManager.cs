using UnityEngine;

public class DamageDisplayManager : MonoBehaviour
{
    public static DamageDisplayManager Current => GameController.Current.DamageDisplayManager;

    public DamageDisplay DamageDisplayPrototype;

    public DamageDisplay[] DamageVisualInstances = new DamageDisplay[60];

    public bool IsInitialized = false;
    
    public void Initialize()
    {
        if (IsInitialized)
            return;

        for (int i = 0; i < DamageVisualInstances.Length; i++)
        {
            var dmgInstance = GameObject.Instantiate<DamageDisplay>(DamageDisplayPrototype, transform);
            dmgInstance.gameObject.SetActive(false);
            DamageVisualInstances[i] = dmgInstance;
        }

        IsInitialized = true;
    }

    public void Reset()
    {
        for (int i = 0; i < DamageVisualInstances.Length; i++)
        {
            if ( DamageVisualInstances[i] != null )
            {
                DamageVisualInstances[i].gameObject.SetActive(false);
            }
        }
    }

    public void DisplayDamageAt(float amt, Vector3 position)
    {
        Initialize();

        DamageDisplay instanceToUse = null;

        for (int i=0; i<DamageVisualInstances.Length; i++)
        {
            if (DamageVisualInstances[i] != null && DamageVisualInstances[i].gameObject.activeSelf == false)
            {
                instanceToUse = DamageVisualInstances[i];
                break;
            }
        }

        if ( instanceToUse != null)
        {
            instanceToUse.gameObject.SetActive(true);
            instanceToUse.transform.position = position;
            instanceToUse.Activate(amt);
        }
    }

}
