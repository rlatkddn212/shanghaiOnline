/*
 *서버로부터 받은 데이터를 사용함 (lobby)
 * 
*/

using UnityEngine;
using System.Collections;
using LitJson;

public class LobbyParser : MonoBehaviour {
	
	JsonReader reader;

	public void StringParser(string s)
	{
		string[] stringn = s.Split('\n');
		for(int i = 0; i < stringn.Length ; i++)
		{
			Debug.Log (stringn[i]);
			string[] stringr = stringn[i].Split('/');

			Lobby lobby = GameObject.Find("lobby Object").GetComponent<Lobby>();
			


			if( stringr[0].Equals("enter") )
			{
				reader= new JsonReader(stringr[1]);
				lobby.startgame();
			}

			if(stringr[0].Equals("roomlist") )
			{		//리스트초기화
				
				GameObject[] removeRooms = GameObject.FindGameObjectsWithTag("room");
				
				foreach (GameObject removeRoom in removeRooms)
				{
					NGUITools.Destroy(removeRoom);
				}
				
				if(stringr[1].Equals("null")){
					Debug.Log("여기서 멈춘거얌");
					return;
				}
				reader= new JsonReader(stringr[1]);
				lobby.updateRoomList(reader);
			}

			if(stringr[0].Equals("userinfo") )
			{
				reader= new JsonReader(stringr[1]);
				lobby.userInfo(reader);
			}
		}
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
