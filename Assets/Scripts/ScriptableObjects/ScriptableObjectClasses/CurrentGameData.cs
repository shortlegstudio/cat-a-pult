using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[CreateAssetMenu(fileName = "CurrentGameData", menuName = "ScriptableObjects/CurrentGameData", order = 1)]
public class CurrentGameData : ScriptableObject
{
    public string InstanceId { get; set; }
    public string PlayerName;
    public string[] Achievements;
    public float Health = 100;
    public bool IsDead = false;
    public bool InDeathThrows = false;
    public float MaxHeightReached = 0;
    public bool GameInProgress = false;
    public float GameEndTime = 0;
    public float GameStartTime = 0;
    public float BonusScore = 0;
    public float CurrentBonusModifier = 0;
    public float LastBonusGottenAt = 0;
    public int ChainedBonusCount = 0;

    public void Clear()
    {
        Health = GameDataHolder.Current.NewGameData.Health;
        MaxHeightReached = 0;
        GameStartTime = Time.time;
        GameEndTime = 0;
        Achievements = new string[0];
        GameInProgress = true;
        InDeathThrows = false;
        IsDead = false;
        BonusScore = 0;
        CurrentBonusModifier = 0;
        LastBonusGottenAt = 0;
        ChainedBonusCount = 0;
    }

    public int GetPlayerScore()
    {
        return (int)(BonusScore + (MaxHeightReached * GameDataHolder.Current.GamePrefs.YPositionToHeightConversion));
    }

    public int BonusRewardTriggered(int baseRewardAmt)
    {
        float  currentModifier = CurrentBonusModifier;
        CurrentBonusModifier += GameDataHolder.Current.GamePrefs.BaseBonusModifier;

        if ( Time.time - LastBonusGottenAt < GameDataHolder.Current.GamePrefs.BonusMultiplierWindowLength)
        {
            ChainedBonusCount++;
            currentModifier *= (1 + ChainedBonusCount * GameDataHolder.Current.GamePrefs.ChainedBonus);
        }
        else
        {
            ChainedBonusCount = 0;
        }

        LastBonusGottenAt = Time.time;
        int totalReward = (int)(baseRewardAmt + currentModifier);
        AddBonusScore(totalReward);

        return totalReward;
    }

    public void AddBonusScore(float score) => BonusScore += score;

    public void AddAchievement(string named)
    {
        if (string.IsNullOrEmpty(named))
            return;

        var tmp = new List<string>();
        if (Achievements != null)
            tmp.AddRange(Achievements);
        tmp.Add(named);
        Achievements = tmp.ToArray();
    }
}
