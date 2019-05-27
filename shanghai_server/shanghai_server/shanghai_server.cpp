#include "stdafx.h"

int _tmain(int argc, _TCHAR* argv[])
{
	Network& server = Network::getInstance();
	ShanghaiMap& shanghaiMap = ShanghaiMap::getInstance();
	shanghaiMap.createMap();
	//std::string& s = shanghaiMap.getMap(0);
	//std::cout << s;
	
	server.startServer();
	return 0;
}