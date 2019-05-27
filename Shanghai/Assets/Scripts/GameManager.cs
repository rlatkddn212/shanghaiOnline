/*
 *게임을관리하는클래~스 
 * 
 * 
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class GameManager : MonoBehaviour {

	private GameObject m_TouchedObject = null;
	private RaycastHit m_hit;

	private int clickNum;
	private Tile clickTile;
	private int removablecount;
	private int remaincount;
	public GUIText removebleText;
	private int[] displaynum;
	public GUIText[] userText;
	private Parser stringParser;
	private PlayState playerState;
	public Camera observerCamera;

	/* 상 수*/
	public static float xDivSize = 7;
	public static float yDivSize =5;
	
	public static int xTileSum = 35;
	public static int yTileSum = 20;
	public static int zTileSum = 7;
	
	public static float xgap = 0.001f;
	public static float ygap = 0.001f;
	public static float zgap = 0.06f;
	
	public static float xyselectgap = 0.03f;
	public static float zselectgap = 0.001f;

	public AudioClip selectsound;
	public AudioClip startsound;
	public AudioClip winsound;
	public AudioClip losesound;
	public AudioClip backgoundsound;
	public AudioClip gameoversound;
	public AudioClip removesound;
	public AudioClip buttonsound;


	public GUISkin ExitButton;
	public GUISkin HintButton;
	public GUISkin ShuffleButton;
	public GUISkin DefaultButton;

	Network network;
	GameObject hint2;

	OppuserInfo[] oppuser = new OppuserInfo[5]{null,null,null,null,null};

	//시간관리
	int limittime = 10;
	int falltime;
	float inttimer;
	// Use this for initialization
	float elapsed = 0f;


	//맵선택관련
	private int currentSlide = 0;
	GameObject[] tempMap;
	//번호관련
	GameObject[] BigNum;
	GameObject[] Ranking;
	GameObject[] ShuffleNum;
	GameObject[] HintNum;

	GameObject Zzangtag;

	Opponent[] opp;

	//ArrayList oppList = new ArrayList();

	bool isObserverSetup = false;
	bool isUserSetup = false;
	bool isZzangSetup = false;
	bool isPlaygameSetup = false;
	bool isGameoverSetup = false;

	bool isInstallMap = false;

	//플레이어상태
	enum PlayState
	{
		OBSERVER,
		USER,
		ZZANG,
		PLAYGAME,
		GAMEOVER,
	};

	private GameManager()
	{
	}

	void Awake()
	{
		Application.runInBackground = true;
		GetComponent<AudioSource>().Stop();
	}

	void Start () {
		network = Network.getInstance();
		//network.NetworkStart();

		displaynum = new int[4];

		tempMap = new  GameObject[3];
		BigNum = new GameObject[5];
		Ranking = new GameObject[5];
		ShuffleNum = new GameObject[5];
		HintNum = new GameObject[5];
		Zzangtag = new GameObject();
		opp = new Opponent[5];


		oppuser[0] = new OppuserInfo();
		oppuser[1] = new OppuserInfo();
		oppuser[2] = new OppuserInfo();
		oppuser[3] = new OppuserInfo();
		oppuser[4] = new OppuserInfo();

		playerState = PlayState.OBSERVER;
		tempMap[0] = Instantiate(Resources.Load("Map_1"))as GameObject;
		tempMap[1] = Instantiate(Resources.Load("Map_2"))as GameObject;
		tempMap[2] = Instantiate(Resources.Load("Map_3"))as GameObject;
		
		Zzangtag = Instantiate(Resources.Load ("Prefabs/zzang"))as GameObject;
		Zzangtag.transform.position = new Vector3(1,1,1);

		for(int i = 0 ; i < 5 ; i++ ){
			BigNum[i] = Instantiate(Resources.Load ("etc/BigNum_"+ (i+1)))as GameObject;
			Ranking[i] = Instantiate(Resources.Load ("etc/Ranking_"+ (i+1)))as GameObject;

			BigNum[i].transform.position = new Vector3(1,1,1);
			Ranking[i].transform.position = new Vector3(1,1,1);
		}

		network.sendObserver();

	}
	
	// Update is called once per frame
	void Update () {

		switch(playerState)
		{
		case PlayState.OBSERVER:
			observerMap ();
			break;
		case PlayState.USER:
			recvMap ();
			break;
		case PlayState.ZZANG:
			SelectMap();
			break;
		case PlayState.PLAYGAME:
			PlayGame();
			break;
		case PlayState.GAMEOVER:
			GameOver();
			break;
		}
	}

	void InitGame()
	{
		//찌꺼기를지우고~

		GameObject[] removeTiles = GameObject.FindGameObjectsWithTag("Tile");

		foreach (GameObject removeTile in removeTiles)
		{
			Destroy(removeTile);
		}

		GameObject[] removeOppTiles = GameObject.FindGameObjectsWithTag("oppTile");
		
		foreach (GameObject removeOppTile in removeOppTiles)
		{
			Destroy(removeOppTile);
		}

		for(int i = 0 ; i < 5 ; i++ ){
			BigNum[i].transform.position = new Vector3(1,1,1);
			Ranking[i].transform.position = new Vector3(1,1,1);
		}
		InitTime();

		//TileMap.init();
	}

	// 옵저버 모드
	void observerMap ()
	{

		if(network.myQ.Count != 0)
		{
			object obj = network.myQ.Dequeue();
			stringParser = new Parser();
			string a =obj.ToString();
			stringParser.StringParser(a);
		}

		if (Input.GetMouseButtonDown (0)) {
			//옵저버화면을터치함
			//Camera observer =  GameObject.Find("ObserverCamera").GetComponent<GameManager>() as Camera;
			Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			Debug.Log ("x : " + mousePosition.x + "y : " + mousePosition.y);


			if( mousePosition.x > 4.7f && mousePosition.x < 6.7f
			   && mousePosition.y < 4.2f && mousePosition.y > 3.15f )
			{
				Debug.Log ("1번화면");
				observerCamera.transform.position = new Vector3( 6.05f , 3.91f , -10);
			}
			if( mousePosition.x > 4.7f && mousePosition.x < 6.7f
			   && mousePosition.y < 3.15f && mousePosition.y > 2.10f  )
			{
				Debug.Log ("2번화면");
				observerCamera.transform.position = new Vector3( 6.05f , 2.83f , -10);
			}

			if( mousePosition.x > 4.7f && mousePosition.x < 6.7f
			   && mousePosition.y < 2.10f && mousePosition.y > 1.05f )
			{
				Debug.Log ("3번화면");
				observerCamera.transform.position = new Vector3( 6.05f , 1.75f , -10);
			}

			if( mousePosition.x > 4.7f && mousePosition.x < 6.7f
			   && mousePosition.y < 1.05f && mousePosition.y > 0  )
			{
				Debug.Log ("4번화면");
				observerCamera.transform.position = new Vector3( 6.05f , 0.67f , -10);
			}
		}
	}

	//방장 모드
	void SelectMap()
	{
		if(!isZzangSetup){
			
			tempMap[0].transform.position = new Vector3(3.2f,2,-5);
			isZzangSetup = true;
		}

		//맵을 받음
		if(network.myQ.Count != 0)
		{
			Debug.Log ("map");
			object obj = network.myQ.Dequeue();
			stringParser = new Parser();
			string a =obj.ToString();
			stringParser.StringParser(a);
			//상태바꾼~다
			isZzangSetup = true;
		}
	}

	//유저모~드
	void recvMap()
	{
		if(network.myQ.Count != 0)
		{
			object obj = network.myQ.Dequeue();
			stringParser = new Parser();
			string a =obj.ToString();

			stringParser.StringParser(a);

			//상태바꾼~다
			//playerState = PlayState.PLAYGAME;
		}
	}

	//게임시작모드
	void PlayGame()
	{

		elapsed += Time.deltaTime;
		
		if(elapsed >= 1)
		{
			inttimer += elapsed;
			elapsed = 0;
			falltime = limittime - (int)(inttimer);

			if(falltime  < 0)
			{
				playerState = PlayState.GAMEOVER;
				network.netGameOver();
				return;
			}
			GameObject timebar = GameObject.Find("PlayInfo_11");
			timebar.transform.localScale = new Vector3(1.3f,1.27f*((float)falltime/limittime),0);
			
			int ten = falltime/10;
			int one = falltime%10;
			
			settimesprite(ten, one);
		}

		//logic
		if(network.myQ.Count != 0)
		{
			object obj = network.myQ.Dequeue();
			string a =obj.ToString();
			stringParser.StringParser(a);
		}

		//제거 가능한 타일 개수검사
		removablecount = TileMap.removableTile();
		remaincount = TileMap.remainTile();

		//가능한 패 확인 -> 없다면 패배 구현해야함
		if(remaincount==0)
		{
			playerState = PlayState.GAMEOVER;
			network.netGameWin();
			return;
		}

		//input & update
		if (Input.GetMouseButtonDown (0)) {
			CheckTile ();
		}
	}

	//제한시간 관련 메소드
	void InitTime()
	{
		GameObject timebar = GameObject.Find("PlayInfo_11");
		timebar.transform.localScale = new Vector3(1.3f,1.27f,0);
		falltime = 10;
		inttimer = 0f;
	}

	void settimesprite(int ten, int one)
	{
		GameObject ob = GameObject.Find("ten");
		ob.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("image/PlayInfo_"+(ten));

		GameObject ob2 = GameObject.Find("one");
		ob2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("image/PlayInfo_"+(one));
	}

	//게임결과모 드~
	void GameOver()
	{
		if(network.myQ.Count != 0)
		{
			
			object obj = network.myQ.Dequeue();
			stringParser = new Parser();
			string a =obj.ToString();
			
			stringParser.StringParser(a);
		}
	}

	// 타일이 클릭된거
	void CheckTile()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Physics.Raycast (ray, out m_hit, Mathf.Infinity)) {
			GameUpdate();
		}
	}

	//타일 변화를 업데이트함
	void GameUpdate()
	{

		int clickX = 0;
		int clickY = 0;
		int clickZ = 0;
		//누른 객체의 좌표를불러옴
		Tile tile = m_hit.collider.gameObject.GetComponent<Tile>();
		if(tile != null)
		{
			clickX = tile.getX();
			clickY = tile.getY();
			clickZ = tile.getZ();

		}
		//만약 전에 누른게 없다면 선택한다.
		if( m_TouchedObject == null)
		{
			//터치가 가능한 패인지 검사한다.
			if(TileMap.isTouchPossible(clickX , clickY , clickZ))
			{
				//select
				AudioSource.PlayClipAtPoint( selectsound , transform.position);
				m_TouchedObject = m_hit.collider.gameObject;
				TileMap.ClickTile(clickX , clickY , clickZ);
				TileMap.setClicked(clickX , clickY , clickZ);
			}

		//만약 전에 누른 타일이 있다면
		}else
		{
			//터치가 가능한 패인지 검사한다.
			if(TileMap.isTouchPossible(clickX , clickY , clickZ))
			{
				AudioSource.PlayClipAtPoint( selectsound , transform.position);
				//만약 같은패를 눌렸다면 빠져나온다.
				if(m_TouchedObject.Equals(m_hit.collider.gameObject))
				{
					return;
				}
				//같은 모양패를 눌렸다면 제거한다.
				else if( TileMap.m_MapArr[TileMap.getClickedX(),
				                          TileMap.getClickedY(),
				                          TileMap.getClickedZ()] == TileMap.m_MapArr[clickX,clickY,clickZ])
				{

					Destroy(m_TouchedObject);
					Destroy(m_hit.collider.gameObject);

					AudioSource.PlayClipAtPoint( removesound , transform.position);

					// 삭제 할내 용

					InitTime();

					TileMap.removeArr(clickX,clickY,clickZ);
					StartCoroutine(DestoryPlayerTile(clickX,clickY,clickZ));

					TileMap.removeArr(TileMap.getClickedX(),TileMap.getClickedY(),TileMap.getClickedZ());
					StartCoroutine(DestoryPlayerTile(TileMap.getClickedX(),TileMap.getClickedY(),TileMap.getClickedZ()));

					TileMap.selectDestory();
					TileMap.hintMove ();

					remaincount = TileMap.remainTile();
					


					network.netremoveTile(remaincount,clickX.ToString(), clickY.ToString() , clickZ.ToString()
					                      ,TileMap.getClickedX().ToString()
					                      ,TileMap.getClickedY().ToString()
					                      ,TileMap.getClickedZ().ToString());

				//다른패를 눌렸다면 그 패를 다시 선택한다.
				}else
				{
					//select
					m_TouchedObject = m_hit.collider.gameObject;

					TileMap.ClickTile(clickX , clickY , clickZ);
					TileMap.setClicked(clickX , clickY , clickZ);
					clickTile = tile;
				}
			}
		}
	}

	/*
	 * 맵관련 메소드
	 */

	//상대편 맵생~성
	public void setOpp(string str)
	{

		for(int i = 0 ; i < displaynum.Length ; i++)
		{
			for(int j = 0 ; j < oppuser.Length ; j++)
			{
				
				if(displaynum[i].ToString().Equals(oppuser[j].getusernum()))
				{
					Debug.Log ( "oppuser[0] : " + oppuser[j].getusernum());
					JsonReader reader= new JsonReader(str);

					opp[j] = new Opponent();
					/////////////////////////////////////////////
					opp[j].Opponentmap(reader,i);
					Debug.Log ("crate opp user map : " + oppuser[j].getusernum());
					Debug.Log ("oppuser " + j + " opp " + i);
				}
			}
		}

		/*망한 코드
		int count = 0;
		
		
		for(int i = 0 ; i < oppuser.Length ; i++){
			
			if(oppuser[i].getnickname().Equals(""))
			{

				continue;
			}
			JsonReader reader= new JsonReader(str);
			opp[count] = new Opponent();
			opp[count].Opponentmap(reader,count);
			count++;
		}
		*/
	}

	//자신의맵생~성
	public void setMap(JsonReader obj)
	{
		//게임을 초기화시킨다~
		InitGame();
		AudioSource.PlayClipAtPoint( startsound , transform.position);
		GetComponent<AudioSource>().clip = backgoundsound;
		GetComponent<AudioSource>().loop = true;
		GetComponent<AudioSource>().Play();
		TileMap.mapConverter(obj);

		//맵을 생성하면 게임을 시작한다.
		playerState = PlayState.PLAYGAME;
	}

	public void setupItem(JsonReader obj)
	{
		JsonData  col = JsonMapper.ToObject(obj);

		UserInfo.globalshuffle = System.Convert.ToInt32(col["shuffle"].ToString());
		UserInfo.globalhint = System.Convert.ToInt32(col["hint"].ToString());
	}

	public void oppremoveTile(JsonReader obj , string username)
	{

		string num;

		for(int i = 0 ; i < oppuser.Length ; i++){
			if(oppuser[i].getnickname().Equals(username)){
				JsonData jobjcol = JsonMapper.ToObject(obj);
				for(int j = 0 ; j < 2 ; j++){

					int x = System.Convert.ToInt32(jobjcol[j]["x"].ToString());
					int y = System.Convert.ToInt32(jobjcol[j]["y"].ToString());
					int z = System.Convert.ToInt32(jobjcol[j]["z"].ToString());

					opp[i].oppTileDestory(x,y,z);
					for(int k = 0 ; k < displaynum.Length ; k++)
					{
						if( displaynum[k] == System.Convert.ToInt32(oppuser[i].getusernum()))
						{
							StartCoroutine(DestoryOppTile(x,y,z, k));
							break;
						}
					}
				}
				return;
			}
		}
	}

	//상대가 맵을 셔~플
	public void oppShuffle(JsonReader obj,string username)
	{
		for(int i = 0 ; i < oppuser.Length ; i++){
			if(oppuser[i].getnickname().Equals(username)){
				
				opp[i].oppshuffle(obj);
				return;

			}
		}
	}

	void setdisplaynum(string s)
	{
		int num = System.Convert.ToInt32(s);
		int count =0;
		for(int i = 1 ; i < 6 ; i++ )
		{
			if( i == num){
				continue;
			}
			displaynum[count] = i;
			count++;
		}

		for(int i = 0 ; i < 4 ; i++)
		{
			Debug.Log (displaynum[i]);
		}
	}

	/*
	 * 입장퇴장시 상태관련메소드
	*/

	//방을만들었을 때
	public void stateZzang(JsonReader obj)
	{
		JsonData  col = JsonMapper.ToObject(obj);

		string oppstate;
		string oppname;
		string oppnum;

		for(int i = 0 ; i < col.Count ; i++)
		{
			oppstate =(col[i]["userstate"].ToString()); 
			oppname = (col[i]["user"].ToString());
			oppnum = (col[i]["num"].ToString());

			if(oppname.Equals(UserInfo.globalusername))
			{
				UserInfo.globalusernum = oppnum;
				BigNum[(System.Convert.ToInt32(oppnum)-1)].transform.position = new Vector3(0.5f,3.5f,-5);

				setdisplaynum(oppnum);

				continue;
			}

			oppuser[i].setnickname(oppname);
			oppuser[i].setusernum(oppnum);
			oppuser[i].setuserstate(oppstate);

			Debug.Log (oppuser[i]==null);
		}

		Zzangtag.transform.position = new Vector3 (0.6f, 3.2f, -5);

		Debug.Log ("state : observer -> Zzang");
		playerState = PlayState.ZZANG;
		tempMap[currentSlide].transform.position = new Vector3(3.2f,2,-5);
	}

	public void movezzang(JsonReader obj)
	{

		JsonData  col = JsonMapper.ToObject(obj);
		
		string zzangstate;
		string zzangname;
		string zzangnum;

		zzangstate =(col["userstate"].ToString()); 
		zzangname = (col["user"].ToString());
		zzangnum = (col["num"].ToString());

		for(int i = 0 ; i < oppuser.Length ; i++ )
		{
			if(UserInfo.globalusername.Equals(zzangname))
			{
				playerState = PlayState.ZZANG;
				tempMap[currentSlide].transform.position = new Vector3(3.2f,2,-5);
				Zzangtag.transform.position = new Vector3 (0.6f, 3.2f, -5);
			}

			if(oppuser[i].getnickname().Equals(zzangname))
			{
				//방장이미지보여주기
				for(int j = 0 ; j < displaynum.Length ; j++){
					if(displaynum[j] == System.Convert.ToInt32(oppuser[i].getusernum()))
					{
						Zzangtag.transform.position = new Vector3 (5, 4 -j ,-5);
						break;
					}

				}
			}
		}
	}


	//방에입장했을 때
	public void stateUser(JsonReader obj)
	{
		JsonData  col = JsonMapper.ToObject(obj);

		string oppstate;
		string oppname;
		string oppnum;
		string oppzzangname = "";
		for(int i = 0 ; i < col.Count ; i++)
		{
			oppstate =(col[i]["userstate"].ToString()); 
			oppname = (col[i]["user"].ToString());
			oppnum = (col[i]["num"].ToString());


			if(oppname.Equals(UserInfo.globalusername))
			{
				UserInfo.globalusernum = oppnum;

				BigNum[(System.Convert.ToInt32(oppnum)-1)].transform.position = new Vector3(0.5f,3.5f,-5);
				setdisplaynum(oppnum);
				continue;
			}

			if(oppstate.Equals("zzang")){
				//oppname 번호저장
				oppzzangname = oppname;
			}

			oppuser[i].setnickname(oppname);
			oppuser[i].setusernum(oppnum);
			oppuser[i].setuserstate(oppstate);

		}

		for(int i = 0 ; i < oppuser.Length ; i++ )
		{

			if(oppuser[i].getnickname().Equals(oppzzangname))
			{
				//방장이미지보여주기
				for(int j = 0 ; j < displaynum.Length ; j++){
					if(displaynum[j] == System.Convert.ToInt32(oppuser[i].getusernum()))
					{
						Zzangtag.transform.position = new Vector3 (5, 4 -j ,-5);
						break;
					}
					
				}
			}
		}

		Debug.Log ("state : observer -> User");
		playerState = PlayState.USER;

	}

	//상대가 들어 왔을때
	public void oppUser(JsonReader obj)
	{

		JsonData  col = JsonMapper.ToObject(obj);
		
		string oppstate = col["userstate"].ToString();
		string oppname = col["user"].ToString();
		string oppnum = col["num"].ToString();

		for(int i = 0 ; i < oppuser.Length ; i++){
			if(oppuser[i].getnickname().Equals("") ){

				oppuser[i].setnickname(oppname);
				oppuser[i].setusernum(oppnum);
				oppuser[i].setuserstate(oppstate);
				Debug.Log (oppuser[i]==null);
				break;
			}
		}
	}

	public void oppobserver(JsonReader obj)
	{
		JsonData col = JsonMapper.ToObject(obj);

		string oppstate = col["userstate"].ToString();
		string oppname = col["user"].ToString();
		string oppnum = col["num"].ToString();

		int temp = 0;
		
		for(int i = 0 ; i < displaynum.Length ; i++)
		{
			
			if(displaynum[i].ToString().Equals(oppnum))
			{
				temp = i;
			}
		}

		for(int i = 0 ; i < oppuser.Length ; i++)
		{
			if(oppuser[i].getnickname().Equals("") ){
				oppuser[i].setnickname(oppname);
				oppuser[i].setusernum(oppnum);
				oppuser[i].setuserstate(oppstate);
				break;
			}
		}
	}
	
	public void observermap(JsonReader obj , string str)
	{
		JsonData  col = JsonMapper.ToObject(obj);
		
		string oppstate = col["userstate"].ToString();
		string oppname = col["user"].ToString();
		string oppnum = col["num"].ToString();
		int temp = 0;

		for(int i = 0 ; i < displaynum.Length ; i++)
		{

			if(displaynum[i].ToString().Equals(oppnum))
			{
				temp = i;
			}
		}


		for(int i = 0 ; i < oppuser.Length ; i++)
		{
			if(oppuser[i].getnickname().Equals("") ){
				JsonReader reader= new JsonReader(str);
				oppuser[i].setnickname(oppname);
				oppuser[i].setusernum(oppnum);
				oppuser[i].setuserstate(oppstate);
				opp[i] = new Opponent();
				/////////////////////////////////////////////
				opp[i].Opponentmap(reader,temp);

				break;
			}
		}
	}

	public void deloppUser(JsonReader obj)
	{
		JsonData  col = JsonMapper.ToObject(obj);
		
		string oppstate = col["userstate"].ToString();
		string oppname = col["user"].ToString();
		string oppnum = col["num"].ToString();

		for(int i = 0 ; i < displaynum.Length ; i++)
		{
			if(displaynum[i].ToString().Equals(oppnum))
			{
				BigNum[(displaynum[i]-1)].transform.position = new Vector3(1,1,1);
				userText[i].text = "";
			}
		}

		for(int i = 0 ; i < oppuser.Length ; i++ ){
			if ( oppuser[i].getnickname().Equals(oppname))
			{

				oppuser[i].setnickname("");
				oppuser[i].setusernum("");
				oppuser[i].setuserstate("");

				return;
			}
		}
		//network.sendoutroom();
	}

	//게임결과화면

	public void gameresult(JsonReader obj)
	{
		JsonData  col = JsonMapper.ToObject(obj);

		string user;
		string rank;
		string score;

		AudioSource.PlayClipAtPoint( gameoversound , transform.position);
		GetComponent<AudioSource>().Stop();
		for(int k = 0 ; k < col.Count ; k++)
		{
			user = col[k]["user"].ToString();
			rank = col[k]["rank"].ToString();
			score = col[k]["score"].ToString();

			for(int i = 0 ; i < displaynum.Length ; i++)
			{
				for(int j = 0 ; j < oppuser.Length ; j++)
				{
					if(oppuser[j].getnickname().Equals(user))
					{
						if(displaynum[i].ToString().Equals(oppuser[j].getusernum()))
						{
							Ranking[System.Convert.ToInt32(rank)-1].transform.position = new Vector3(5.5f ,3.8f-(i *1.1f), -6);
						}
					}else if(UserInfo.globalusername.Equals(user))
					{
						Ranking[System.Convert.ToInt32(rank)-1].transform.position = new Vector3(2 ,2, -5);
					}
				}
			}
		}

		TileMap.hintdestroy();

		m_TouchedObject = null;
		TileMap.selectDestory();
		playerState = PlayState.GAMEOVER;

		for(int i = 0 ; i < oppuser.Length ; i++){

			oppuser[i].setnickname("");
			oppuser[i].setusernum("");
			oppuser[i].setuserstate("");

		}
		//옵저버 카메라 위치를 초기화한다.
		observerCamera.transform.position = new Vector3( 1 , 1 , 1);

		network.netReStart();
	}

	public void userGameOver(JsonReader obj)
	{
		JsonData  col = JsonMapper.ToObject(obj);
		string user = col[0]["user"].ToString();
		string rank = col[0]["rank"].ToString();
		string score = col[0]["score"].ToString();

		for(int i = 0 ; i < displaynum.Length ; i++)
		{
			for(int j = 0 ; j < oppuser.Length ; j++)
			{
				if(oppuser[j].getnickname().Equals(user))
				{
					if(displaynum[i].ToString().Equals(oppuser[j].getusernum()))
					{
						Ranking[System.Convert.ToInt32(rank)-1].transform.position = new Vector3(5.5f ,3.8f-(i *1.1f), -6);
					}
				}else if(UserInfo.globalusername.Equals(user))
				{
					Ranking[System.Convert.ToInt32(rank)-1].transform.position = new Vector3(2 ,2, -5);
				}
			}
		}
	}

	public void requestMap(JsonReader obj)
	{
		JsonData  col = JsonMapper.ToObject(obj);
		string userstate = col["userstate"].ToString();
		string user = col["user"].ToString();
		string num = col["num"].ToString();
		//유저가들어왔음

		for(int i = 0 ; i < oppuser.Length ; i++){
			
			if(oppuser[i].getnickname().Equals("") ){
				
				oppuser[i].setnickname(user);
				oppuser[i].setusernum(num);
				oppuser[i].setuserstate(userstate);
				break;
			}
		}

		if(user.Equals(UserInfo.globalusername))
		{
			UserInfo.globalusernum = num;
			setdisplaynum(num);


			//방장을정해준다. 
			return;
		}
		if(playerState == PlayState.OBSERVER)
		{
			network.observerInfo(user);
			return;
		}

		network.observerMap(TileMap.sendmap(),user);
	}


	// GUI
	void OnGUI()
	{
		for(int i = 0 ; i < displaynum.Length ; i++)
		{
			for(int j = 0 ; j < oppuser.Length ; j++)
			{

				if(displaynum[i].ToString().Equals(oppuser[j].getusernum()))
				{
					userText[i].text = oppuser[j].getnickname();
					BigNum[(displaynum[i]-1)].transform.position = new Vector3(5,4-(i*1.1f) ,-5);
				}
			}
		}


		switch(playerState)
		{
		case PlayState.OBSERVER:
			GUI.skin = ExitButton;
			if(GUI.Button( new Rect( 0 , 0 , Screen.width*0.2f , Screen.height/8 ), ""))
			{
				network.sendoutroom();
				Application.LoadLevel("Lobby");
			}
			break;

		case PlayState.USER:
			GUI.skin = ExitButton;
			if(GUI.Button( new Rect( 0 , 0 , Screen.width*0.2f , Screen.height/8 ), ""))
			{
				network.sendoutroom();
				Application.LoadLevel("Lobby");
			}
			break;

		case PlayState.ZZANG:
			GUI.skin = ExitButton;
			if(GUI.Button( new Rect( 0 , 0 , Screen.width*0.2f , Screen.height/8 ), ""))
			{
				network.sendoutroom();
				Application.LoadLevel("Lobby");
			}
			GUI.skin = DefaultButton;
			if(GUI.Button( new Rect( Screen.width*0.4f , (Screen.height*3)/4 , Screen.width/5 , Screen.height/8 ), "start"))
			{
				tempMap[currentSlide].transform.position = new Vector3(1,1,1);
				//맵번호를보낸~다
				network.sendMapNum (currentSlide.ToString());

				playerState = PlayState.USER;
			}
			if(GUI.Button( new Rect( (Screen.width*2)/8 ,(Screen.height*3)/4 , Screen.width/8 , Screen.height/8 ),"<"))
			{
				currentSlide--;
				if(currentSlide < 0 )
				{
					currentSlide = 0;
				}
				tempMap[currentSlide].transform.position = new Vector3(3.2f,2,-5);
				tempMap[currentSlide+1].transform.position = new Vector3(1,1,1);
			}


			if(GUI.Button( new Rect( (Screen.width*5)/8 ,(Screen.height*3)/4 , Screen.width/8 , Screen.height/8 ),">"))
			{
				currentSlide++;
				if(currentSlide > 2)
				{
					currentSlide--;
				}
				tempMap[currentSlide].transform.position = new Vector3(3.2f,2,-5);
				tempMap[currentSlide-1].transform.position = new Vector3(1,1,1);
			}

			break;

		case PlayState.PLAYGAME:
			GUI.skin = ExitButton;
			if(GUI.Button( new Rect( 0 , 0 , Screen.width*0.2f , Screen.height/8 ), ""))
			{

				network.sendoutroom();
				Application.LoadLevel("Lobby");
			}

			GUI.skin = ShuffleButton;
			if(GUI.Button( new Rect( Screen.width*0.2f ,0, Screen.width*0.2f , Screen.height/8 ), UserInfo.globalshuffle.ToString()))
			{
				if(UserInfo.globalshuffle > 0){
					m_TouchedObject = null;
					Shuffle.mapShuffle();
					network.netShuffle(TileMap.sendmap());
					TileMap.hintMove ();
					UserInfo.globalshuffle--;
				}

			}
			GUI.skin = HintButton;
			if(GUI.Button( new Rect( Screen.width*0.4f ,0, Screen.width*0.2f , Screen.height/8 ),UserInfo.globalhint.ToString()))
			{
				if(UserInfo.globalhint > 0){
					TileMap.hint();
					UserInfo.globalhint--;
				}
			}
			removebleText.text = "removable : " + removablecount;

			break;

		case PlayState.GAMEOVER:

			//

			//상태화면을 보여줌
			//점수 증감
			break;
		}
	}

	//어플종료
	void OnApplicationQuit ()
	{

		network.close();
	}


	////////////////////코루틴/////////////////
	/// 
	IEnumerator DestoryOppTile(int x,int y, int z,int usergap){
		
		//생성
		GameObject obj =  Instantiate (Resources.Load ("Prefabs/effectsmall"))as GameObject;
		//위치변경
		obj.transform.position = new Vector3(((float)x/35)+5.4f,((float)y/25)+3.5f-(usergap*1.08f),(-z));
		//크기조절
		
		yield return new WaitForSeconds(0.3f);
		//삭제
		Destroy(obj);
	}

	IEnumerator DestoryPlayerTile(int x,int y, int z)
	{
		GameObject obj =  Instantiate (Resources.Load ("Prefabs/effectbig"))as GameObject;

		float xterm = xgap * (float)x;
		float yterm = ygap * (float)y;
		float zterm = zgap * (float)z;

		obj.transform.position = new Vector3((x/xDivSize)+zterm+xyselectgap,(y/yDivSize)+zterm+xyselectgap,(-z)+xterm+yterm-zselectgap);

		yield return new WaitForSeconds(0.3f);
		Destroy(obj);
	}
}
 