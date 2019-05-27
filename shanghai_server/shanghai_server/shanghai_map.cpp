#include "stdafx.h"
#include "shanghai_map.h"
using namespace std;

void ShanghaiMap::installMap(string fileName){
	//MapData md;
	//md.arrMap[][][];

	ifstream readMapFile(fileName);
	string s;
	getline(readMapFile, s);
	//cout << s;
	Json::Value root;
	Json::Reader reader;
	bool parsingSuccessful = reader.parse(s, root);
	if (!parsingSuccessful){
		cout << "parser ERROR!" << endl;
	}
	int rootSize = root.size();
	int num = 0;
	int cnt = 0;
	for (int i = 0; i < rootSize; i++){
		//string  x = root[i].get("x", "").asString();
		root[i]["Tile"] = num + 1;
		if (cnt % 4 == 3) {
			num += 1;
		}
		cnt += 1;
	}
	mapContainer.push_back(root);
}

void ShanghaiMap::createMap(){
	string map1Name = "test1.txt";
	string map2Name = "test2.txt";
	string map3Name = "test3.txt";

	installMap(map1Name);
	installMap(map2Name);
	installMap(map3Name);
}

void ShanghaiMap::shuffleMap(Json::Value& jsonMap){
	int jsonSize = jsonMap.size();
	for (int i = 0; i< jsonSize; i++){
		int randomValue = RAND(int, jsonSize-1);
		int a = jsonMap[i].get("Tile","").asInt();
		int b = jsonMap[randomValue].get("Tile", "").asInt();
		jsonMap[i]["Tile"] = b;
		jsonMap[randomValue]["Tile"]= a;
	}
}

int ShanghaiMap::getTileNum(int mapNum){
	return mapContainer[mapNum].size();
}

string ShanghaiMap::getMap(int mapNum){
	//ShuffleMap(ListMap1);
	Json::Value m = mapContainer[mapNum];
	shuffleMap(m);
	//cout << m;
	Json::FastWriter fw;
	return fw.write(m);
}
