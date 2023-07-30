using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LevelController : MonoBehaviour
{
    [Tooltip("Items spawned on this level using the GlobalSpawnQueue will get this item as their parent if they are not specifically given one, allowing hiding of all spawned items when unloading the level.")]
    public GameObject LevelSpawnItemParent;

    [Tooltip("Define the playable area on the scene. Used to help the camera understand how far it can pan " +
        "as it follows the player.")]
    public Rect LevelBounds;

    public GameObject FieldParent;
    public PlayerController Player;
    public GameObject NextLevelUi;
    public GameObject PauseUi;

    internal void LevelComplete(bool v)
    {
        NextLevelUi.SetActive(true);
    }
    public void ShowPauseUi()
    {
        PauseUi.SetActive(true);
    }

    public void QuitToMain()
    {
        PauseUi.SetActive(false);
        GameController.Current.ShowStartMenu();
    }

    private void Start()
    {
        GlobalSpawnQueue.DefaultParentObject = LevelSpawnItemParent != null ? LevelSpawnItemParent : gameObject;
        ServerComs.Current.GetHighScores();
        GameController.Current.ShowInGameUi();
        InitField();
    }

    private void Update()
    {
        CheckGoalsAndProgression();
    }


    public void ProgressToNextLevel()
    {
        IncreaseBoardSize();
        GameController.Current.ReloadCurrentScene();
    }

    public void InitField()
    {
        //AudioController.Current.PlayMusic();
        InitPlayer();
        CreateNewField();
        InitCameraFollower();
    }

    public void IncreaseBoardSize()
    {
        GamePreferences.Current.CurrentLevelWidth = GamePreferences.Current.CurrentLevelWidth + (int)(GamePreferences.Current.CurrentLevelWidth * GamePreferences.Current.LevelProgressionSizeIncrease);
        GamePreferences.Current.CurrentLevelHeight = GamePreferences.Current.CurrentLevelHeight + (int)(GamePreferences.Current.CurrentLevelHeight * GamePreferences.Current.LevelProgressionSizeIncrease);
    }

    public void ResetGameBoardSize()
    {
        GamePreferences.Current.CurrentLevelHeight = GamePreferences.Current.StartingLevelHeight;
        GamePreferences.Current.CurrentLevelWidth = GamePreferences.Current.StartingLevelWidth;
    }

    public static Vector3 NearestGridCenterPoint(Vector3 pt)
    {
        pt = pt - (Vector3)GamePreferences.Current.CurrentLevelBounds.min;

        int xPart = (int)(pt.x / GamePreferences.Current.SeededFieldGridBoxSize);
        int yPart = (int)(pt.y / GamePreferences.Current.SeededFieldGridBoxSize);

        return NearestGridCenterPoint(xPart, yPart);
    }

    public static Vector3 NearestGridCenterPoint(int gridX, int gridY)
    {
        Vector3 centerPt = new Vector3(gridX * GamePreferences.Current.SeededFieldGridBoxSize + GamePreferences.Current.CurrentLevelBounds.xMin, gridY * GamePreferences.Current.SeededFieldGridBoxSize + GamePreferences.Current.CurrentLevelBounds.yMin, 0);
        return centerPt;
    }

    public static int GridXNum => (int)(GamePreferences.Current.CurrentLevelBounds.width / GamePreferences.Current.SeededFieldGridBoxSize);
    public static int GridYNum => (int)(GamePreferences.Current.CurrentLevelBounds.height / GamePreferences.Current.SeededFieldGridBoxSize);
    public static int TotalFieldCells => GridXNum * GridYNum;

    public void CreateNewField()
    {
        return;

        if (GamePreferences.Current.CurrentLevelHeight == 0 || GamePreferences.Current.CurrentLevelWidth == 0)
        {
            ResetGameBoardSize();
        }

        Player.transform.position = GamePreferences.Current.PlayerStartingLocation;
        LevelBounds.xMin = GamePreferences.Current.PlayerStartingLocation.x - GamePreferences.Current.CurrentLevelWidth / 2;
        LevelBounds.xMax = LevelBounds.xMin + GamePreferences.Current.CurrentLevelWidth;
        LevelBounds.yMin = GamePreferences.Current.PlayerStartingLocation.y - 20;
        LevelBounds.yMax = LevelBounds.yMin + GamePreferences.Current.CurrentLevelHeight;

        GamePreferences.Current.CurrentLevelBounds = LevelBounds;

        for (int i = 0; i < FieldParent.transform.childCount; i++)
        {
            var child = FieldParent.transform.GetChild(i);
            GameObject.Destroy(child.gameObject);
        }

        List<string> usedGrids = new List<string>();

        int goalAdded = 0;
        int powerUpsAdded = 0;
        int totalSeededItems = (int)(TotalFieldCells * GamePreferences.Current.SeededFieldGoopPercent);
        int totalPowerUps = (int)(totalSeededItems * GamePreferences.Current.SeededFieldPowerUpPercent);
        int totalTeamPortals = 1 + (int)(totalSeededItems * GamePreferences.Current.SeededTeamPortalPercent);

        int n = 0;
        while (n < totalSeededItems)
        {
            int xLoc = Random.Range(0, GridXNum);
            int yLoc = Random.Range(0, GridYNum);
            var label = $"{xLoc}-{yLoc}";

            if (usedGrids.Contains(label))
            {
                continue;
            }

            usedGrids.Add(label);
            n++;

            GameObject proto = GamePreferences.Current.SeededFieldPrototypes[Random.Range(0, GamePreferences.Current.SeededFieldPrototypes.Length)];
            Vector3 loc = NearestGridCenterPoint(xLoc, yLoc);// new Vector3(xLoc * GamePreferences.Current.SeededFieldGridBoxSize + LevelBounds.xMin, yLoc * GamePreferences.Current.SeededFieldGridBoxSize + LevelBounds.yMin, 0);

            GameObject created = GameObject.Instantiate(proto, loc, Quaternion.identity, FieldParent.transform);

            //if (powerUpsAdded < totalPowerUps)
            //{
            //    GameObject powerUp = GamePreferences.Current.SeededFieldPowerUpPrototypes[Random.Range(0, GamePreferences.Current.SeededFieldPowerUpPrototypes.Length)];
            //    GameObject pu = GameObject.Instantiate(powerUp, loc, Quaternion.identity, FieldParent.transform);
            //    powerUpsAdded++;
            //}
            //else if ( goalAdded < 1 )
            //{
            //    GameObject pu = GameObject.Instantiate(GamePreferences.Current.FinishGoalProtoType, loc, Quaternion.identity, FieldParent.transform);
            //    goalAdded++;
            //}
        }

    }
    private void CheckGoalsAndProgression()
    {
        if (GameDataHolder.Current.GameData.InDeathThrows)
        {
            ServerComs.Current.GetHighScores();
        }

        if (GameDataHolder.Current.GameData.IsDead)
        {
            EndGame(false);
        }
    }


    public void EndGame(bool didWin)
    {
        if (!GameDataHolder.Current.GameData.GameInProgress)
        {
            return;
        }

        GameDataHolder.Current.GameData.GameInProgress = false;
        GameDataHolder.Current.GameData.GameEndTime = Time.time;

        if (didWin)
        {
            //ShowGameWinUi();
        }
        else
        {
            GameController.Current.InitiateGameOver();
        }
    }

    private void InitPlayer()
    {
        //Player = FindObjectOfType<PlayerController>();
    }

    private void InitCameraFollower()
    {
        if (TryGetComponent<Camera2DFollow>(out var follower))
        {
            follower.Init(Camera.main, LevelBounds);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(LevelBounds.center, LevelBounds.size);

        Vector2 box2 = new Vector2(LevelBounds.size.x - 2, LevelBounds.size.y - 2);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(LevelBounds.center, box2);

        //Gizmos.color = Color.white;
        //Vector3 gridSize = new Vector3(GamePreferences.Current.SeededFieldGridBoxSize, GamePreferences.Current.SeededFieldGridBoxSize, 0);
        //for (int i = 0; i<GridXNum; i++)
        //{
        //    for (int j=0; j<GridYNum; j++)
        //    {
        //        Vector3 cp = NearestGridCenterPoint(i, j);
        //        Gizmos.DrawWireCube(cp, gridSize );
        //    }
        //}
    }

    internal void InitiatePlayerThrust()
    {
        Player.InitiateThrust();
    }
}
