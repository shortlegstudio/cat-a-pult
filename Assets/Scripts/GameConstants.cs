using UnityEngine;

public static class GameConstants
{
    public static string TagGameController = "GameController";
    public static string TagLevelController = "LevelController";
    public static string GamePointer = "GamePointer";
	public static string Default = "Default";
	public static string Goop = "Goop";


	public static int LayerMaskGoop;
	public static int LayerMaskDefault;

	public static int SortingLayerDefaultId;

	public static void Init()
	{
		LayerMaskDefault = 1 << LayerMask.NameToLayer(Default);
		LayerMaskGoop = 1 << LayerMask.NameToLayer(Goop);

		SortingLayerDefaultId = SortingLayer.NameToID(Default);
	}

}
