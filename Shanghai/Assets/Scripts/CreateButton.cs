using UnityEngine;
using System.Collections;

public class CreateButton : MonoBehaviour {
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

		if(transform.parent.name.Equals("CreatePanel")){
			Debug.Log ("방인원 수");
			UILabel Titlename = transform.parent.Find("Input").Find("Label").GetComponent<UILabel>();
			UILabel HintList = transform.parent.Find("Hint").Find("Label").GetComponent<UILabel>();
			UILabel ShuffleList = transform.parent.Find("Shuffle").Find("Label").GetComponent<UILabel>();

			network.sendCreateRoom( Titlename.text , UserInfo.globalusername , HintList.text , ShuffleList.text );
		}
		if(transform.parent.name.Equals("LoginPanel"))
		{
			UILabel UserId = transform.parent.Find("Input").Find("Label").GetComponent<UILabel>();
			UserInfo.globalusername= UserId.text;

			GameObject LoginPanel = GameObject.Find("LoginPanel");
			LoginPanel.SetActive(false);
			network.sendUserName( UserId.text ,UserInfo.globalusercn);

		}
	}
}
