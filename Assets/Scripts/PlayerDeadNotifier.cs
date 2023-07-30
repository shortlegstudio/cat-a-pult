using UnityEngine;

public class PlayerDeadNotifier : MonoBehaviour
{
    private void Start()
    {
        GameDataHolder.Current.GameData.InDeathThrows = true;
        GameDataHolder.Current.GameData.IsDead = false;
    }

    private void OnDestroy()
    {
        GameDataHolder.Current.GameData.IsDead = true;
        GameDataHolder.Current.GameData.InDeathThrows = false;
    }
}
