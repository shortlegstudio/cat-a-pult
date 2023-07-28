using Assets.Scripts.Extensions;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public GameObject UseEffect;
    public GameObject UseEffectSpawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            UseEffect.SafeInstantiate(UseEffectSpawnPoint.transform.position, out _, 2);
        }
    }
}
