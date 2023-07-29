using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public float DamageAmt = 1000;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.gameObject.CompareTag("Player"))
        {
            GameController.CurrentGame.GameData.Health -= 1000;
        }
    }
}
