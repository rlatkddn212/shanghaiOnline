// stdafx.h : ���� ��������� ���� ��������� �ʴ�
// ǥ�� �ý��� ���� ���� �� ������Ʈ ���� ���� ������
// ��� �ִ� ���� �����Դϴ�.
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

// TODO: ���α׷��� �ʿ��� �߰� ����� ���⿡�� �����մϴ�.
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
