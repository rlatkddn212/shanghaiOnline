using UnityEngine;
using System.Collections;

public class ClickButton : MonoBehaviour {
	Network network;
	// Use this for initialization
	void Start () {
		network = Network.getInstance();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnClick()
	{
		UILabel button = transform.Find("buttonnum").GetComponent<UILabel>();
		UILabel size = transform.parent.Find("Size").GetComponent<UILabel>();
		//방인원 수 체크 최대 6명
		Debug.Log ("방인원 수" + size.text);
		if(System.Convert.ToInt32(size.text) < 5 ){
			Debug.Log ("방인원 수" + size.text);
			network.sendEnterRoom(UserInfo.globalusername,button.text);
		}
	}
}
