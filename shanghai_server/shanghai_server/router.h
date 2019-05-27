#pragma once
#include "stdafx.h"

class Router{
private:

public:
	Router(){
	}
	inline std::vector<std::string> split(std::string& s, Char c){
		std::vector<std::string> ret;
		int stringSize = s.size();
		int index = 0;
		int counter = 0;
		for (int i = 0; i < stringSize; i++){
			if (s[i] == c){
				ret.push_back(s.substr(index, counter));
				index = i + 1;
				counter = 0;
			}
			else{
				counter++;
			}
		}
		ret.push_back(s.substr(index, counter));
		return ret;
	}
	void parseMessage(SOCKET clientSocket, SOCKET_DATA* socketInfo, IO_DATA* ioInfo);
	void endMessage(SOCKET clientSocket, SOCKET_DATA* socketInfo, IO_DATA* ioInfo);
};