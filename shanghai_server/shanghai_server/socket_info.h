#pragma once
#include "stdafx.h"
#include "type.h"

enum USERSTATE{
	WAITROOM, OBSERVER, USER, CHIEF, GAMEOVER
};

struct SOCKET_DATA{

	SOCKET		clientSocket;
	SOCKADDR_IN	clientAddrInfo;
	Char nickName[32];
	int nameLen = 0;
	int titlenum = 0;
	int usernum;
	int win;
	int lose;
	int score;
	int tilenum = 0;
	USERSTATE state;
};

struct IO_DATA{
	OVERLAPPED	overlapped;
	WSABUF		wsaBuf;
	Char		buffer[BUF_SIZE];
	int			readWriteMode;
};