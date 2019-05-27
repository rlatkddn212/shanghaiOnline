#pragma once
#include "stdafx.h"

class Network :public Singleton<Network>{

private:
	SOCKET serverSocket;
	SOCKADDR_IN serverAddr;
public:
	Network(){
	}
	~Network(){
	}
	bool startServer();

private:
	static unsigned int WINAPI workThread(LPVOID completionPortIo);
	bool acceptThread(HANDLE comPort);
};