using UnityEngine;
using System.Collections;

public class OppuserInfo : MonoBehaviour {

	string nickname ="";
	string usernum ="";
	string userstate ="";
	int oppremaintile = 0;

	public void setnickname(string name){
		nickname = name;
	}
	public string getnickname(){
		return nickname;
	}
	public void setusernum(string num){
		usernum = num;
	}
	public string getusernum(){
		return usernum;
	}

	public void setuserstate(string state){
		userstate = state;
	}
	public string getuserstate(){
		return userstate;
	}
}
