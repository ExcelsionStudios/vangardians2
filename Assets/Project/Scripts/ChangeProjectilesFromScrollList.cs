using UnityEngine;
using System.Collections;

// Matt McGrath - 5/27/2015

// Allows changing prefabs from a scroll list rather than radial menu.
public class ChangeProjectilesFromScrollList : MonoBehaviour 
{
	public FlingProjectilePrediction playerTower;

	public void changePrefab(GameObject obj)
	{
		playerTower.projectilePrefab = obj;
	}
}
