#include "stdafx.h"
#include "room_manager.h"
using namespace std;

//로비 관련

User* RoomManager::findUser(const string& id){
	auto it = userList.find(id);
	return it->second;
}

bool RoomManager::insertUserAtList(const string& id, User* user){
	userList.insert(make_pair(id, user));
	return true;
}

bool RoomManager::deleteUserAtList(const string& id){
	auto it = userList.find(id);
	User* user = it->second;
	delete(user->ioData);
	delete(user->socketData);
	delete(user);
	user = nullptr;
	userList.erase(id);
	return true;
}

//방 관련
GameRoom* RoomManager::makeGameRoom(string chiefUser, const RoomNum roomNum, int hint, int shuffle, std::string roomName){
	GameRoom* gameRoom = new GameRoom(chiefUser,roomNum, hint, shuffle, roomName);
	roomList.insert(make_pair(roomNum, gameRoom));
	gameRoom->roomState = GameRoom::WAIT;
	return gameRoom;
}

bool RoomManager::deleteGameRoom(const RoomNum roomNum){
	auto it = roomList.find(roomNum);
	gameRoomFlag[roomNum] = false;
	GameRoom* removeRoom = it->second;
	delete(removeRoom);
	removeRoom = nullptr;
	roomList.erase(roomNum);
	return true;
}

GameRoom* RoomManager::findGameRoom(const RoomNum roomNum){
	auto it = roomList.find(roomNum);
	return it->second;
}

int RoomManager::craeteRoomNumber(){
	for (int i = 1; i < 1000; i++){
		if (!gameRoomFlag[i]){
			gameRoomFlag[i] = true;
			return i;
		}
	}
	return 0;
}
