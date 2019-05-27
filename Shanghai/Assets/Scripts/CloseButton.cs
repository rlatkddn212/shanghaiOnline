using UnityEngine;
using System.Collections;

public class CloseButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}
	void OnClick()
	{
		if(transform.parent.name.Equals("CreatePanel")){
			GameObject CreatePanel = GameObject.Find("CreatePanel");
			CreatePanel.SetActive(false);
		}
	}
}
