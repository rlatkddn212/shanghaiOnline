using UnityEngine;
using System.Collections;
using System.IO; 
using System.Text;
using System.Collections.Generic;
using LitJson;

public class TileMap : MonoBehaviour {

	private static GameObject m_Select = null;

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

	public static int Tilenums = 36;

	public static int tilecount = 4;
	//맵데이 터저장
	public static int[,,] m_MapArr;
	//제거가능한 애들 저장
	public static ArrayList removeTile = new ArrayList();
	public static ArrayList hintTile = new ArrayList();
	public static ArrayList hintTile2 = new ArrayList();
	private static int clickedX;
	private static int clickedY;
	private static int clickedZ;
	private static GameObject hint1;
	private static GameObject hint2;
	private static Tile touchPossbleTile;
	// Use this for initialization
	void Start () {
		/* test map
		 * 


		FileInfo mapFile = null;
		TextReader reader = null;
		
		//text Load
		mapFile = new FileInfo (Application.dataPath + "test.txt");
		if ( mapFile != null && mapFile.Exists )
		{
			reader = mapFile.OpenText();
		}
		else
		{
			TextAsset mapText = (TextAsset)Resources.Load("test", typeof(TextAsset));
			reader = new StringReader(mapText.text);
		}
		if ( reader == null )
		{
			Debug.Log("error");
		}
		else
		{
			string txt = reader.ReadToEnd();
			reader.Close();
			mapConverter (txt);
		}
		*/

	}
	
	// Update is called once per frame
	void Update () {

	}

	public static void init()
	{

	}

	//string - > Tile Object
	public static void mapConverter(JsonReader obj)
	{
		hint1 = Instantiate(Resources.Load("Prefabs/hint"))as GameObject;
		hint2 = Instantiate(Resources.Load("Prefabs/hint"))as GameObject;
		hint1.transform.position = new Vector3(1,1,1);
		hint2.transform.position = new Vector3(1,1,1);


		m_MapArr = new int[xTileSum,yTileSum,zTileSum];

		JsonData col = JsonMapper.ToObject(obj);
		int Tile;
		int x;
		int y;
		int z;

	//[{"Tile" : 0 , "x" : 1 , "y":2 ,"z" : 1 },{"Tile" : 0 , "x" : 1 , "y":2 ,"z" : 1 },{"Tile" : 0 , "x" : 1 , "y":2 ,"z" : 1 }]


		//hint2 = Instantiate(Resources.Load("Prefabs/hint"))as GameObject;
		//hint2.transform.position = new Vector3(3.5f,2.5f, -5);

		for(int i = 0 ; i < col.Count ; i++)
		{
			Tile =System.Convert.ToInt32(col[i]["Tile"].ToString()); 
			x = System.Convert.ToInt32(col[i]["x"].ToString());
			y = System.Convert.ToInt32(col[i]["y"].ToString());
			z = System.Convert.ToInt32(col[i]["z"].ToString());

			m_MapArr[x,y,z] = Tile;

			float xterm = xgap * (float)x;
			float yterm = ygap * (float)y;
			float zterm = zgap * (float)z;
			//create Tile
			GameObject tempTile = Instantiate (Resources.Load ("Prefabs/BigMblock_"+(Tile-1)))as GameObject;
			
			//setproperty
			Tile setTile = tempTile.GetComponent<Tile>();
			setTile.setpoint(x,y,z);
			
			//move
			tempTile.transform.position = new Vector3((x/xDivSize)+zterm,(y/yDivSize)+zterm,(-z)+xterm+yterm);

		}
		//shuffle
		//Shuffle.mapShuffle();
	}

	public static int removableTile()
	{
		int count =0;
		removeTile.Clear();
		for(int x = 0 ; x < xTileSum-1 ; x++ )
		{
			for(int y = 0 ; y < yTileSum-1 ; y++)
			{
				for ( int z = 0 ; z < zTileSum-1 ; z++)
				{
					if(isTouchPossible(x,y,z))
					{
						removeTile.Add(m_MapArr[x,y,z]);
					}
				}
			}
		}

		for(int i = 0 ; i < Tilenums ; i++)
		{
			int sum = 0;
			for(int j = 0 ; j < removeTile.Count ; j++ )
			{
				if(removeTile[j].Equals(i))
				{

					sum++;
					if(sum>1)
					{
						count++;
					}
				}
			}
			sum = 0;
		}
		return count;
	}

	public static int  remainTile()
	{
		int count = 0;
		for(int x = 0 ; x < xTileSum-1 ; x++ )
		{
			for(int y = 0 ; y < yTileSum-1 ; y++)
			{
				for ( int z = 0 ; z < zTileSum-1 ; z++)
				{
					if(m_MapArr[x,y,z]!=0){
						count++;
					}
				}
			}
		}
		Debug.Log("remain : " + count);
		return count;
	}

	public static bool isTouchPossible(int pointx, int pointy, int pointz)
	{
		if(m_MapArr [pointx, pointy, pointz] == 0)
		{
			return false;
		}
		if (m_MapArr [pointx - 2, pointy, pointz] != 0 
				|| m_MapArr [pointx - 2, pointy + 1, pointz] != 0
				|| m_MapArr [pointx - 2, pointy - 1, pointz] != 0) {
				//왼쪽이막혔을경 우
				if (m_MapArr [pointx + 2, pointy, pointz] != 0 
						|| m_MapArr [pointx + 2, pointy + 1, pointz] != 0
						|| m_MapArr [pointx + 2, pointy - 1, pointz] != 0) {
						return false;
				}
		} else if (m_MapArr [pointx + 2, pointy, pointz] != 0 
				|| m_MapArr [pointx + 2, pointy + 1, pointz] != 0
				|| m_MapArr [pointx + 2, pointy - 1, pointz] != 0) {
				//오른쪽이막혔을경 우
				if (m_MapArr [pointx - 2, pointy, pointz] != 0 
						|| m_MapArr [pointx - 2, pointy + 1, pointz] != 0
						|| m_MapArr [pointx - 2, pointy - 1, pointz] != 0) {
						return false;
				}
		}
		if (m_MapArr [pointx, pointy, pointz + 1] != 0
				   ||m_MapArr [pointx + 1, pointy - 1, pointz + 1] != 0
		           || m_MapArr [pointx + 1, pointy, pointz + 1] != 0
		           || m_MapArr [pointx + 1, pointy + 1, pointz + 1] != 0
		           || m_MapArr [pointx - 1, pointy - 1, pointz + 1] != 0
		           || m_MapArr [pointx - 1, pointy, pointz + 1] != 0
		           || m_MapArr [pointx - 1, pointy + 1, pointz + 1] != 0
		           || m_MapArr [pointx, pointy - 1, pointz + 1] != 0
		           || m_MapArr [pointx, pointy + 1, pointz + 1] != 0) {
			return false;
		}

		return true;
	}

	public static void ClickTile(int x, int y , int z)
	{
		float xterm = xgap * (float)x;
		float yterm = ygap * (float)y;
		float zterm = zgap * (float)z;
		if(m_Select == null)
		{
			m_Select =Instantiate (Resources.Load ("select"))as GameObject;
		}
		m_Select.transform.position = new Vector3((x/xDivSize)+zterm+xyselectgap,(y/yDivSize)+zterm+xyselectgap,(-z)+xterm+yterm-zselectgap);

	}

	public static void selectDestory()
	{
		Destroy(m_Select);
	}

	public static void removeArr(int x, int y, int z)
	{
		m_MapArr [x, y, z] = 0;
	}

	public static void setClicked(int x, int y ,int z)
	{
		clickedX = x;
		clickedY = y;
		clickedZ = z;
	}



	public static int getClickedX()
	{
		return clickedX;		
	}

	public static int getClickedY()
	{
		return clickedY;
	}

	public static int getClickedZ()
	{
		return clickedZ;
	}

	public static void hintMove ()
	{
		hint1.transform.position = new Vector3(1,1,1);
		hint2.transform.position = new Vector3(1,1,1);
	}
	public static void hintdestroy()
	{
		Destroy(hint1);
		Destroy(hint2);
	}
	public static void hint ()
	{
		

		hintTile.Clear();
		hintTile2.Clear();

		for(int x = 0 ; x < xTileSum-1 ; x++ )
		{
			for(int y = 0 ; y < yTileSum-1 ; y++)
			{
				for ( int z = 0 ; z < zTileSum-1 ; z++)
				{
					if(isTouchPossible(x,y,z))
					{
						touchPossbleTile = new Tile();
						touchPossbleTile.setpoint(x,y,z);
						touchPossbleTile.setNum(m_MapArr[x,y,z]);
						hintTile.Add(touchPossbleTile);
					}
				}
			}
		}

		int sum = 0;
		Tile temp;
		for(int i = 0 ; i < Tilenums ; i++)
		{
			for(int j = 0 ; j < hintTile.Count ; j++ )
			{
				Tile t = hintTile[j] as Tile;
				int a =t.getNum();

				if(a.Equals(i))
				{

					sum++;
					if(sum>1)
					{
						hintTile2.Add(hintTile[j]);
					}
				}
			}
			sum = 0;
		}
		int random = UnityEngine.Random.Range(0, hintTile2.Count);
		if(hintTile2.Count ==0 )
		{
			return;
		}
		Tile tile2  = hintTile2[random] as Tile;

		for(int i = 0 ; i < hintTile.Count ; i++)
		{
			Tile tile1  = hintTile[i] as Tile;

			if( tile1.getNum() == tile2.getNum() 
			   && (tile1.getX()!=tile2.getX()
			   ||tile1.getY()!=tile2.getY()
			   ||tile1.getZ()!=tile2.getZ()))
			{

				Debug.Log (tile1.getNum());
				int x1 = tile1.getX();
				int y1 = tile1.getY();
				int z1 = tile1.getZ();
				Debug.Log (tile2.getNum());
				int x2 = tile2.getX();
				int y2 = tile2.getY();
				int z2 = tile2.getZ();
				float xterm1 = xgap * (float)x1;
				float yterm1 = ygap * (float)y1;
				float zterm1 = zgap * (float)z1;
				float xterm2 = xgap * (float)x2;
				float yterm2 = ygap * (float)y2;
				float zterm2 = zgap * (float)z2;

				Debug.Log (tile2.getNum());
				hint1.transform.position = new Vector3((x1/xDivSize)+zterm1+xyselectgap,(y1/yDivSize)+zterm1+xyselectgap,(-z1)+xterm1+yterm1-zselectgap);
				hint2.transform.position = new Vector3((x2/xDivSize)+zterm2+xyselectgap,(y2/yDivSize)+zterm2+xyselectgap,(-z2)+xterm2+yterm2-zselectgap);
			}
		}
	}

	public static string sendmap()
	{
		StringBuilder sb = new StringBuilder ();
		JsonWriter writer = new JsonWriter (sb);

		writer.WriteArrayStart ();

		for(int x = 0 ; x < xTileSum-1 ; x++ )
		{
			for(int y = 0 ; y < yTileSum-1 ; y++)
			{
				for ( int z = 0 ; z < zTileSum-1 ; z++)
				{
					if(m_MapArr[x,y,z]!=0){

						writer.WriteObjectStart ();

						writer.WritePropertyName ("Tile");
						writer.Write (m_MapArr[x,y,z].ToString());
						writer.WritePropertyName ("x");
						writer.Write (x.ToString());
						writer.WritePropertyName ("y");
						writer.Write (y.ToString());
						writer.WritePropertyName ("z");
						writer.Write (z.ToString());
						writer.WriteObjectEnd ();


					}
				}
			}
		}
		writer.WriteArrayEnd();

		return sb+"";
	}
}




