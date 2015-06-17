/// <summary>
/// 06/17/2015 --- Sergey Bedov
/// LerpChangePosition.cs
/// 
/// This is to WaitAndMove (wait for "delayTime", and then move using Lerp function) an object to target position.
/// 
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LerpChangePosition : MonoBehaviour {

	public float delayTime;
	private Vector3 startPos;
	public Vector3 targetPos;

	public void WaitAndMove(Vector3 target)
	{
		startPos = transform.position;
		targetPos = target;
		StartCoroutine(WaitAndMove(delayTime));
	}
	
	IEnumerator WaitAndMove(float delayTime){
		yield return new WaitForSeconds(delayTime); // start at time X
		float startTime=Time.time; // Time.time contains current frame time, so remember starting point
		while(Time.time-startTime<=1){ // until one second passed
			transform.position=Vector3.Lerp(startPos,targetPos,Time.time-startTime); // lerp from A to B in one second
			yield return 1; // wait for next frame
		}
	}
}
