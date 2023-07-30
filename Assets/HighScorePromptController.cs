using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class HighScorePromptController : MonoBehaviour
{
    public TextMeshProUGUI ScoreField;
    public UnityEvent OnComplete;
    public TMP_InputField InputField;


    void OnEnable()
    {
        if (GameDataHolder.Current != null)
        {
            int score = GameDataHolder.Current.GameData.GetPlayerScore();
            ScoreField.text = $"{score:N0}";
        }

        InputField.ActivateInputField();
    }

    public void OnNameChanged(string name)
    {
        int score = GameDataHolder.Current.GameData.GetPlayerScore();
        var theirScore = new GameScore
        {
            playerId = GameDataHolder.Current.GameData.InstanceId,
            playerName = name,
            score = score
        };

        GamePreferences.Current.HighScoreTable.AddScore(theirScore);

        ServerComs.Current.AddGameScore(theirScore);
        OnComplete?.Invoke();
    }
}
