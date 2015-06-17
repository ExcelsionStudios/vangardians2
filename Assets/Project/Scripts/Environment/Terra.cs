/// <summary>
/// 06/17/2015 --- Sergey Bedov
/// Terrain.cs
/// 
/// This script has methods to transform Terrain
/// 
/// </summary>

using UnityEngine;
using System.Collections;

public class Terra : MonoBehaviour
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
			MoveTo(new Vector3 (0,10,0));
			break;
		case 3:
			SetActiveParts(true, false, false);
			MoveTo(new Vector3 (0,20,0));
			break;
		case 4:
			SetActiveParts(false, false, false);
			MoveTo(new Vector3 (0,28,0));
			break;
		default:
			SetActiveParts(true, true, true);
			MoveTo(new Vector3 (0,0,0));
			break;
		}
	}
	private void SetActiveParts(bool bottom, bool mid, bool top)
	{
		TerrainTop.SetActive(top);
		TerrainMid.SetActive(mid);
		TerrainGround.SetActive(bottom);
	}
	private void MoveTo(Vector3 moveToPos)
	{
		GetComponent<LerpChangePosition>().WaitAndMove(moveToPos);
	}
}
