#pragma once
#include "stdafx.h"
#define RAND(type, maxVal)       (type) Random::getInstance().rand(maxVal)

class Random : public Singleton < Random >{
public:
	unsigned long long rand(int maxVal){
		std::random_device rd;
		std::mt19937 engine(rd());
		std::uniform_int_distribution<int> distribution(0, maxVal);
		return (unsigned long long)(distribution(engine));
	}
};