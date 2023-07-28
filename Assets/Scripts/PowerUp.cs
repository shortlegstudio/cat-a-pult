using Assets.Scripts.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpKind
{
    None
}

public class PowerUp : MonoBehaviour
{
    public PowerUpKind Kind = PowerUpKind.None;
    public int Quantity = 10;
    public GameObject UseEffectSpawnPoint;
    public GameObject UseEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.gameObject.CompareTag("Player"))
        {
            UseEffect.SafeInstantiate(UseEffectSpawnPoint.transform.position, out _, 2);

            PlayerController pc = collision.gameObject.GetComponentInParent<PlayerController>();
            if ( pc != null )
            {
                pc.AddPowerUp(Kind, Quantity);
            }    

            if ( collision.gameObject.TryGetComponent<FadeOut>(out var fo))
            {
                fo.DestroyWhenFaded = true;
                fo.StartFade();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
