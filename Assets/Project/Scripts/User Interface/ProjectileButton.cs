using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Matt McGrath - 5/21/2015

// Helper class which a "Change Projectile" Button will use to grab the OnButtonPressed method for its OnClick event.
// Doing this will update the Button with the name of the currently selected type of projectile, as wells witch to that projectile
// by accessing it and changing it from the projectile spawner object's "FlingProjectilePrediction" script.
// TODO: Well if we even keep this feature (just prototype) make it so clicking button doesn't fire a projectile.
public class ProjectileButton : MonoBehaviour 
{
	string[] projectileNames = { "Standard", "Bouncy", "Rolling", "Explosive", "Land Mine", "Barrier", "Scatter Shot" };
	private int stringIndex;
	public Button projectileButton;
	public Text projectileButtonText;

	public GameObject[] projectilePrefabs = new GameObject[3];

	// Use this for initialization
	void Start () 
	{
		stringIndex = 0;
		projectileButtonText.text = projectileNames[stringIndex];
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	// Link this method to the Button's OnClick event.
	public void OnButtonPressed()
	{
		// Just increase the index, clamping it between 0 and array length - 1 via the mod operator.
		stringIndex++;
		stringIndex = stringIndex % projectileNames.Length;
		Debug.Log (stringIndex);
		projectileButtonText.text = projectileNames[stringIndex];

		// Now change the type of projectile. We'll have to set these from our scene into the "projectilePrefabs" array in the same or der as the string names appear, for now.
		GameObject flingProjectileObject = GameObject.Find("ProjectileSpawnerPrediction");
		flingProjectileObject.GetComponent<FlingProjectilePrediction>().projectilePrefab = projectilePrefabs[stringIndex];
	}
}
