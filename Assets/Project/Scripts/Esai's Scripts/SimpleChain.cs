using System.Collections;
using UnityEngine;

//Script by Esai Sollrio
//June 15, 2015

/* Stephan Ennen 7/2/15 - 
+ Added the functionality to automatically detect aspect ratio of line renderer texture. 
  Used to ensure aspect ratio never changes regardless of distance.
+ Added a width variable. This is used with the above to essentially dictate flat size multiplier. 
  (You cant read the width variable of a linerenderer for whatever reason, so we give it a start/end width instead.)
*/ //TODO - Add an "Anchor" option that effects how the texture appears to be added and which appears to be static. (As is, the anchor is at the location of 'chainBase')

public class SimpleChain : MonoBehaviour 
{
	public LineRenderer myLine;
	public Transform chainHead;
	public Transform chainBase; //Might be easier just to attach this script to chainBase instead. - Stephan 7/2/15
	[Range(0.1f, 10f)] 
	public float width = 1.0f;


	private float imgAspectRatio; //Used so that the mat is scaled to be displayed correctly, regardless of length.
	void Start () 
	{
		width = Mathf.Max(0.01f, width);
		myLine.SetWidth( width, width );

		imgAspectRatio = GetRatio();
	}

	void LateUpdate () //LateUpdate() is better because other scripts might modify variables later in Update() than we do. - Stephan 7/2/15
	{
		myLine.SetPosition (0, chainBase.position);
		myLine.SetPosition (1, chainHead.position);
		myLine.material.mainTextureScale = new Vector2(Vector3.Distance(chainBase.transform.position, chainHead.position) / imgAspectRatio / width, 1f);
	}

	private float GetRatio() //Gets the ratio of the linerenderer's texture size, in the format of "1 : X". This value cannot return less than one.
	{
		Texture tex = myLine.material.mainTexture;
		if( tex.width == tex.height )
			return 1.0f;
		else
		{
			float bigger  = (float)Mathf.Max( tex.width, tex.height );
			float smaller = (float)Mathf.Min( tex.width, tex.height );
			return bigger / smaller;
		}
	}
}
