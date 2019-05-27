#pragma once
#include "stdafx.h"

class ShanghaiMap :public Singleton<ShanghaiMap> {
private:
	std::vector<Json::Value> mapContainer;

	void installMap(std::string fileName);

public:
	ShanghaiMap(){
	}
	void createMap();
	void shuffleMap(Json::Value& jsonMap);

	int getTileNum(int mapNum);
	std::string getMap(int mapNum);

};