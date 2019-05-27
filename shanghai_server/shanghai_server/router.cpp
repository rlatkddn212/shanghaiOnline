#include "stdafx.h"
#include "router.h"
using namespace std;
void Router::parseMessage(SOCKET clientSocket,  SOCKET_DATA* socketInfo, IO_DATA* ioInfo){
	

	string s = ioInfo->wsaBuf.buf;

	//cout << "메시지의 내용은 : " + s <<endl;
	
	vector<string> parser = split(s, '/');
	
	Process p(clientSocket, socketInfo, ioInfo);
	
	if (parser[0] == "username"){
		p.processUserName(parser[1]);
	}
	else if (parser[0] == "enterroom"){
		p.processEnterRoom(parser[1]);
	}
	else if (parser[0] == "createroom"){
		p.processCreateRoom(parser[1]);
	}
	else if (parser[0] == "num"){
		p.processSelectedMap(parser[1]);
	}
	else if (parser[0] == "removeTile"){
		p.processRemoveTile(parser[1],parser[2]);
	}
	else if (parser[0] == "shuffle"){
		p.processShuffle(parser[1]);
	}
	else if (parser[0] == "close"){
		p.processClose(parser[1]);
	}
	else if (parser[0] == "win"){
		p.processsWin(parser[1]);
	}
	else if (parser[0] == "gameover"){
		p.processsGameOver(parser[1]);
	}
	else if (parser[0] == "user"){
		p.processsUser(parser[1]);
	}
	else if (parser[0] == "observer"){
		p.processsObserver(parser[1]);
	}
	else if (parser[0] == "restart"){
		p.processsRestart(parser[1]);
	}
	else if (parser[0] == "outroom"){
		p.processsOutRoom(parser[1]);
	}
	else if (parser[0] == "observerMap"){
		p.processsObserverMap(parser[1], parser[2]);
	}
	else if (parser[0] == "observerInfo"){
		p.processsobserverInfo(parser[1]);
	}
}

void Router::endMessage(SOCKET clientSocket, SOCKET_DATA* socketInfo, IO_DATA* ioInfo){
	//소켓을 끝낸다.
	Process p(clientSocket, socketInfo, ioInfo);
	p.exitUser();

}
