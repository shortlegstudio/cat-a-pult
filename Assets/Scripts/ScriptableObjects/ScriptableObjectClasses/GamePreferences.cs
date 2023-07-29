using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GamePreferences", menuName = "ScriptableObjects/GamePreferences", order = 1)]
public class GamePreferences : ScriptableObject
{
    public static GamePreferences Current => GameController.CurrentGame.GamePrefs;

    public Rect CurrentLevelBounds;

    public string[] LevelProgression;

    public GameSettings GameSettings;
    public float SeededFieldGoopPercent = .5f;
    public float SeededFieldPowerUpPercent = .02f;
    public float SeededTeamPortalPercent = .01f;
    public GameObject[] SeededFieldPrototypes;
    public GameObject[] SeededFieldPowerUpPrototypes;
    public int SeededFieldGridBoxSize = 50;

    public Vector2 PlayerStartingLocation;
    public int StartingLevelWidth = 1000;
    public int StartingLevelHeight = 1000000;
    public int CurrentLevelWidth = 0;
    public int CurrentLevelHeight = 0;
    public float LevelProgressionSizeIncrease = .2f;

    public float YPositionToHeightConversion = .1f;
}


[Serializable]
public class GameSettings
{
    public bool MuteAll = false;
    public float MusicVolume = .5f;
    public float EffectVolume = .8f;
}
