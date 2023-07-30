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
            DoReward();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DoReward();
        }
    }

    public void DoReward()
    {


        int inc = GameController.CurrentGame.GameData.BonusRewardTriggered(ScoreAwarded);
        AudioController.PlaySound(SoundEffect);

        if (DamageDisplayManager.Current != null)
            DamageDisplayManager.Current.DisplayDamageAt(inc, transform.position + new Vector3(0, 15, 0));

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
