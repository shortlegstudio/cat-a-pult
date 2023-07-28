using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GamePreferences", menuName = "ScriptableObjects/GamePreferences", order = 1)]
public class GamePreferences : ScriptableObject
{
    public static GamePreferences Current => GameController.TheGameData.GamePrefs;

    public Rect CurrentLevelBounds;

    public float GoopGrowthRate = .25f;
    public float GoopGrowthStartDelay = .1f;
    public float MovementGoopDetectionRadius = 2f;
    public float GoopSpreadDetectionRadius = 5f;

    public Color ColorTeamA = Color.red;
    public Color ColorPlayerA = Color.red;
    public Color ColorTeamB = Color.blue;
    public Color ColorPlayerB = Color.blue;
    public Color ColorTeamC = Color.green;
    public Color ColorPlayerC = Color.red;
    public Color ColorTeamD = Color.cyan;
    public Color ColorPlayerD = Color.blue;


    public string[] LevelProgression;

    public GameSettings GameSettings;
    public ColorAndThemeParameters ColorAndTheme;
    public float SeededFieldGoopPercent = .5f;
    public float SeededFieldPowerUpPercent = .02f;
    public float SeededTeamPortalPercent = .01f;
    public GameObject[] SeededFieldPrototypes;
    public GameObject[] SeededFieldPowerUpPrototypes;
    public int SeededFieldGridBoxSize = 50;
    public GameObject FinishGoalProtoType;
    public GameObject TeamPortalProtoType;

    public Vector2 PlayerStartingLocation;
    public int StartingLevelWidth = 100;
    public int StartingLevelHeight = 100;
    public int CurrentLevelWidth = 0;
    public int CurrentLevelHeight = 0;
    public float LevelProgressionSizeIncrease = .2f;
}


[Serializable]
public class GameSettings
{
    public bool MuteAll = false;
    public float MusicVolume = .5f;
    public float EffectVolume = .8f;
}

[Serializable]
public class ColorAndThemeParameters
{
    [Tooltip("The standard highlight color for items, overlayed atop their sprite.")]
    public Color HighlightColor = new Color(152, 255, 0);
}
