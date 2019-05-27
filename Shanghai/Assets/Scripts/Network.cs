/*
 * 서버연결 및 서버로 보내는 작업
*/

using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;
using System.IO;
using LitJson;

public class Network:MonoBehaviour{


	private Socket client;
	private byte[] recvBuffer = new byte[1024];
	private Socket sock;
	public Queue myQ;
	private static Network inst = new Network();
	private StreamWriter sw;
	string message;


	public static Network getInstance()
	{
		return inst;
	}

	public void NetworkStart()
	{
		if(sock != null)
		{
			return;
		}
		myQ = new Queue();
		sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

		sock.Connect("127.0.0.1", 8080);
		if(sock.Connected)
		{
			sock.BeginReceive(recvBuffer, 
			                  0, 
			                  recvBuffer.Length, 
			                  SocketFlags.None, 
			                  new AsyncCallback( ReceiveData ), 
			                  sock );
		}
	}

	public void sendData(String s)
	{
		byte[] buffer = new UTF8Encoding().GetBytes(s);
		sock.Send (buffer);
	}

	void Start () 
	{
	}
	void Update () 
	{
	}

	private void ReceiveData(IAsyncResult async)
	{
		Socket temp = (Socket)async.AsyncState;
		int recv = sock.EndReceive(async);

		message = String.Concat(message , new UTF8Encoding().GetString (recvBuffer, 0,recv)) ;
		if(message.EndsWith("\n")){

			myQ.Enqueue(message);
			message = "";
		}
		if (recv>0)	
		{

			sock.BeginReceive(recvBuffer, 
			                  0, 
			                  recvBuffer.Length, 
			                  SocketFlags.None, 
			                  new AsyncCallback( ReceiveData ),
			                  null );
				
				//myQ.Enqueue(message);
				
		}
	}
	public void sendoutroom()
	{
		string data = "outroom/";
		sendData (data);
	}

	public void sendEnterRoom(string s , string num)
	{
		StringBuilder sb = new StringBuilder();
		JsonWriter writer = new JsonWriter(sb);
		
		writer.WriteObjectStart();
		writer.WritePropertyName ( "nickname");
		writer.Write(s);
		writer.WritePropertyName ( "roomnum");
		writer.Write(num);
		writer.WriteObjectEnd();
		
		String Json = "enterroom/"+sb.ToString()+"/";
		
		sendData (Json);
	}
	//가입하기
	public void sendUserName(string s , string cn)
	{
		StringBuilder sb = new StringBuilder();
		JsonWriter writer = new JsonWriter(sb);

		writer.WriteObjectStart();

		writer.WritePropertyName ( "nickname");
		writer.Write(s);
		writer.WritePropertyName ( "cn");
		writer.Write(cn);

		writer.WriteObjectEnd();

		String Json = "username/"+sb.ToString()+"/";
		
		sendData (Json);
	}

	public void sendCreateRoom(string room ,string name , string hint , string shuf)
	{
		StringBuilder sb = new StringBuilder();
		JsonWriter writer = new JsonWriter(sb);
		
		writer.WriteObjectStart();
		
		writer.WritePropertyName ( "roomname");
		writer.Write(room);
		writer.WritePropertyName ( "zzang");
		writer.Write(name);
		writer.WritePropertyName ( "hint");
		writer.Write(hint);
		writer.WritePropertyName ( "shuf");
		writer.Write(shuf);
		writer.WriteObjectEnd();

		String Json = "createroom/"+sb.ToString()+"/";
		
		sendData (Json);
	}

	public void sendMapNum(String s)
	{
		StringBuilder sb = new StringBuilder();
		JsonWriter writer = new JsonWriter(sb);

		writer.WriteObjectStart();
		writer.WritePropertyName ( "Mapnum");
		writer.Write(s);

		writer.WriteObjectEnd();

		String Json = "num/"+sb.ToString()+"/";

		sendData (Json);
	}

	public void sendObserver()
	{
		StringBuilder sb = new StringBuilder();
		JsonWriter writer = new JsonWriter(sb);


		writer.WriteObjectStart();
		writer.WritePropertyName ( "name");
		writer.Write("");
		
		writer.WriteObjectEnd();
		sendData ("observer/"+ writer+"/");
	}

	public void sendCN(string cn)
	{
		StringBuilder sb = new StringBuilder();
		JsonWriter writer = new JsonWriter(sb);

		writer.WriteObjectStart();
		writer.WritePropertyName ("cn");
		writer.Write(cn);
		writer.WriteObjectEnd();
		sendData ("netmarblescn/"+ sb.ToString() +"/");
	}

	public void sendnetmarbleAccountset()
	{
		sendData ("netmarblesAccount/");
	}

	public void netremoveTile( int count, string x, string y, string z,string x1 ,string y1 ,string z1)
	{
		StringBuilder sb = new StringBuilder();
		JsonWriter writer = new JsonWriter(sb);

		writer.WriteArrayStart();
		writer.WriteObjectStart();

		writer.WritePropertyName ( "x");
		writer.Write(x);
		writer.WritePropertyName ( "y");
		writer.Write(y);
		writer.WritePropertyName ( "z");
		writer.Write(z);
		writer.WriteObjectEnd();
		writer.WriteObjectStart();
		writer.WritePropertyName ( "x");
		writer.Write(x1);
		writer.WritePropertyName ( "y");
		writer.Write(y1);
		writer.WritePropertyName ( "z");
		writer.Write(z1);
		writer.WriteObjectEnd();

		writer.WriteArrayEnd();
		String Json = "removeTile/"+count+"/"+sb.ToString()+"/";
		
		sendData (Json);
	}

	public void netShuffle(string s)
	{
		string shuffle = "shuffle/"+s+"/";
		
		sendData (shuffle);
	}

	public void netReStart()
	{
		string data = "restart/";
		
		sendData (data);
	}

	public void netGameOver()
	{

		string data = "gameover/";
		sendData(data);
	}

	public void netGameWin()
	{
		
		string data = "win/";
		sendData(data);
	}

	public void observerMap(string s, string name)
	{
		string sendmap = "observerMap/" + name +"/" + s+"/";
		sendData(sendmap);
	}

	public void observerInfo(string name)
	{
		string sendinfo = "observerInfo/"+name +"/";
		sendData(sendinfo);
	}

	public void close()
	{
		string s = "close/";

		sendData(s);
		sock.Close ();
		sock =null;
	}
}