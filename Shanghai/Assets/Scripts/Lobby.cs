using UnityEngine;
using System.Collections;
using LitJson;


public class Lobby: MonoBehaviour {
	//string textFieldString;
	Vector2 scrollPos;
	bool isCreate = false;
	private Vector2 scrollViewVector = Vector2.zero;
	Network network;
	private LobbyParser stringParser;
	string user;
	string roomnum;
	string textFieldString = "";

	public GameObject ItemObject;

	public GameObject CreatePanel;
	public GameObject FriendPanel;
	public GameObject RankingPanel;
	public GameObject OptionPanel;

	public AudioClip buttonsound;

	//초기화
	void Start () 
	{
		network = Network.getInstance();
		network.NetworkStart();

		if(!UserInfo.globalusername.Equals(""))
		{
			GameObject LoginPanel = GameObject.Find ("LoginPanel");
			GameObject UserPanel = GameObject.Find ("UserPanel");
			UILabel idLabel = UserPanel.transform.Find("IdLabel").GetComponent<UILabel>();
			idLabel.text = UserInfo.globalusername;
			UILabel winLabel = UserPanel.transform.Find("WinLabel").GetComponent<UILabel>();
			winLabel.text = UserInfo.globaluserwin.ToString();
			UILabel loseLabel = UserPanel.transform.Find("LoseLabel").GetComponent<UILabel>();
			loseLabel.text = UserInfo.globaluserlose.ToString();
			UILabel scoreLabel = UserPanel.transform.Find("ScoreLabel").GetComponent<UILabel>();
			scoreLabel.text = UserInfo.globaluserscore.ToString();

			LoginPanel.SetActive(false);
		}
	}
	
	// Update is called once per frame

	void Update () {

		if(network.myQ.Count != 0)
		{
			object obj = network.myQ.Dequeue();
			string a =obj.ToString();
			stringParser = new LobbyParser();
			stringParser.StringParser(a);
		}
	}

	void OnGUI(){

		int w = Screen.width / 2;
		int h = Screen.height / 2;
		//textFieldString = GUI.TextField (new Rect (w-250, 0, 500, 50), textFieldString); 

		//랭킹
		if(GUI.Button( new Rect( Screen.width / 2 , (Screen.height*7)/8  , Screen.width / 4 , Screen.height/8 ), "friend"))
		{

			//roomnum = "1";
			//network.sendEnterRoom(UserInfo.globalusername,roomnum);

			//netmarbles Ranking
			//NetmarbleSRanking rank  = GameObject.Find("lobby Object").GetComponent<NetmarbleSRanking>();
			AudioSource.PlayClipAtPoint(buttonsound , transform.position);
			//rank.RankingList();
			RankingPanel.SetActive(true);
			FriendPanel.SetActive(true);
			OptionPanel.SetActive(false);
			CreatePanel.SetActive(false);

			//Application.LoadLevel("game");

		}if(GUI.Button( new Rect( Screen.width / 4 , (Screen.height*7)/8  , Screen.width / 4 , Screen.height/8 ), "Logout"))
		{
			AudioSource.PlayClipAtPoint(buttonsound , transform.position);
			//옵션이 들어갈 부분
			//isCreate = true;
			RankingPanel.SetActive(false);
			FriendPanel.SetActive(false);
			OptionPanel.SetActive(true);
			CreatePanel.SetActive(false);


			//Application.LoadLevel("game");
		}
		if(GUI.Button( new Rect( (Screen.width * 3) / 4 , (Screen.height*7)/8  , Screen.width / 4 , Screen.height/8 ),"Create Room"))
		{
			AudioSource.PlayClipAtPoint(buttonsound , transform.position);
			RankingPanel.SetActive(false);
			FriendPanel.SetActive(false);
			OptionPanel.SetActive(false);
			CreatePanel.SetActive(true);
		}
	}


	public void userInfo(JsonReader obj)
	{
		JsonData  col = JsonMapper.ToObject(obj);
		UserInfo.globalusername = col["id"].ToString();
		UserInfo.globaluserwin = System.Convert.ToInt32(col["win"].ToString());
		UserInfo.globaluserlose = System.Convert.ToInt32(col["lose"].ToString());
		UserInfo.globaluserscore = System.Convert.ToInt32(col["score"].ToString());

		GameObject LoginPanel = GameObject.Find ("LoginPanel");
		GameObject UserPanel = GameObject.Find ("UserPanel");
		UILabel idLabel = UserPanel.transform.Find("IdLabel").GetComponent<UILabel>();
		idLabel.text = UserInfo.globalusername;
		UILabel winLabel = UserPanel.transform.Find("WinLabel").GetComponent<UILabel>();
		winLabel.text = UserInfo.globaluserwin.ToString();
		UILabel loseLabel = UserPanel.transform.Find("LoseLabel").GetComponent<UILabel>();
		loseLabel.text = UserInfo.globaluserlose.ToString();
		UILabel scoreLabel = UserPanel.transform.Find("ScoreLabel").GetComponent<UILabel>();
		scoreLabel.text = UserInfo.globaluserscore.ToString();

	}

	public void startgame()
	{
		Application.LoadLevel("game");
	}

	public void updateRoomList(JsonReader obj)
	{


		JsonData  col = JsonMapper.ToObject(obj);

		//UIGrid grid = GetComponent<UIGrid>();

		GameObject grid =  GameObject.Find("Grid");
		UIGrid gridComponent = grid.GetComponent<UIGrid>();


		for( int i = 0 ; i < col.Count ; i++){

			GameObject roomitem = Instantiate(Resources.Load ("etc/Item"))as GameObject;

			UILabel roomnum = roomitem.transform.Find("Roomnum").GetComponent<UILabel>();
			UILabel size = roomitem.transform.Find("Size").GetComponent<UILabel>();
			UILabel zzang = roomitem.transform.Find("Zzang").GetComponent<UILabel>();
			UILabel roomname = roomitem.transform.Find("Title").GetComponent<UILabel>();
			UILabel roomstate = roomitem.transform.Find("State").GetComponent<UILabel>();
			UILabel button = roomitem.transform.Find("JoinButton").Find("buttonnum").GetComponent<UILabel>();


			button.text = col[i]["roomnum"].ToString();
			roomnum.text = col[i]["roomnum"].ToString();
			roomname.text = col[i]["roomname"].ToString();
			size.text = col[i]["size"].ToString();
			zzang.text = col[i]["zzang"].ToString();
			roomstate.text = col[i]["roomstate"].ToString();

			NGUITools.AddChild(grid , roomitem);
			NGUITools.Destroy(roomitem);

			gridComponent.Reposition();

		}
		//룸 리스트를 그리드에 등록한다. 
	}
}
