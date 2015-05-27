using UnityEngine;
using System.Collections;

/// Sergey Bedov --- 05/26/2015
/// This is to make Arrow behaviour

public class ArrowBehaviour : MonoBehaviour
{
	[SerializeField]
	private float destroyDelay = 2.0F;

	void Update ()
	{
		transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity);
	}

	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.name == "Ground")
		{
			GetComponent<Rigidbody>().isKinematic = true;
			Destroy(gameObject, destroyDelay);
		}
	}
}
