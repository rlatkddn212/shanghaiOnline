#pragma once
#include "stdafx.h"
class GameRoom{
private:
	const static int MAX_ROOM_USER = 5;
	const static int MAX_NAME_SIZE = 32;
	bool userNumberFlag[MAX_ROOM_USER+1];
	


public:
	enum ROOMSTATE {
		WAIT, GAME
	};

	std::string chiefName;
	int roomnum;
	int hint;
	int shuffle;
	std::string roomName;
	std::list<User*> gameUser;
	ROOMSTATE roomState;

	GameRoom(std::string& chief, int rNum, int ht, int shuf,std::string& rName): 
		chiefName(chief),
		roomnum(rNum),
		hint(ht),
		shuffle(shuf),
		roomName(rName){
		memset(userNumberFlag, false, sizeof(userNumberFlag));
	}

	int createUserNum();
	bool addUser(User* user);
	bool deleteUser(User* user);
	bool isEmpty();
	bool isFull();
	User* getFirstUser();
	User* findUser(std::string name);
	// todo : 방장 변경
};