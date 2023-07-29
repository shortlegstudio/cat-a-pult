using UnityEngine;

[CreateAssetMenu(fileName = "GameDataHolder", menuName = "ScriptableObjects/GameDataHolder", order = 1)]
public class GameDataHolder : ScriptableObject
{
    public static GameDataHolder Current => GameController.CurrentGame;

    public GamePreferences GamePrefs;
    public CurrentGameData GameData;
    public CurrentGameData NewGameData;
}
