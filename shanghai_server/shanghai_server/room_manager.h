#pragma once
#include "stdafx.h"

class RoomManager:public Singleton<RoomManager>{
private:
	bool gameRoomFlag[1000];
public:
	
	std::hash_map<std::string, User*> userList;
	std::hash_map<RoomNum, GameRoom*> roomList;
	
	RoomManager(){
		memset(gameRoomFlag, false, sizeof(gameRoomFlag));
	}
	User* findUser(const std::string& id);
	bool insertUserAtList(const std::string& id, User* user);
	bool deleteUserAtList(const std::string& id);

	GameRoom* makeGameRoom(std::string chiefUser, const RoomNum roomNum, int hint, int shuffle, std::string roomName);
	bool deleteGameRoom(const RoomNum roomNum);
	GameRoom* findGameRoom(const RoomNum roomNum);
	int craeteRoomNumber();
};
