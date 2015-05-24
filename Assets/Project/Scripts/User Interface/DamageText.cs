using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public static class CanvasExtensions
{
	public static Vector2 WorldToCanvas(this Canvas canvas,
	                                    Vector3 world_position,
	                                    Camera camera)
	{
		if (camera == null)
		{
			camera = Camera.main;
		}
		
		Vector3 viewport_position = camera.WorldToViewportPoint(world_position);
		RectTransform canvas_rect = canvas.GetComponent<RectTransform>();
		
		return new Vector2((viewport_position.x * canvas_rect.sizeDelta.x) - (canvas_rect.sizeDelta.x * 0.5f),
		                   (viewport_position.y * canvas_rect.sizeDelta.y) - (canvas_rect.sizeDelta.y * 0.5f));
	}
}

public class DamageText : MonoBehaviour 
{
	public Canvas canvas;
	public RectTransform healthBar;
	public Transform objectToFollow;


	public Text damageText;

	void Start()
	{
	}

	void Update()
	{
		// _ui_canvas being the Canvas, _world_point being a point in the world
//		RectTransform rectTrans = healthBar.GetComponent<RectTransform>();
//		rectTrans.anchoredPosition = canvas.WorldToCanvas(objectToFollow.transform.position, Camera.main);
	}
}
