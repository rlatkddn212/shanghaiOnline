// stdafx.h : 자주 사용하지만 자주 변경되지는 않는
// 표준 시스템 포함 파일 및 프로젝트 관련 포함 파일이
// 들어 있는 포함 파일입니다.
//

#pragma once

#include "targetver.h"

#include <stdio.h>
#include <tchar.h>
#include <string.h>
#include <stdlib.h>
#include <process.h>
#include <WinSock2.h>
#include <Windows.h>
#include <atlstr.h>
#include <iostream>
#include <fstream>
#include <vector>
#include <list>
#include <string>
#include <hash_map>
#include <algorithm>
#include <ctime>
#include <random>
#pragma comment(lib, "ws2_32.lib")
#pragma comment(lib, "mswsock.lib")
#pragma comment(lib, "Winmm.lib")

// TODO: 프로그램에 필요한 추가 헤더는 여기에서 참조합니다.
//
#include "singleton.h" 
#include "type.h"
#include "random.h"
#include "shanghai_map.h"
#include "socket_info.h"
#include "user.h"
#include "router.h"
#include "network.h"
#include "game_room.h"
#include "room_manager.h"
#include "message_process.h"
