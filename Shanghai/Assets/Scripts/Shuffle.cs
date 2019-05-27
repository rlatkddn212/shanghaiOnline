using UnityEngine;
using System.Collections;

public class Shuffle : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void mapShuffle()
	{

		int TileNumTemp;
		int tileX;
		int tileY;
		int tileZ;
		GameObject[] temps = GameObject.FindGameObjectsWithTag ("Tile");
		TileMap.setClicked(0,0,0);
		TileMap.selectDestory();
		for (int i = 0; i < temps.Length; i++)
		{

			int random = Random.Range(0, temps.Length);
			Vector3 temp = new Vector3();
			//arr position

			Tile tile1 = temps[i].GetComponent<Tile>();
			Tile tile2 = temps[random].GetComponent<Tile>();

			//swapp!!!
			TileNumTemp = TileMap.m_MapArr[tile1.getX(),tile1.getY(),tile1.getZ()];
			TileMap.m_MapArr[tile1.getX(),tile1.getY(),tile1.getZ()]= TileMap.m_MapArr[tile2.getX(),tile2.getY(),tile2.getZ()];
			TileMap.m_MapArr[tile2.getX(),tile2.getY(),tile2.getZ()] = TileNumTemp;
			tileX = tile1.getX();
			tileY = tile1.getY();
			tileZ = tile1.getZ();
			tile1.setpoint(tile2.getX(),tile2.getY(),tile2.getZ());
			tile2.setpoint(tileX,tileY,tileZ);

			//Tile position 
			temp.x = temps[i].transform.position.x;
			temp.y = temps[i].transform.position.y;
			temp.z = temps[i].transform.position.z;
			
			temps[i].transform.position = temps[random].transform.position;
			temps[random].transform.position = temp;


		}

	}
}
