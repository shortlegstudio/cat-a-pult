using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplayer : MonoBehaviour
{
    public TextMeshProUGUI UiElement;

    void Update()
    {
        UiElement.text = $"{GameDataHolder.Current.GameData.GetPlayerScore():N0}";
    }
}
