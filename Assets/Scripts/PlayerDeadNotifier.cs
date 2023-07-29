using UnityEngine;

public class PlayerDeadNotifier : MonoBehaviour
{
    private void OnDestroy()
    {
        GameDataHolder.Current.GameData.IsDead = true;
    }
}
