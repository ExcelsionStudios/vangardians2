using UnityEngine;
using System.Collections;

public class DragOnClick : MonoBehaviour {

	// Use this for initialization
	public bool dragging = false;
	public GameObject currentDraggable;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (dragging) {
			currentDraggable.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
			if(Input.GetMouseButtonUp(0)){
				dragging = false;
			}
		}
	}

	public void setDragging(){
		dragging = true;
	}
	public void instantiateButton(GameObject obj){
		currentDraggable = (GameObject)Instantiate (obj, Input.mousePosition, Quaternion.identity);
		Debug.Log ("RAN!");
	}
}
