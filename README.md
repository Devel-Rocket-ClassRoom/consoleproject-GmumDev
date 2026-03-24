# DungeonGameReadme

상태: 아이디어

### Flow

[https://excalidraw.com/#json=E4_v2Q6zOM9BH7GeYnFni,roAtR2PARaUdbZQL7BUmhg](https://excalidraw.com/#json=E4_v2Q6zOM9BH7GeYnFni,roAtR2PARaUdbZQL7BUmhg)

### Quick Start

- dd
- dd

### ㅇㅅㅇ

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
-