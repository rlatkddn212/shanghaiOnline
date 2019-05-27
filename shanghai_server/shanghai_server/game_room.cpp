#include "stdafx.h"
#include "game_room.h"
using namespace std;


bool GameRoom::addUser(User* user){
	if (!isFull()){
		gameUser.push_back(user);
		return true;
	}
	return false;
}


bool GameRoom::deleteUser(User* user){
	userNumberFlag[user->socketData->usernum] = false;
	gameUser.remove(user);
	return true;
}

bool GameRoom::isEmpty(){
	if (gameUser.empty()){
		return true;
	}
	return false;
}

bool GameRoom::isFull(){
	if (gameUser.size() != MAX_ROOM_USER){
		return false;
	}
	return true;
}

User* GameRoom::getFirstUser(){
	return gameUser.front();
}

int GameRoom::createUserNum(){
	for (int i = 1; i < MAX_ROOM_USER + 1; i++){
		if (!userNumberFlag[i]){
			userNumberFlag[i] = true;
			return i;
		}
	}
	return 0;
}

User* GameRoom::findUser(std::string name){
	for (auto it : gameUser){
		string s = it->socketData->nickName;
		if (s == name){
			return it;
		}
	}
	return nullptr;
}