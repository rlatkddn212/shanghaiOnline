 /*
 *서버로부터 받은 데이터를 사용함
 * 
*/

using UnityEngine;
using System.Collections;
using LitJson;

public class Parser : MonoBehaviour {

	JsonReader reader;

	GameObject hint2;
	public void StringParser(string s)
	{
		GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();

		string[] stringn = s.Split('\n');
		for(int i = 0; i < stringn.Length ; i++)
		{
			Debug.Log (stringn[i]);
			string[] stringr = stringn[i].Split('/');


			///////////////////////////ingame/////////////////////////
			if( stringr[0].Equals("map") )
			{

			//	TileMap.mapConverter(parser.Parse(stringr[1]));
			//
				reader= new JsonReader(stringr[1]);
				JsonReader reader2= new JsonReader(stringr[1]);
				gm.setMap(reader);

				gm.setOpp(stringr[1]);
			}

			if(stringr[0].Equals("item"))
			{
				reader = new JsonReader (stringr[1]);
				gm.setupItem(reader);
			}

			if( stringr[0].Equals("opp") )
			{
				reader= new JsonReader(stringr[2]);
				gm.oppremoveTile(reader,stringr[1]);
			}

			if( stringr[0].Equals("oppshuffle") )
			{
				reader= new JsonReader(stringr[2]);
				gm.oppShuffle(reader,stringr[1]);
			}

			if(stringr[0].Equals("zzangs"))
			{

				reader = new JsonReader(stringr[1]);
				gm.stateZzang(reader);
			}

			if(stringr[0].Equals("users"))
			{

				reader = new JsonReader(stringr[1]);
				gm.stateUser(reader);
			}

			if(stringr[0].Equals("oppinfo"))
			{
				reader = new JsonReader(stringr[1]);
				gm.oppUser(reader);
			}

			if(stringr[0].Equals("delroomuser"))
			{
				reader = new JsonReader(stringr[1]);
				gm.deloppUser(reader);
			}

			if(stringr[0].Equals("final"))
			{
				
				reader = new JsonReader(stringr[1]);
				gm.gameresult(reader);
			}
			if(stringr[0].Equals("usergameover"))
			{
				
				reader = new JsonReader(stringr[1]);
				gm.userGameOver(reader);
			}
			if(stringr[0].Equals("movezzang"))
			{
				reader = new JsonReader(stringr[1]);
				gm.movezzang(reader);
			}
			//서버가옵저버맵을 요청한 메시지
			if(stringr[0].Equals("requestMap"))
			{
				reader = new JsonReader(stringr[1]);
				gm.requestMap(reader);
			}
			//
			if(stringr[0].Equals("sendobservermap"))
			{
				reader = new JsonReader(stringr[1]);

				gm.observermap(reader,stringr[2]);
			}
			if(stringr[0].Equals("sendobserverInfo"))
			{
				reader = new JsonReader(stringr[1]);
				gm.oppobserver(reader);
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
