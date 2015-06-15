using UnityEngine;
using System.Collections;

// Stephan - Around 6/07/2015

[RequireComponent(typeof(LineRenderer))]
public class LineRendererTiled : MonoBehaviour 
{

	private LineRenderer lineRenderer;
	void Awake () 
	{
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.material.mainTextureScale = new Vector2( 1,1 );
	}
	
	//CANT read linerenderer positions. We must store and read from our own array of positions, as we set them.
	void OnPreRender () 
	{
	
	}

	float GetRenderLength()
	{
		return 0f;//lineRenderer.Set
	}
}
