using UnityEngine;

public class Rewarder : MonoBehaviour
{
    public int ScoreAwarded = 20;
    public GameObject VisualEffect;
    public bool DestroyOnUse = true;
    public Sounds SoundEffect;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameController.CurrentGame.GameData.AddBonusScore(ScoreAwarded);
            AudioController.PlaySound(SoundEffect);

            if (DestroyOnUse)
            {
                if (VisualEffect != null)
                {
                    Instantiate(VisualEffect, transform.position, transform.rotation);
                }
                Destroy(gameObject);
            }
        }

    }

}
