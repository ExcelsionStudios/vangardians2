/// <summary>
/// 06/17/2015 --- Sergey Bedov
/// Terrain.cs
/// 
/// This script has methods to transform Terrain
/// 
/// </summary>

using UnityEngine;
using System.Collections;

public class Terrain : MonoBehaviour
{
	public int LevelNum = 1;

	public GameObject TerrainTop;
	public GameObject TerrainMid;
	public GameObject TerrainGround;

	public void ChouseLevel (int num)
	{
		switch(num)
		{
		case 2:
			SetActiveParts(true, true, false);
			MoveCameraTo(new Vector3 (0,30,-15));
			break;
		case 3:
			SetActiveParts(true, false, false);
			MoveCameraTo(new Vector3 (0,30,-18));
			break;
		case 4:
			SetActiveParts(false, false, false);
			MoveCameraTo(new Vector3 (0,30,-20));
			break;
		default:
			SetActiveParts(true, true, true);
			MoveCameraTo(new Vector3 (0,30,-11));
			break;
		}
	}
	private void SetActiveParts(bool bottom, bool mid, bool top)
	{
		TerrainTop.SetActive(top);
		TerrainMid.SetActive(mid);
		TerrainGround.SetActive(bottom);
	}
	private void MoveCameraTo(Vector3 moveToPos)
	{
		Camera.main.GetComponent<LerpChangePosition>().WaitAndMove(moveToPos);
	}
}
