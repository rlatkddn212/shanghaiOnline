/*
 * 상대편화면 을보여 줌
 * 
*/
using UnityEngine;
using System.Collections;
using LitJson;
using System;
public class Opponent : MonoBehaviour {


	public  float xDivSize = 7;
	public  float yDivSize =5;
	
	public  int xTileSum = 35;
	public  int yTileSum = 20;
	public  int zTileSum = 7;
	
	public  float xgap = 0.001f;
	public  float ygap = 0.001f;
	public  float zgap = 0.06f;

	ArrayList opp = new ArrayList();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Opponentmap(JsonReader obj ,int usergap)
	{

		JsonData  col = JsonMapper.ToObject(obj);
		int Tile;
		int x;
		int y;
		int z;
		for(int i = 0 ; i < col.Count ; i++)
		{
			Tile =Convert.ToInt32(col[i]["Tile"].ToString()); 
			x = Convert.ToInt32(col[i]["x"].ToString());
			y = Convert.ToInt32(col[i]["y"].ToString());
			z = Convert.ToInt32(col[i]["z"].ToString());

			
			float xterm = xgap * (float)x;
			float yterm = ygap * (float)y;
			float zterm = zgap * (float)z;
			//create Tile

			GameObject tempTile = Instantiate (Resources.Load ("opp/BigMblock_"+(Tile-1)))as GameObject;

			Tile setTile = tempTile.GetComponent<Tile>();
			setTile.setpoint(x,y,z);
			setTile.setNum(Tile);

			//move
			tempTile.transform.position = new Vector3(((float)x/35)+5.4f,((float)y/25)+3.5f-(usergap*1.08f),(-z));

			opp.Add (tempTile);
			
		}
	}

	public void oppTileDestory(int x, int y , int z)
	{
		int count= 0;
		int removeindex = 0;

		foreach(GameObject tile in opp)
		{
			Tile tempscript = tile.GetComponent<Tile>();

			int tx= tempscript.getX();
			int ty= tempscript.getY();
			int tz= tempscript.getZ();

			if( x == tx && y == ty&& z == tz)
			{
				Destroy(tile);
				removeindex = count;
			}
			count++;
		}
		opp.RemoveAt(removeindex);

	}

	public void oppshuffle(JsonReader obj)
	{
		JsonData col = JsonMapper.ToObject(obj);
		
		int Tile;
		int x;
		int y;
		int z;
		
		for(int i = 0 ; i < col.Count ; i++)
		{
			Tile =Convert.ToInt32(col[i]["Tile"].ToString()); 
			x = Convert.ToInt32(col[i]["x"].ToString());
			y = Convert.ToInt32(col[i]["y"].ToString());
			z = Convert.ToInt32(col[i]["z"].ToString());

			for(int j = 0 ; j < opp.Count ; j++)
			{
				GameObject temp = (GameObject)opp[j];
				Tile tempscript = temp.GetComponent<Tile>();

				int tx = tempscript.getX();
				int ty = tempscript.getY();
				int tz = tempscript.getZ();

				if( x == tx && y == ty&& z == tz)
				{
					temp.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("image/BigMblock_"+(Tile-1));

				}
			}
		}
	}
	
}
