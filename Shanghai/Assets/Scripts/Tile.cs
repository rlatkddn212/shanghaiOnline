using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	private int m_x;
	private int m_y;
	private int m_z;
	private int cardNum;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	}


	public void setpoint(int x, int y ,int z)
	{
		this.m_x = x;
		this.m_y = y;
		this.m_z = z;
	}
	public int getX(){
		return m_x;
	}
	public int getY(){
		return m_y;
	}
	public int getZ(){
		return m_z;
	}

	public void setNum(int num)
	{
		cardNum = num;
	}

	public int getNum()
	{
		return cardNum;
	}

}
