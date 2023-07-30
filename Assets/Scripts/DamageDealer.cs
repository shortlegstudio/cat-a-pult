using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public float DamageAmt = 1000;
    public bool DestroyOnUse = true;
    public GameObject DestroyEffect;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameController.CurrentGame.GameData.Health -= 1000;
            if (DestroyOnUse)
            {
                if ( DestroyEffect != null)
                {
                    Instantiate(DestroyEffect, transform.position, transform.rotation);
                    Destroy(gameObject);
                }
            }
        }

    }
}
