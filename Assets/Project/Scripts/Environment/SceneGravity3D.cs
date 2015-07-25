using UnityEngine;
using System.Collections;

//Stephan Ennen 7/24/15

//Used to modify gravity on a single-scene basis, so changing project settings doesn't mess up other scenes. 
//Resets to whatever was in ProjectSettings when removed.
public class SceneGravity3D : MonoBehaviour 
{
	public Vector3 sceneGravity = new Vector3(0.0f, -9.81f, 0.0f);
	private Vector3 defaultGravity;
	void Awake () 
	{
		defaultGravity = Physics.gravity;
		Physics.gravity = sceneGravity;
	}
	
	void OnDestroy()
	{
		Physics.gravity = defaultGravity;
	}
}
