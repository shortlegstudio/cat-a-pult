using Assets.Scripts.Extensions;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject UiTitleScreen;
    public GameObject UiOptionsScreen;
    public GameObject UiAboutScreen;
    public GameObject GameOverUi;
    public GameObject InGameUi;
    public GameObject HighScoreEntryUi;
    public ServerComs ServerCommunications;
    public GameDataHolder GameData;
    public DamageDisplayManager DamageDisplayManager;

    [ReadOnly]
    [Tooltip("The current scene/leel being played.")]
    public string CurrentGameScene;

    private static GameController InnerGameController { get; set; }

    public static GameDataHolder CurrentGame => Current.GameData;
    public static GameController Current
    {
        get
        {
            if (null == InnerGameController)
            {
                var goController = GameObject.FindGameObjectWithTag(GameConstants.TagGameController);
                if (goController.TryGetComponent<GameController>(out GameController controller))
                    InnerGameController = controller;

            }
            return InnerGameController;
        }
        set
        {
            InnerGameController = value;
        }
    }

    private void Start()
    {
        GameConstants.Init();

        // Present for game development ease. The controller scene is mandatory. In the production build, the
        // controller scene will be the initial scene and drive the whole thing. In development mode, though,
        // we want to have the level we are working on loaded into the editor. We also want to be able to 
        // press play and have this work. This bit here will locate the loaded level and remember this as the current
        // scene. It will also hide all the Ui scenes. This should allow us a more pleasant dev experience.
        if (SceneManager.sceneCount > 1)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene aScene = SceneManager.GetSceneAt(i);
                if (GameData.GamePrefs.LevelProgression.Contains(aScene.name))
                {
                    CurrentGameScene = aScene.name;
                    UnloadLevel();
                    CurrentGameScene = aScene.name;
                    StartNewGame();
                    break;
                }
            }
        }
        else
        {
            ShowStartMenu();
        }

    }


    bool needToPlayMusic = true;
    private void Update()
    {
        GlobalSpawnQueue.SpawnQueueItems();
    }

    private void UnloadLevel()
    {
        if (CurrentGameScene?.Any() == true)
            SceneManager.UnloadSceneAsync(CurrentGameScene);
        CurrentGameScene = string.Empty;
    }


    public void ShowStartMenu()
    {
        UnloadLevel();
        UiTitleScreen.SetActive(true);
        UiOptionsScreen.SafeSetActive(false);
        UiAboutScreen.SafeSetActive(false);
        GameOverUi.SafeSetActive(false);
        HighScoreEntryUi.SafeSetActive(false);
        InGameUi.SafeSetActive(false);
    }

    public void HideStartMenu()
    {
        UiTitleScreen.SetActive(false);
    }

    public void ReloadCurrentScene()
    {
        if (CurrentGameScene?.Any() == true)
            SceneManager.UnloadSceneAsync(CurrentGameScene);

        SceneManager.LoadSceneAsync(CurrentGameScene, LoadSceneMode.Additive);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        if (CurrentGameScene?.Any() == true)
            SceneManager.UnloadSceneAsync(CurrentGameScene);
        CurrentGameScene = sceneName;
    }

    public void StartNewGame()
    {
        if (string.IsNullOrEmpty(GameData.GameData.InstanceId))
            GameData.GameData.InstanceId = Guid.NewGuid().ToString();

        GameData.GameData.Clear();
        GameData.GameData.GameStartTime = Time.time;

        LoadCurrentScene();
        GameOverUi.SafeSetActive(false);
        HighScoreEntryUi.SafeSetActive(false);
        UiTitleScreen.SafeSetActive(false);
        UiAboutScreen.SafeSetActive(false);
        UiOptionsScreen.SafeSetActive(false);
        InGameUi.SafeSetActive(false);
    }

    public void ShowInGameUi()
    {
        InGameUi.SafeSetActive(true);
    }

    public void HideInGameUi()
    {
        InGameUi.SafeSetActive(false);
    }

    public void LoadCurrentScene()
    {
        int nextSceneIndex = 1;

        if (!string.IsNullOrEmpty(CurrentGameScene))
        {
            for (int i = 0; i < GameData.GamePrefs.LevelProgression.Length; i++)
            {
                if (GameData.GamePrefs.LevelProgression[i] == CurrentGameScene)
                {
                    nextSceneIndex = i;
                    break;
                }
            }
        }

        nextSceneIndex %= GameData.GamePrefs.LevelProgression.Length;
        LoadScene(GameData.GamePrefs.LevelProgression[nextSceneIndex]);
    }

    public void LogGameAchievement(string achievment)
    {
        Debug.Log($"Game achievement: {achievment}.");
        CurrentGame.GameData.AddAchievement(achievment);
    }


    public void ShowGameOverUi()
    {
        HighScoreEntryUi.SafeSetActive(false);
        GameOverUi.SafeSetActive(true);
        InGameUi.SafeSetActive(false);
    }

    public void InitiateGameOver()
    {
        UnloadLevel();
        InGameUi.SafeSetActive(false);

        int gameScore = GameDataHolder.Current.GameData.GetPlayerScore();

        if (GameDataHolder.Current.GamePrefs.HighScoreTable.scores.Count < 10 || GameDataHolder.Current.GamePrefs.HighScoreTable.scores.Any(m => m.score < gameScore))
        {
            HighScoreEntryUi.SetActive(true);
        }
        else
        {
            ServerComs.Current.AddGameScore(new GameScore
            {
                playerId = GameData.GameData.InstanceId,
                playerName = "___",
                score = gameScore
            });

            ShowGameOverUi();
        }
    }

    public void OpenPlantTherapySteam()
    {
        Application.OpenURL("https://store.steampowered.com/app/2505120/Plant_Therapy/");
    }
}
