#include "stdafx.h"
#include "network.h"
using namespace std;

bool Network::startServer(){
	WSADATA wsaData;
	HANDLE comPort;
	SYSTEM_INFO sysInfo;

	if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0){
		printf("WSAStartup error\n");
	}
	GetSystemInfo(&sysInfo);

	//코어 개수만큼 IOCP 쓰레드를 생성한다.
	comPort = CreateIoCompletionPort(INVALID_HANDLE_VALUE, NULL, 0, sysInfo.dwNumberOfProcessors);
	int processcount = sysInfo.dwNumberOfProcessors;
	for (int i = 0; i < processcount ; i++){
		_beginthreadex(NULL, 0, workThread, (LPVOID)comPort, 0, NULL);
	}

	//서버 소켓 생성
	serverSocket = WSASocket(AF_INET, SOCK_STREAM, 0, NULL, 0, WSA_FLAG_OVERLAPPED);
	//서버 주소 정보 구조체 초기화
	memset(&serverAddr, 0, sizeof(serverAddr));
	serverAddr.sin_family = AF_INET;
	serverAddr.sin_addr.s_addr = htonl(INADDR_ANY);
	serverAddr.sin_port = htons(8080);

	bind(serverSocket, (SOCKADDR*)&serverAddr, sizeof(serverAddr));
	listen(serverSocket, 5);

	printf("server start\n");

	acceptThread(comPort);

	return true;
}

unsigned int WINAPI Network::workThread(LPVOID completionPortIo){
	
	HANDLE comPort = (HANDLE)completionPortIo;


	while (true){
		SOCKET clientSocket;
		DWORD bytesTrans;
		SOCKET_DATA* socketInfo = nullptr;
		IO_DATA* ioInfo = nullptr;
		DWORD flag = 0;

		GetQueuedCompletionStatus(comPort, &bytesTrans, (LPDWORD)&socketInfo,
			(LPOVERLAPPED*)&ioInfo, INFINITE);
		clientSocket = socketInfo->clientSocket;

		if (ioInfo->readWriteMode == READ){
			puts("message received! \n");
			//EOF전송
			if (bytesTrans == 0){
				Router router;
				router.endMessage(clientSocket, socketInfo, ioInfo);
				closesocket(socketInfo->clientSocket);
				continue;
			}
			memset(&(ioInfo->overlapped), 0, sizeof(OVERLAPPED));
			ioInfo->readWriteMode = WRITE;
			ioInfo->wsaBuf.len = bytesTrans;
			ioInfo->wsaBuf.buf[bytesTrans] = '\0';
			flag = 0;
			
			Router router;
			router.parseMessage(clientSocket, socketInfo, ioInfo);
			
			memset(&(ioInfo->overlapped), 0, sizeof(OVERLAPPED));
			
			ZeroMemory(ioInfo->buffer, sizeof(ioInfo->buffer));

			ioInfo = new IO_DATA;
			
			memset(&(ioInfo->overlapped), 0, sizeof(OVERLAPPED));
			ioInfo->wsaBuf.len = BUF_SIZE;
			ioInfo->wsaBuf.buf = ioInfo->buffer;
			ioInfo->readWriteMode = READ;

			WSARecv(clientSocket, &(ioInfo->wsaBuf), 1, NULL, &flag,
				&(ioInfo->overlapped), NULL);
		}
	}
	return 0;
}

bool Network::acceptThread(HANDLE comPort){

	IO_DATA* ioInfo;
	SOCKET_DATA* socketInfo;
	DWORD recvBytes;
	DWORD flag = 0;
	while (true){

		SOCKET clientSocket;
		SOCKADDR_IN clientAddr;
		int addrLen = sizeof(clientAddr);

		clientSocket = accept(serverSocket, (SOCKADDR*)&clientAddr, &addrLen);
		//printf("accept\n");
		
		if (clientSocket == 0){
			cout << "accept error" << endl;
			continue;
		}

		socketInfo = new SOCKET_DATA;
		memset(socketInfo, 0, sizeof(socketInfo));
		socketInfo->clientSocket = clientSocket;
		memcpy(&(socketInfo->clientAddrInfo), &clientAddr, addrLen);

		CreateIoCompletionPort((HANDLE)clientSocket, comPort, (DWORD)socketInfo, 0);

		ioInfo = new IO_DATA;
		memset(&(ioInfo->overlapped), 0, sizeof(OVERLAPPED));
		ioInfo->wsaBuf.len = BUF_SIZE;
		ioInfo->wsaBuf.buf = ioInfo->buffer;
		ioInfo->readWriteMode = READ;

		WSARecv(socketInfo->clientSocket, &(ioInfo->wsaBuf),
			1, &recvBytes, &flag, &(ioInfo->overlapped), NULL);
	}
}
