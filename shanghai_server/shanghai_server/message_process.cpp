#include "stdafx.h"
#include "message_process.h"

using namespace std;

void Process::processUserName(const string& sParam){
	//process로 보낸다.
	Json::Value root;
	Json::Reader reader;
	bool parsingSuccessful = reader.parse(sParam, root);
	if (!parsingSuccessful){
		cout << "parser ERROR!" << endl;
	}
	string nickname = root.get("nickname", "").asString();
	string cn = root.get("cn", "").asString();
	strcpy_s(socketInfo->nickName, nickname.c_str());
	socketInfo->nameLen = nickname.size();
	//유저 등록

	User* user = new User();
	user->socketData = socketInfo;
	user->ioData = ioInfo;

	roomManager.insertUserAtList(nickname, user);
	
	user->socketData->state = USERSTATE::WAITROOM;

	Json::Value obj;

	obj["id"] = nickname;
	obj["win"] = 0;
	obj["lose"] = 0;
	obj["score"] = 0;
	
	Json::FastWriter writer;
	string outputConfig = writer.write(obj);
	string senddata = "userinfo/" + outputConfig;

	sendStringData(senddata, clientSocket, ioInfo);
	sendRoomList();
}

void Process::processEnterRoom(const string& parser){
	cout << "방 입장 하기" << endl;

	Json::Value root;
	Json::Reader reader;
	bool parsingSuccessful = reader.parse(parser, root);
	if (!parsingSuccessful){
		cout << "parser ERROR!" << endl;
	}

	string enterUser = root.get("nickname", "").asString();
	string enterNum = root.get("roomnum", "").asString();
	
	int titlenum = atoi(enterNum.c_str());

	User* user = new User();
	user->ioData = ioInfo;
	user->socketData = socketInfo;

	socketInfo->state = USERSTATE::OBSERVER;
	socketInfo->titlenum = titlenum;

	auto roomInfo = roomManager.roomList.find(titlenum);
	socketInfo->usernum = roomInfo->second->createUserNum();
	
	roomInfo->second->gameUser.push_back(user);

	string senddata = "enter/\n";
	sendStringData(senddata, clientSocket, ioInfo);
	sendRoomList();
}

void Process::processCreateRoom(const string& parser){
	cout << "방만들기 " << endl;
	int roomNum = roomManager.craeteRoomNumber();

	Json::Value root;
	Json::Reader reader;
	bool parsingSuccessful = reader.parse(parser, root);
	if (!parsingSuccessful){
		cout << "parser ERROR!" << endl;
	}
	string roomname = root.get("roomname", "").asString();
	string chiefUser = root.get("zzang", "").asString();
	//int hint = root.get("hint", "").asInt();
	//int shuffle = root.get("shuf", "").asInt();
	int hint = 10;
	int shuffle = 10;
	GameRoom* gameRoom = roomManager.makeGameRoom(chiefUser, roomNum, hint, shuffle, roomname);

	socketInfo->state = USERSTATE::CHIEF;
	socketInfo->titlenum = roomNum;
	socketInfo->usernum = gameRoom->createUserNum();
	User* user = new User();
	user->socketData = socketInfo;
	user->ioData = ioInfo;
	gameRoom->addUser(user);

	//보낼 데이터 생성
	string senddata = "enter/\n";

	sendStringData(senddata, clientSocket, ioInfo);
	sendRoomList();
}

void Process::processSelectedMap(const string& parser){
	//json 꺼내기
	// 맵 번호로 getmap
	Json::Value root;
	Json::Reader reader;
	bool parsingSuccessful = reader.parse(parser, root);
	if (!parsingSuccessful){
		cout << "parser ERROR!" << endl;
	}

	string mapNum = root.get("Mapnum", "").asString();
	int selectedMapNum = atoi(mapNum.c_str());
	string mapData = shanghaiMap.getMap(selectedMapNum);

	auto room = roomManager.roomList.find(socketInfo->titlenum);
	GameRoom* gr = room->second;
	for (auto it : gr->gameUser){
		it->socketData->tilenum = shanghaiMap.getTileNum(selectedMapNum);
	}

	string senddata = "map/" + mapData;
	broadCastRoom(socketInfo->titlenum, senddata);
	
	//아이템 개수도 다시 채워줌

	int hint = room->second->hint;
	int shuffle = room->second->shuffle;

	Json::Value obj;
	obj["shuffle"] = shuffle;
	obj["hint"] = hint;
	Json::FastWriter writer;
	string outputConfig = writer.write(obj);
	string itemdata = "item/" + outputConfig;

	broadCastRoom(socketInfo->titlenum, itemdata);

	// 게임 룸의 상태를 바꿈

	room->second->roomState = GameRoom::GAME;
	sendRoomList();
}

void Process::processRemoveTile(const string& parser1, const string& parser2){
	socketInfo->tilenum = atoi(parser1.c_str());
	string nickName = socketInfo->nickName;
	string remove = "opp/" + nickName + '/' + parser2+ "\n";
	oppBroadCastRoom(socketInfo->titlenum, remove);
}

void Process::processShuffle(const string& parser){
	string nickName = socketInfo->nickName;
	string oppMap = "oppshuffle/" + nickName + '/' + parser+"\n";
	oppBroadCastRoom(socketInfo->titlenum, oppMap);
}

void Process::processClose(const string& parser){

	//접속 종료
}


bool cmpFunc(User* user1, User* user2){
	return user1->socketData->tilenum < user2->socketData->tilenum;
}
// 승패 관련
void Process::processsWin(const string& parser){

	int roomNum = socketInfo->titlenum;
	auto room = roomManager.roomList.find(socketInfo->titlenum);
	GameRoom* gr = room->second;
	int counter = 0;
	for (auto it : gr->gameUser){
		if (it->socketData->state == USERSTATE::GAMEOVER || it->socketData->state == USERSTATE::OBSERVER){
			continue;
		}
		counter++;
	}
	vector<User*> playRanking(counter);
	counter = 0;
	for (auto it : gr->gameUser){
		if (it->socketData->state != USERSTATE::GAMEOVER || it->socketData->state != USERSTATE::OBSERVER){
			playRanking[counter] = it;
			counter++;
		}
	}

	sort(playRanking.begin(), playRanking.end(), cmpFunc);
	int rankSize = playRanking.size();

	Json::Value arr;

	for (int i = 0; i < rankSize; i++){
		Json::Value obj;

		obj["user"] = playRanking[i]->socketData->nickName;
		obj["rank"] = i + 1;
		obj["score"] = 20;

		if (i == 0){
			if (rankSize > 1){
				playRanking[i]->socketData->win++;
				playRanking[i]->socketData->score += 20;

			}
			else{
				playRanking[i]->socketData->lose++;
				if (playRanking[i]->socketData->score > 5){
					playRanking[i]->socketData->score -= 5;
				}
				else{
					playRanking[i]->socketData->score = 0;
				}
			}
		}

		arr.append(obj);
	}
	Json::FastWriter writer;
	string outputConfig = writer.write(arr);
	string senddata = "final/" + outputConfig;

	broadCastRoom(socketInfo->titlenum, senddata);

	gr->roomState = GameRoom::WAIT;
	for (auto it : gr->gameUser){
		//방장 설정
		string userName = it->socketData->nickName;
		if (gr->chiefName == userName){
			it->socketData->state = USERSTATE::CHIEF;
		}
	}
	sendRoomList();
}

void Process::processsGameOver(const string& parser){
	auto room = roomManager.roomList.find(socketInfo->titlenum);
	if (room->second->roomState == GameRoom::GAME && socketInfo->state != USERSTATE::OBSERVER){
		cout << "게임 오버!" << endl;
		gameOverUser();
	}
}

void Process::processsUser(const string& parser){
	throw std::logic_error("The method or operation is not implemented.");
}

void Process::processsObserver(const string& parser){
	//
	int userRoomNum = socketInfo->titlenum;
	if (isGameState(userRoomNum)){
		Json::Value obj;
		string user = socketInfo->nickName;
		obj["userstate"] = "observer";
		obj["user"] = user;
		obj["num"] = socketInfo->usernum;
		Json::FastWriter writer;
		string outputConfig = writer.write(obj);

		string senddata = "requestMap/" + outputConfig;
		
		broadCastRoom(userRoomNum,senddata);
	}
	else{
		Json::Value arr;
		string senddata;
		
		auto room = roomManager.roomList.find(userRoomNum);
		string userName = socketInfo->nickName;
		if (room->second->chiefName == userName){
			senddata = "zzangs/";
		}
		else{
			senddata = "users/";
		}
		GameRoom* gr = room->second;
		for (auto it : gr->gameUser){
			Json::Value obj;

			string name = it->socketData->nickName;
			int num = it->socketData->usernum;
			if (it->socketData->state == USERSTATE::CHIEF){
				obj["userstate"] = "zzang";
			}
			else{
				//의문의 코드 *************
				it->socketData->state = USERSTATE::USER;
				obj["userstate"] = "user";
			}
			obj["user"] = it->socketData->nickName;
			obj["num"] = it->socketData->usernum;
			arr.append(obj);
		}
		Json::FastWriter writer;
		string outputConfig = writer.write(arr);
		senddata = senddata + outputConfig;
		
		sendStringData(senddata, clientSocket, ioInfo);
		
		//적들에게 보내는 메시지
		Json::Value obj;
		obj["userstate"] = "user";
		obj["user"] = socketInfo->nickName;
		obj["num"] = socketInfo->usernum;

		outputConfig = writer.write(obj);

		senddata = "oppinfo/" + outputConfig;

		oppBroadCastRoom(socketInfo->titlenum , senddata);
	}
}

void Process::processsRestart(const string& parser){
	Json::Value arr;
	string senddata = "";
	if (socketInfo->state == USERSTATE::CHIEF){
		senddata = "zzangs/";
	}
	else{
		senddata = "users/";
	}

	auto room = roomManager.roomList.find(socketInfo->titlenum);
	GameRoom* gr = room->second;
	for (auto it : gr->gameUser){
		Json::Value obj;
		string name = it->socketData->nickName;
		int num = it->socketData->usernum;

		if (it->socketData->state == USERSTATE::CHIEF){
			obj["userstate"] = "zzang";
		}
		else{
			//-> 주의
			it->socketData->state = USERSTATE::USER;
			obj["userstate"] = "user";
		}
		obj["user"] = name;
		obj["num"] = num;
		arr.append(obj);
	}
	Json::FastWriter writer;
	string userdata = writer.write(arr);

	senddata = senddata + userdata;
	sendStringData(senddata, socketInfo->clientSocket, ioInfo);
}

void Process::processsOutRoom(const string& parser){
	auto room = roomManager.roomList.find(socketInfo->titlenum);

	if (room->second->roomState == GameRoom::GAME && socketInfo->state != USERSTATE::OBSERVER){
		gameOverUser();
	}
	outRoomUser();
}

void Process::processsObserverMap(const string& parser1, const string& parser2){
	auto room = roomManager.roomList.find(socketInfo->titlenum);
	GameRoom* gr = room->second;
	for (auto it : gr->gameUser){
		string userName = it->socketData->nickName;
		if (parser1 == userName){
			Json::Value obj;
			//보내는 사람이름
			string nickName = socketInfo->nickName;
			if (nickName == gr->chiefName){
				obj["userstate"] = "zzang";
			}
			else{
				obj["userstate"] = "user";
			}

			obj["user"] = nickName;
			obj["num"] = socketInfo->usernum;
			
			Json::FastWriter writer;
			string userData =writer.write(obj);

			//Json 생성 중 생기는 개행문자(\n)제거를 위한 임시방편
			userData.pop_back();

			string senddata = "sendobservermap/" + userData + '/' + parser2 + "\n";
			sendStringData(senddata, it->socketData->clientSocket, it->ioData);
		}
	}
}

void Process::processsobserverInfo(const string& parser){
	auto room = roomManager.roomList.find(socketInfo->titlenum);
	GameRoom* gr = room->second;
	for (auto it : gr->gameUser){
		string userName = it->socketData->nickName;
		if (parser == userName){
			Json::Value obj;
			string nickName = socketInfo->nickName;
			obj["userstate"] = "observer";
			obj["user"] = nickName;
			obj["num"] = socketInfo->usernum;
			Json::FastWriter writer;
			string userData = writer.write(obj);
			string senddata = "sendobserverInfo/" + userData + "\n";

			sendStringData(senddata, it->socketData->clientSocket, it->ioData);
		}
	}
}


void Process::sendRoomList(){
	
	//모든 방을 검색한다.
	Json::Value arr;
	for (auto it : roomManager.roomList){
		int roomNum = it.first;
		string roomName = it.second->roomName;
		int size = it.second->gameUser.size();
		string chief = it.second->chiefName;
		string state;
		if (it.second->roomState == GameRoom::GAME){
			state = "play";
		}
		else{
			state = "wait";
		}
		Json::Value obj;

		obj["roomnum"] = roomNum;
		obj["roomname"] = roomName;
		obj["size"] = size;
		obj["zzang"] = chief;
		obj["roomstate"] = state;

		arr.append(obj);
	}
	
	Json::FastWriter writer;
	string outputConfig = writer.write(arr);
	string senddata;

	senddata = "roomlist/" + outputConfig;
	
	for (auto it : roomManager.userList){
		SOCKET_DATA* userData = it.second->socketData;
		if (userData->state == USERSTATE::WAITROOM){
			IO_DATA* userIo = it.second->ioData;
			sendStringData(senddata, userData->clientSocket, userIo);
		}
	}
}

void Process::sendStringData(const string& senddata, SOCKET cSocket, IO_DATA* userIo){
	//cout << "보내는 메시지 : " << senddata << endl;
	userIo->wsaBuf.buf = (Char*)senddata.c_str();
	userIo->wsaBuf.len = senddata.size();
	userIo->readWriteMode = WRITE;
	WSASend(cSocket, &(userIo->wsaBuf), 1, NULL, 0, &(userIo->overlapped), NULL);
	//WSASend(cSocket, &(wb), 1, NULL, 0, &(overlap), NULL);
}

const bool Process::isGameState(int titlenum){
	auto room = roomManager.roomList.find(titlenum);
	if (room->second->roomState == GameRoom::GAME){
		return true;
	}
	return false;
}

void Process::broadCastRoom(int userRoomNum,const string& senddata){
	auto room = roomManager.roomList.find(userRoomNum);
	GameRoom* gr = room->second;
	for (auto it : gr->gameUser){
		sendStringData(senddata,it->socketData->clientSocket,it->ioData);
	}
}

void Process::oppBroadCastRoom(int userRoomNum, const string& senddata){
	auto room = roomManager.roomList.find(userRoomNum);
	GameRoom* gr = room->second;
	for (auto it : gr->gameUser){
		if (it->socketData->usernum != socketInfo->usernum){
			sendStringData(senddata, it->socketData->clientSocket, it->ioData);
			//cout << it->socketData->nickName << "유저에게 방 메시지를 보냄 : " + senddata << endl;
		}
	}
}

void Process::gameOverUser(){
	//등수 기록
	//현재 플레이어수

	int counter = 0;

	auto room = roomManager.roomList.find(socketInfo->titlenum);
	GameRoom* gr = room->second;
	for (auto it : gr->gameUser){
		if (it->socketData->state == USERSTATE::USER || it->socketData->state == USERSTATE::CHIEF){
			counter++;
		}
	}

	socketInfo->state = USERSTATE::GAMEOVER;
	Json::Value arr;
	Json::Value obj;
	string nickName = socketInfo->nickName;
	obj["user"] = nickName;
	obj["rank"] = counter;
	obj["score"] = 20;

	arr.append(obj);

	counter = 0;

	//현재 방에서 게임 중인 유저 수
	SOCKET_DATA* winUserData = nullptr;
	for (auto it : gr->gameUser){
		if (it->socketData->state == USERSTATE::USER || it->socketData->state == USERSTATE::CHIEF){
			winUserData = it->socketData;
			counter++;
		}
	}
	//유저가 졌을 경우 게임의 승패가 판정나는 경우
	if (counter == 1){
		Json::Value winner;

		winner["user"] = winUserData->nickName;
		winner["rank"] = 1;
		winner["score"] = 20;
		winUserData->win++;
		winUserData->score += 20;

		arr.append(winner);
		Json::FastWriter writer;
		string outData = writer.write(arr);
		string senddata = "final/" + outData;
		broadCastRoom(room->first, senddata);

		room->second->roomState = GameRoom::WAIT;

		for (auto it : gr->gameUser){
			//방장 설정
			string userName = it->socketData->nickName;
			if (gr->chiefName == userName){
				it->socketData->state = USERSTATE::CHIEF;
			}
		}
		sendRoomList();
	}
	//유저가 혼자 게임하던 경우
	else if (counter == 0){
		Json::FastWriter writer;
		string outData = writer.write(arr);
		string senddata = "final/" + outData;
		sendStringData(senddata, socketInfo->clientSocket, ioInfo);

		room->second->roomState = GameRoom::WAIT;

		for (auto it : gr->gameUser){
			//방장 설정
			string userName = it->socketData->nickName;
			if (gr->chiefName == userName){
				it->socketData->state = USERSTATE::CHIEF;
			}
		}
		sendRoomList();
	}
	//아직 게임 중인 유저가 있을 경우
	else{
		Json::FastWriter writer;
		string gameOverData = writer.write(arr);

		string senddata = "usergameover/" + gameOverData;

		broadCastRoom(room->first, senddata);
	}
}

void Process::outRoomUser(){
	Json::Value obj;

	string name = socketInfo->nickName;
	int num = socketInfo->usernum;
	
	int titleNum = socketInfo->titlenum;
	
	obj["userstate"] = "user";
	obj["user"] = name;
	obj["num"] = num;
	Json::FastWriter writer;
	string outData = writer.write(obj);
	string outUser = "delroomuser/" + outData;
	oppBroadCastRoom(titleNum, outUser);

	//방을 나가는 코드작성

	auto room = roomManager.roomList.find(socketInfo->titlenum);
	GameRoom* gr = room->second;

	User* user = gr->findUser(name);
	//방 유저 목록에서 삭제
	gr->deleteUser(user);

	user->socketData->titlenum = 0;
	user->socketData->state = USERSTATE::WAITROOM;
	user->socketData->usernum = 0;

	if (gr->isEmpty()){
		roomManager.deleteGameRoom(room->first);
	}
	else{
		//방장을 바꾼다.
		if (name == gr->chiefName){
			User* nextChiefUser = gr->getFirstUser();
			gr->chiefName = nextChiefUser->socketData->nickName;

			if (gr->roomState == GameRoom::WAIT){
				Json::Value chief;

				chief["userstate"] = "zzang";
				chief["user"] = gr->chiefName;
				chief["num"] = nextChiefUser->socketData->usernum;

				Json::FastWriter writer;
				string chiefData = writer.write(chief);

				string senddata = "movezzang/" + chiefData;

				oppBroadCastRoom(room->first, senddata);
			}
		}
	}
	sendRoomList();
}
/* 중복된 코드
void Process::outGameRoomUser(){
	auto room = roomManager.roomList.find(socketInfo->titlenum);
	GameRoom* gr = room->second;

	if (gr->roomState == GameRoom::GAME){

	}
}
*/
void Process::outLobby(){
	string outUser = socketInfo->nickName;
	roomManager.deleteUserAtList(outUser);
}

void Process::exitUser(){
	if (socketInfo->titlenum != 0){
		auto room = roomManager.roomList.find(socketInfo->titlenum);
		GameRoom* gr = room->second;

		if (gr->roomState == GameRoom::GAME && socketInfo->state != USERSTATE::OBSERVER){
			gameOverUser();
		}
		outRoomUser();
	}
	cout<< "이름의 길이 : " << socketInfo->nameLen<<endl;
	if (socketInfo->nameLen != 0)
		outLobby();
}
