## 상하이 Network 게임



![1560470261848](https://github.com/rlatkddn212/shanghaiOnline/blob/master/assets/1560470261848.png)

![1560470269000](https://github.com/rlatkddn212/shanghaiOnline/blob/master/assets/1560470269000.png)



넷마블 인턴 때 상하이를 개발했었는데 그 게임을 다시 구현했던 코드 입니다.
리소스가 없기 때문에 클라이언트는 빌드가 되지 않습니다.

------------------

## 개발 도구

visual studio 2013으로 개발  
클라이언트 : 유니티 4, C#, NGUI  
서버 : C++, STL, iocp 사용  

------------------

## 소스 코드를 보실려면?

클라이언트 코드 경로 : Shanghai/Assets/Scripts/    
서버 코드 경로 : shanghai_server/shanghai_server/  



------------------------

## 게임 영상을 보실려면?
https://www.youtube.com/watch?v=S0JXCIlJf3Y



-------------------

## 어떻게 개발했나요?



### 기능 명세

| 인증          | 넷마블s                                                      | 아이디 설정 가능   비밀번호 설정 가능   로그인 기록이 있으면 자동 로그인   인증 기록이 없으면 닉네임 재설정 |
| ------------- | ------------------------------------------------------------ | ------------------------------------------------------------ |
| 대기실&게임방 | 방 생성                                                      | 방의 제목 설정   힌트 재배치량 설정   대기실 상태에서 방장 상태로 상태 변화   방에 입장수 5명 제한 |
| 방 입장       | 방을 구분하여 입장   대기실 상태에서 유저 상태로 상태 변화   |                                                              |
| 방 목록       | 방 번호, 방 제목, 방장의 이름, 방 인원수   현재 방의 상태를 보여줌   입장 버튼으로 방에 입장 |                                                              |
| 유저 정보     | 닉네임을 보여줌   승패, 점수를 보여줌                        |                                                              |
| 랭킹          | 1~100위까지 유저들의 목록을 보여줌                           |                                                              |
| 게임          | 맵 선택                                                      | 방장이 3개의 맵중 하나를   선택하여 플레이   시작시 모든 패를 썪어서 시작 |
| 화면          | 자신의 맵과 상대방들의 맵을 볼 수 있도록 화면 구성           |                                                              |
| 패 제거       | 좌우가 열려 있고 최상위에 있는 패들의 짝을 제거   패를 제거 했을 때 애니메이션 효과 |                                                              |
| 터치          | 패를 터치 했을 때 무늬를 보여줌   다른 패를 선택한 경우 무늬 제거 |                                                              |
| 힌트          | 제거할 수 있는 패들의 쌍을 하나 보여줌   애니메이션 효과   힌트 활성화중 다시 힌트를 누를 경우 무작위 짝을 찾아냄 |                                                              |
| 재배치        | 모든 패들을 다시 썪음                                        |                                                              |
| 관전자        | 게임중인 방에 입장 했을 때 진행중인 게임을 관전할 수 있음   상대방의 맵을 클릭하면 화면 보드에 크기를 확대해 보여줌   방장이 게임 재시작 시 관전자도 함께 게임 진행 |                                                              |
| 나가기        | 방을 나가는 기능   방장이 방을 나가면 가장 먼저 들어온 유저에게 방장 위임 |                                                              |
| 승패          | 모든 패를 제거할 경우 게임을 끝내고 남은 패의 개수로 등수를 매김   시간 초과될 경우 남은 패의 개수와 관계없이 낮은 등수를 받음   한 사람을 제외하고 모든 사람이 시간 초과될 경우 1위   관전자는 등수에 포함하지 않음 |                                                              |



### 설계

1) 액티비티 다이어그램

<대기실>

![1560470450578](https://github.com/rlatkddn212/shanghaiOnline/blob/master/assets/1560470450578.png)                                                  

 

2) 상태 다이어그램

!![1560470459989](https://github.com/rlatkddn212/shanghaiOnline/blob/master/assets/1560470459989.png)

구현을 하면서 상태 관리가 복잡해져 상태 다이어그램을 그려 관리 했습니다. User의 상태나 Room의 상태 변화에 따라 로직 처리가 달라지게 됩니다.  

 

3) 클래스 다이어그램 

3-1) 클라이언트 부분

 

**게임 대기실 부분**

   ![1560470475732](https://github.com/rlatkddn212/shanghaiOnline/blob/master/assets/1560470475732.png)



Lobby 클래스는 시작 시 네트워크 연결을 한 뒤 로비에서의 기능을 수행합니다.  

서버와 연결하여 데이터를 받으면 LobbyParser가 데이터를 구분하여 Lobby의 메소드를 실행 시킵니다. 로비의 기능으로는 방목록을 보여주거나 방을 생성/참여 하는 기능이 있습니다.  

 

**게임 룸 부분**

   ![1560470492698](https://github.com/rlatkddn212/shanghaiOnline/blob/master/assets/1560470492698.png)

GameManager는 게임에 관한 모든 로직들을 관리합니다. 적(Opponent)들과 맵(TileMap)의 상태들 유저의 상태등 게임 상태 변화를 서버에서 받아와 처리하는 일들을 합니다.

3-2) 서버 부분

**네트워크 연결 및 데이터 처리 부분**  

 ![1560470498110](https://github.com/rlatkddn212/shanghaiOnline/blob/master/assets/1560470498110.png)

   

 

Network 클래스는 클라이언트의 요청을 받아들이는 acceptThread와 클라이언트의 데이터를 비동기적으로 처리하는 workThread를 생성합니다.  

workThread에서 데이터를 받으면 Router를 생성하여 어떤 데이터인지 구분하고 Process 객체를 생성하여 받은 데이터에 맞는 비즈니스 로직을 실행합니다.  

 

네트워크는 C++은 IOCP를 사용했습니다.  

 ![1560470504235](https://github.com/rlatkddn212/shanghaiOnline/blob/master/assets/1560470504235.png)

   

 

데이터를 받으면 queue에 데이터를 넣고 쓰레드를 통해 처리하는 모델입니다.  

 

**유저 관리 부분**

 ![1560470516385](https://github.com/rlatkddn212/shanghaiOnline/blob/master/assets/1560470516385.png)

   

 

RoomManager클래스는 대기실에 있는 유저와 GameRoom에 있는 유저들을 관리하는 클래스입니다. 방을 생성하거나 유저들을 찾는 역할을 합니다.  

User클래스는 네트워크의 socket데이터와 io데이터 그리고 유저의 상태정보를 가지고 있습니다.  

GameRoom 클래스는 게임룸에 관련된 기능을 수행합니다. 방에 있는 유저를 추가, 삭제, 검색하거나 방의 현재 상태들을 변경하는 일을 합니다.  

 

**맵 생성 부분**

   

 ![1560470522755](https://github.com/rlatkddn212/shanghaiOnline/blob/master/assets/1560470522755.png)

ShanghaiMap 클래스는 서버를 시작하기 전 txt파일에서 Json형식의 맵 데이터를 불러와 vector에 저장 해두고 필요 할 때 꺼내서 사용할 수 있는 클래스입니다. 맵을 랜덤으로 셔플하는 기능도 있습니다.