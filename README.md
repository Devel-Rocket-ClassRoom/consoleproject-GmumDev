# DungeonGameReadme

상태: 아이디어

### Flow

1. Main에서 DungeonGame 인스턴스를 생성하여 Play 합니다.
2. Stage를 생성합니다. Map들을 생성하고 Stage의 그리드 위에 배치합니다.
3. Stage에 반드시 1개 존재하는 시작 맵을 현재 맵으로 설정합니다.
    
    (여기서부터 메인 루프, Update)
    4. 플레이어의 입력을 받습니다.
    4-1. [입력이 WASD라면] 플레이어를 이동하고, 해당 위치의 엔티티의 OnPlayerEnter를 호출하여 관련 태스크를 수행합니다(충돌시 데미지처리, 이동 불가 처리, 제거 및 이동 처리 등). Entity에서 전체 피드백을 자체적으로 처리할 수 없는 경우는 아래 세 경우입니다.
    4-1-1. 해당 엔티티가 Door이고 맵을 클리어했다면, 다음 맵으로 이동합니다.
    4-1-2. 해당 엔티티가 NextStageBicon이고 스테이지를 클리어했다면, 다음 스테이지로 이동합니다.
    4-1-3. 해당 엔티티의 OnPlayerEnter 수행 결과 플레이어가 죽었다면, GameOver 팝업을 띄웁니다.
    
    4-2. [입력이 Q라면] SaveLoad팝업을 띄웁니다.
    4-2-1. Save라면 저장하고, Load라면 불러옵니다.
    4-2-2. Cancel라면 게임을 재개합니다. Quit라면 게임을 종료하고, Restart라면 재시작 합니다.
    
    1. Renderer가 화면에 정보를 그립니다.

# 모든 파일

DungeonGame.cs

- 싱글턴
- 메인 루프가 돌아가고 게임을 저장/구현/재시작/종료 등 합니다.
- 스테이지와 플레이어, 현재 맵을 참조하고 업데이트 합니다.

StageFactory.cs

- 싱글턴
- 스테이지를 생성하고 저장 및 관리합니다.
- 생성된 맵들을 참조하여 맵 간 연결 구조를 갖고 있습니다.

MapFactory.cs

- 싱글턴
- 특정 타입의 맵을 생성합니다.
- 싱글톤 엔티티매니저를 참조하여 엔티티를 맵에 생성합니다.

Map.cs

- Interfaces\IMapDataModifier.cs 를 구현합니다.
- 엔티티를 2차원 Entity 배열에 저장합니다.
- 맵 내의 엔티티에 대한 업데이트와 수정이 이루어집니다.

Interfaces\IMapDataModifier.cs

- SetEntity<T>, GetEntity(x, y), Swap(point 1, point 2) 등 맵을 수정하기 위한 인터페이스를 제공합니다.

Entities\EntityManager.cs

- 싱글턴
- 맵의 특정 위치에 엔티티 객체를 생성하고 반환합니다.

Entities\Entity.cs

- 추상 클래스.  Interfaces\IEntity.cs 를 구현합니다.
- 각 엔티티에 대한 정보를 저장하고 Update를 수행합니다.
- 플레이어가 해당 엔티티와 닿았을 때의 동작을 정의합니다.
    - Entities\Player.cs
        - 인벤토리를 보유합니다. ItemShop에서 아이템을 구매합니다.
    - Entities\Door.cs
    - Entities\EmptyEntity.cs
    - Entities\Fence.cs
    - Entities\HealingPotion.cs
    - Entities\ItemShop.cs
        - 아이템을 판매합니다. 판매할 아이템은 MapFactory.cs에서 결정됩니다.
        - ItemDBContainer.cs 에서 아이템 정보를 얻습니다.
    - Entities\MapClearBicon.cs
    - Entities\Monster.cs
    - Entities\NextStageBicon.cs
    - Entities\SnakeBaby.cs
    - Entities\Snake_Head.cs
        - Entities\BigSnake_Head.cs
    - Entities\Snake_Tail.cs

 Interfaces\IEntity.cs

- Init, Update, OnPlayerEnter, GetEntityEnum 등을 선언합니다.

Utility.cs

- static 클래스
- Random 인스턴스 등 전역에서 참조합니다.
- 데이터의 Json 저장, 불러오기를 제공합니다.

RawDataParser.cs

- static 클래스
- 주로 Utility의 Json 관련 메소드와 객체 X 사이의 어댑터 역할을 합니다.
- Raw X Data 구조체를 해당하는 구조체 인스턴스로 Parse 합니다. 또는 그 반대

Structs

- Structs\MyVector.cs
- Structs\Square.cs
- Structs\RawGameData.cs
- Structs\RawItemData.cs
- Structs\RawMapData.cs
- Structs\RawPlayerData.cs
- Structs\RawStageData.cs
- Structs\ShopItemData.cs
    - 상점 아이템 데이터입니다.
    - Structs\RawItemData.cs 를 상점 객체가 사용할 수 있도록 변환한 형태입니다.

ItemDBContainer.cs

- 모든 아이템 정보를 구조체로 저장하고 있는 컨테이너입니다.

Jsons

- Jsons\Game.json
- Jsons\ItemData.json
- Jsons\TestMapData.json

Renderer.cs

- RenderBox들을 화면에 출력합니다.

Interfaces\IRenderable.cs

- Entity와 RenderBox가 구현하는 인터페이스 입니다.
- Render(char[,] parentBuf) 를 선언합니다.
- Entity는 Map의 버퍼에 Render를 수행하고, RenderBox는 Renderer에 수행합니다.
- 최종적으로 Renderer가 전체를 한 번에 그립니다.

AlertRenderer.cs

- AlertRenderBox를 통해 Alert(팝업 창)을 발생시킵니다.

RenderBoxes\RenderBox.cs

- 추상 클래스. 화면에 그려지는 블럭입니다.
    - RenderBoxes\AlertRenderBox.cs
        - 팝업을 발생시키고, 플레이어의 선택지에 따라 Callback을 수행합니다.
        - 게임 시작/재시작/저장/불러오기 및 상점 UI가 이 클래스를 사용하여 구현되었습니다.
    - RenderBoxes\InputBox.cs
    - RenderBoxes\InventoryBox.cs
    - RenderBoxes\MapRenderBox.cs
    - RenderBoxes\MinimapRenderBox.cs
    - RenderBoxes\MsgRenderBox.cs
    - RenderBoxes\PlayerRenderBox.cs
    - RenderBoxes\ShopAlertRenderBox.cs

Enums

- Enums\DirIndex.cs
    - 동서남북 네 방향을 int로 변환하기 위한 Enum입니다.
- Enums\EntityEnum.cs
    - 엔티티의 ID입니다.
- Enums\ItemID.cs
    - 아이템의 ID입니다.

Program.cs

- 메인 함수가 실행됩니다. 프로그램의 진입점입니다.

### 시작단계, 입력단계, 맵을 가져오는 단계, 몬스터와의 전투 등 전체적인 게임의 흐름을 단계 별로 설명해주세요.

1. Main에서 DungeonGame 인스턴스를 생성하여 Play 합니다.
2. Stage를 생성합니다. Map들을 생성하고 Stage의 그리드 위에 배치합니다.
3. Stage에 반드시 1개 존재하는 시작 맵을 현재 맵으로 설정합니다.

(여기서부터 메인 루프, Update)
4. 플레이어의 입력을 받습니다.
4-1. [입력이 WASD라면] 플레이어를 이동하고, 해당 위치의 엔티티의 OnPlayerEnter를 호출하여 관련 태스크를 수행합니다(충돌시 데미지처리, 이동 불가 처리, 제거 및 이동 처리 등). Entity에서 전체 피드백을 자체적으로 처리할 수 없는 경우는 아래 세 경우입니다.
4-1-1. 해당 엔티티가 Door이고 맵을 클리어했다면, 다음 맵으로 이동합니다.
4-1-2. 해당 엔티티가 NextStageBicon이고 스테이지를 클리어했다면, 다음 스테이지로 이동합니다.
4-1-3. 해당 엔티티의 OnPlayerEnter 수행 결과 플레이어가 죽었다면, GameOver 팝업을 띄웁니다.

4-2. [입력이 Q라면] SaveLoad팝업을 띄웁니다.
4-2-1. Save라면 저장하고, Load라면 불러옵니다.
4-2-2. Cancel라면 게임을 재개합니다. Quit라면 게임을 종료하고, Restart라면 재시작 합니다.

1. Renderer가 화면에 정보를 그립니다.

### 추가로 본인이 구현한 내용 중 위 항목에 없는 내용이거나 로직 중 자세한 설명이 필요한 경우 추가로 입력해 주세요.

[StageFactory]
StageFactory의 경우 초기엔 Factory로 설계했으나 현재 Manager의 역할도 겸합니다. 스테이지를 생성하는 방법은 아래와 같습니다.

1. 몇 개의 방(Map)을 생성할지 랜덤하게 결정합니다. 그리드의 최대 크기를 벗어나지 않습니다. 그리드는 미니맵을 그리는 데에 사용되고, 맵 간 연결을 쉽게 도와주는 그래프로 활용됩니다.
2. 시작 맵의 위치를 정합니다. 끝 맵의 위치도 정해진 방의 개수 이내에 도달할 수 있도록 정합니다.
3. MainPath는 시작맵에서 끝 맵을 잇는 최단 경로입니다. 여러 종류의 MainPath가 발생 할 수 있으나 모두 최단경로이고, 이 중 랜덤으로 하나를 고릅니다.
4. MainPath에 포함된 맵의 수가 초기에 정해진 맵의 수보다 적다면 추가로 맵을 생성합니다. 이렇게 생성하는 맵은 사이드 맵입니다.
5. 사이드 맵의 위치는 MainPath의 맵 중 랜덤하게 선택된 맵에서 BFS를 통해 탐색한 첫번째 빈 그리드의 위치로 선택됩니다. 선택된 사이드맵은 MainPath로 합류하여, 다음 사이드 맵이 이 사이드 맵에 이어서 생길 수 있도록 합니다.
6. 초기에 정해진 개수의 맵을 모두 생성하면, 각 맵의 그리드 위치를 참고하여 맵마다 Door Entity를 생성해 서로를 연결합니다.
7. 스테이지의 클리어 조건을 각 맵의 클리어 조건 전부 달성하기로 설정합니다.

[MapFactory]
MapFactory는 Factory역할을 합니다. 맵을 생성하는 과정은 아래와 같습니다.

1. List<FillUpMapDelegate> FillUpMapTasks에서 랜덤하게 맵을 채울 방법을 고르고, 맵을 인자로 받습니다. 이 맵은 StageFactory에서 테두리만 쳐준 빈 껍데기 맵입니다. 테두리는 Fence, 나머지는 EmptyEntity로 채워져 있습니다.
2. 단, 시작 맵과 끝 맵은 특정한 함수로만 채워집니다.
3. 랜덤하게 선택된 콜백 함수 FillUpMapTasks[i]를 실행하여 맵을 Entity로 채웁니다.
4. FillUp 방식마다의 맵 클리어 조건을 설정합니다.

[RenderBox]
RenderBox를 상속받은 클래스를 AlertRenderBox와 나머지로 분류하면, 다른 RenderBox가 단순히 화면에 그려주는 역할을 하는 반면 AlertRenderBox들은 각 선택지에 해당하는 입력키와 콜백 딕셔너리를 갖고 있습니다. 즉, <char, Callback> 입니다. 따라서 플레이어가 해당 키를 누르면 콜백이 수행되며, 이 콜백을 통해 다른 AlertRenderBox가 활성화 될 수 있습니다. 예를들어, 3개의 아이템을 판매하는 상점UI는 총 6개의 팝업으로 구성됩니다. 그 중 2개는 아이템을 선택하기 위한 A, D 입력키를 갖고, 1개는 선택된 아이템을 구매하기 위한 G 입력 키를 가지며, 나머지 3개는 각 아이템을 화면에 표시하기 위함입니다.

[Snake]
Snake는 플레이어가 제거할 수 없는 Entity입니다. Monster의 경우 그 위치로 플레이어가 이동하면 보상으로 $1을 제공하고 사라지지만, Snake는 플레이어가 이동할 때마다 주변의 빈 칸으로 이동합니다. 또한 Snake는 Snake_Head와 Snake_Tail로 구성되어있고, 실제 이동하는 것은 Snake_Head이지만 Snake_Tail이 이전 n번째 프레임의 Snake_Head 위치에 잔류합니다. n은 Snake의 길이와 비례합니다.
또한 Snake가 구석에 몰리고 자신의 꼬리로 둘러싸인 상황에서, Snake는 주변에 빈 칸이 없어 이동할 수 없습니다. 이러한 Stucked 상태가 stuckedTurn 프레임 만큼 진행되면, 머리는 가장 끝 꼬리의 위치로 이동합니다. 뱀이 자신의 꼬리를 먹는 모습을 상상하세요, 딱 그런 방식으로 동작합니다.
Snake가 등장하는 맵에서는 클리어 조건을 달성하기 위해, Snake가 일정 확률로 생성하는 MapClearBicon Entity를 획득해야합니다. 이는 Snake의 가장 끝 꼬리를 대체하는 식으로 생성되고 Snake_Tail처럼 Snake_Head를 따라다닙니다. MapClearBicon과의 충돌은 맵을 클리어 처리합니다.
SnakeBaby는 n턴 후 Snake가 되는 Entity입니다. 그 전에 충돌하면 제거할 수 있습니다. 그러나 돈을 제공하지는 않습니다.

[ItemDBContainer]
게임 데이터 뿐만아니라 아이템 정보 또한 Json으로 관리합니다.

[MsgRenderBox]
메시지를 출력합니다.

[MinimapRenderBox]
미니맵을 그립니다.

[InventoryBox]
인벤토리를 그립니다.

[InputBox]
미구현입니다!! 플레이어 입력에 대한 UI를 표시할 예정이었습니다.

### 기타

- 빠른 출력을 위해 Stringbuilder 사용
- Entity의 확장성 확보
    - EntityManager와 IEntity를 사용하여 새로운 Entity 추가에 유리
    - Player, Fence, Monster에 더하여 아이템 HealingPotion과 상점 ItemShop 등 10개 이상의 다양한 Entity를 구현
- Json 사용
    - ItemDBContainer에서 RawItemData 구조체 형태로 Json파일을 읽어들여 Item의 정보를 관리하고, ItemShop 등에서 활용한다.
    - …
- Delegate 사용
    - AlertRenderBox, DungeonGame_MapFactory 등 다양한 곳에서 유의미하게 활용
        - AlertRenderBox: 팝업의 선택지를 선택하여 새로운 팝업을 띄울 수 있다. 팝업을 띄우는 Alert() 함수가 delegate에 전달되어 구현되었다.
        - DungeonGame_MapFactory : Map을 생성할 때 랜덤하게 메소드를 선택하여 다양한 Map을 반환하는 GetMapTasks 대리자 리스트를 활용한다.