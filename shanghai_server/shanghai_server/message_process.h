#pragma once
#include "stdafx.h"

class Process{
	SOCKET clientSocket;
	SOCKET_DATA* socketInfo;
	IO_DATA* ioInfo;
	RoomManager& roomManager;
	ShanghaiMap& shanghaiMap;

public:

	Process(const SOCKET cs, SOCKET_DATA* sd, IO_DATA* iod) :clientSocket(cs),
		socketInfo(sd),
		ioInfo(iod),
		roomManager(RoomManager::getInstance()),
		shanghaiMap(ShanghaiMap::getInstance()){
	}
	void processUserName(const std::string& sParam);
	void processEnterRoom(const std::string& sParam);
	void processCreateRoom(const std::string& parser);
	void processSelectedMap(const std::string& parser);
	void processRemoveTile(const std::string& parser1, const std::string& parser2);
	void processShuffle(const std::string& parser);
	void processClose(const std::string& parser);
	void processsWin(const std::string& parser);
	void processsGameOver(const std::string& parser);
	void processsUser(const std::string& parser);
	void processsObserver(const std::string& parser);
	void processsRestart(const std::string& parser);
	void processsOutRoom(const std::string& parser);
	void processsObserverMap(const std::string& parser1, const std::string& parser2);
	void processsobserverInfo(const std::string& parser);

	void sendRoomList();
	void sendStringData(const std::string& senddata, SOCKET cSocket, IO_DATA* userIo);
	const bool isGameState(int titlenum);
	void broadCastRoom(int userRoomNum, const std::string& senddata);
	void oppBroadCastRoom(int userRoomNum, const std::string& senddata);
	void gameOverUser();
	void outRoomUser();
	void exitUser();
	void outGameRoomUser();
	void outLobby();
};