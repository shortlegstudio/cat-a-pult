using Assets.Scripts.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverDataController : MonoBehaviour
{
    public GameObject NewHighScoreElement;
    public TextMeshProUGUI MaxHeightLabel;
    public TextMeshProUGUI HighScoreList;

    public void OnEnable()
    {
        NewHighScoreElement.SafeSetActive(false);
        int heightScore = (int)(GameDataHolder.Current.GameData.MaxHeightReached * GameDataHolder.Current.GamePrefs.YPositionToHeightConversion);
        MaxHeightLabel.text = $"Height Reached: {heightScore:N0}";
        HighScoreList.text = $"XXX .... {heightScore}";
    }
}
