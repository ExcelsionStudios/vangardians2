using UnityEngine;
//Source: http://wiki.unity3d.com/index.php/TransformRotation2D

public static class TransformExtensions 
{
	public enum Axis { X, Y, Z }
	
	public static void SetRotation2D(this Transform transform, float angle, 
	                                 Space space = Space.World, Axis axis = Axis.Z)
	{
		Quaternion rotation = new Quaternion();
		float halfAngle = (angle * Mathf.Deg2Rad) % (2.0f * Mathf.PI) * 0.5f;
		rotation[(int)axis] = Mathf.Sin(halfAngle);
		rotation.w = Mathf.Cos(halfAngle);
		switch (space)
		{
			case Space.Self:
				transform.localRotation = rotation;
				break;
			default:
				transform.rotation = rotation;
				break;
		}
	}
	
	public static float GetRotation2D(this Transform transform, 
	                                  Space space = Space.World, Axis axis = Axis.Z)
	{
		Quaternion rotation;	
		switch (space)
		{
			case Space.Self:
				rotation = transform.localRotation;
				break;
			default:
				rotation = transform.rotation;
				break;
		}
		return Mathf.Asin( rotation[(int)axis] * Mathf.Sign(rotation.w) ) * 2.0f * Mathf.Rad2Deg;
	}
}