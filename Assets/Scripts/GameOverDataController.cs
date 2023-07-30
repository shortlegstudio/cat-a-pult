using Assets.Scripts.Extensions;
using System.Linq;
using System.Text;
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
        int heightScore = GameDataHolder.Current.GameData.GetPlayerScore();
        MaxHeightLabel.text = $"Height Reached: {heightScore:N0}";

        try
        {
            StringBuilder sb = new StringBuilder();
            foreach (var m in GamePreferences.Current.HighScoreTable.scores.OrderByDescending(m => m.score))
            {
                sb.AppendLine($"{m.playerName} ... {m.score:N0}");
            }

            HighScoreList.text = sb.ToString();

        }
        catch (System.Exception)
        {
        }
    }
}
