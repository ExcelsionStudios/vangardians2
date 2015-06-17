/// <summary>
/// 06/17/2015 --- Sergey Bedov
/// TerrainController.cs
/// 
/// This script is to make the terrain changes while the Player progress
/// 
/// </summary>

using UnityEngine;
using System.Collections;

public class TerrainController : MonoBehaviour
{
	public GameObject Terrain_;

	private int LevelNum;

	void Start()
	{
		LevelNum = (int)GetComponent<UnityEngine.UI.Slider>().value;
	}

	public void ChangeLevel()
	{
		LevelNum = (int)GetComponent<UnityEngine.UI.Slider>().value;;
		Debug.Log("Level Changed to " + LevelNum);
		Terrain_.GetComponent<Terrain>().ChouseLevel(LevelNum);
	}
}
