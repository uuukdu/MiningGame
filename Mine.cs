using System;
using System.Text;

namespace MiningGame
{
    // 블록 종류
    public enum BlockType
    {
        Empty, // 빈 공간
        Wall,  // 벽
        Dirt,  // 흙
        Stone, // 돌
        Iron,  // 철
        Gold,  // 금
        Diamond// 다이아몬드
    }

    // 광산 클래스
    public class Mine
    {
        public string Name { get; private set; }
        public Pickaxe RequiredPickaxe { get; private set; }
        public Mineral PrimaryMineral { get; private set; }

        // 광산의 크기
        private const int MapWidth = 30;
        private const int MapHeight = 30;

        // 2차원 배열을 사용하여 광산 맵 데이터 저장
        private BlockType[,] _map;

        internal Mine(string name, Pickaxe requiredPickaxe, Mineral primaryMineral)
        {
            Name = name;
            RequiredPickaxe = requiredPickaxe;
            PrimaryMineral = primaryMineral;
        }

        // 광산 입장
        public void EnterMine(Player player)
        {
            GenerateMap(); // 랜덤 맵 생성

            // 플레이어의 현재 위치를 맵의 중앙으로 초기화
            int playerX = MapWidth / 2;
            int playerY = MapHeight / 2;

            // 플레이어의 이전 위치를 저장할 변수
            int prevPlayerX = playerX;
            int prevPlayerY = playerY;

            Console.Clear();
            Console.WriteLine($"[{Name}] (나가기: Esc)");
            // 광산에 처음 입장 시, 전체 맵을 한 번 출력한다.
            DisplayMap(player, playerX, playerY, true);

            // 하단 UI(가이드) 출력.
            Guide(player);

            bool isInMine = true; // 광산 내 루프를 제어할 bool 변수

            while (isInMine)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.RightArrow:
                        prevPlayerX = playerX; prevPlayerY = playerY; // 이동 전 현재 위치를 이전 위치로 저장
                        PlayerMove(player, keyInfo.Key, ref playerX, ref playerY); // playerX, playerY 변수의 주소를 넘겨 메서드 내에서 값을 직접 변경할 수 있게 함
                        UpdatePlayerPosition(player, playerX, playerY, prevPlayerX, prevPlayerY); // 변경된 위치만 화면에 다시 출력
                        break;
                    case ConsoleKey.Q:
                        // 조건문으로 피로도가 남아있는지 확인
                        if (player.Fatigue > 0)
                        {
                            Mining(player, playerX, playerY);
                        }
                        else
                        {
                            Console.SetCursorPosition(0, MapHeight + 5);
                            Console.Write("피로도가 모두 소모되어 더 이상 채굴할 수 없습니다... (아무 키나 눌러 광산 나가기)");
                            Console.ReadKey(true);
                            isInMine = false;
                        }
                        break;
                    case ConsoleKey.D1: // 1번 키 (피로도 드링크 사용)
                        UseItem(player, Items.FatigueDrink);
                        break;
                    case ConsoleKey.D2: // 2번 키 (폭탄 사용)
                        UseItem(player, Items.Bomb, playerX, playerY);
                        break;
                    case ConsoleKey.Escape: // ESC 키 (광산 나가기)
                        isInMine = false;
                        break;
                }
            }
            // 광산에서 나갈 때 피로도를 최대로 회복
            player.Fatigue = player.MaxFatigue;
        }

        // 소모품 아이템 사용
        private void UseItem(Player player, Consumable item, int pX = 0, int pY = 0)
        {
            if (player.Inventory.HasConsumable(item))
            {
                player.Inventory.UseConsumable(item); // 인벤토리에서 아이템 1개 제거
                if (item == Items.FatigueDrink)
                {
                    player.Fatigue += item.FatigueRecovery; // 플레이어 피로도 회복
                    if (player.Fatigue > player.MaxFatigue)
                    {
                        player.Fatigue = player.MaxFatigue; // 최대 피로도를 넘지 않도록 처리
                    }
                }
                else if (item == Items.Bomb)
                {
                    ExplodeBomb(player, pX, pY); // 폭탄 폭발
                }
                Guide(player); // 변경된 상태를 즉시 반영
            }
        }

        // 폭탄이 터졌을 때 주변 블록 변경
        private void ExplodeBomb(Player player, int pX, int pY)
        {
            int targetX = pX;
            int targetY = pY;

            // 플레이어가 바라보는 방향으로 한 칸 앞을 폭발(3x3) 중심으로 설정
            switch (player.PlayerDirection)
            {
                case Direction.Up: targetY--; break;
                case Direction.Down: targetY++; break;
                case Direction.Left: targetX--; break;
                case Direction.Right: targetX++; break;
            }

            for (int y = targetY - 1; y <= targetY + 1; y++)
            {
                for (int x = targetX - 1; x <= targetX + 1; x++)
                {
                    // 맵 경계를 벗어나지 않는지, 해당 위치가 흙 블록인지 확인
                    if (y > 0 && y < MapHeight - 1 && x > 0 && x < MapWidth - 1 && _map[y, x] == BlockType.Dirt)
                    {
                        _map[y, x] = BlockType.Empty; // 맵 데이터를 빈 공간으로 변경
                        DrawBlock(x, y); // 변경된 블록 하나만 다시 그려 화면을 업데이트
                    }
                }
            }
        }

        // 플레이어 이동
        private void PlayerMove(Player player, ConsoleKey key, ref int pX, ref int pY)
        {
            int targetX = pX;
            int targetY = pY;

            // 바라보는 방향을 먼저 바꾸고, 목표 좌표를 계산
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    player.PlayerDirection = Direction.Up;
                    targetY--;
                    break;
                case ConsoleKey.DownArrow:
                    player.PlayerDirection = Direction.Down;
                    targetY++;
                    break;
                case ConsoleKey.LeftArrow:
                    player.PlayerDirection = Direction.Left;
                    targetX--;
                    break;
                case ConsoleKey.RightArrow:
                    player.PlayerDirection = Direction.Right;
                    targetX++;
                    break;
            }

            // 목표 좌표가 빈 공간일 때만 실제 플레이어 위치 이동(변경)
            if (_map[targetY, targetX] == BlockType.Empty)
            {
                pX = targetX;
                pY = targetY;
            }
        }

        // 채굴
        private void Mining(Player player, int pX, int pY)
        {
            int targetX = pX;
            int targetY = pY;

            switch (player.PlayerDirection)
            {
                case Direction.Up: targetY--; break;
                case Direction.Down: targetY++; break;
                case Direction.Left: targetX--; break;
                case Direction.Right: targetX++; break;
            }

            BlockType block = _map[targetY, targetX]; // 목표 좌표의 블록 타입을 가져온다
            bool mined = false; // 채굴성공 여부 저장 변수

            if (block == BlockType.Dirt)
            {
                _map[targetY, targetX] = BlockType.Empty;
                mined = true;
            }
            // 벽이나 빈 공간이 아닌, 광물일 경우
            else if (block != BlockType.Wall && block != BlockType.Empty)
            {
                _map[targetY, targetX] = BlockType.Empty;
                player.Inventory.AddItem(PrimaryMineral); // 광산의 주요 광물을 인벤토리에 추가
                mined = true;
            }

            // 채굴에 성공했다면
            if (mined)
            {
                player.Fatigue--; // 피로도 1 감소
                DrawBlock(targetX, targetY); // 변경된 블록만 다시 그리기
                Guide(player); // 변경된 피로도와 아이템 개수를 UI에 업데이트
            }
        }

        // 랜덤 맵 생성
        private void GenerateMap()
        {
            _map = new BlockType[MapHeight, MapWidth];
            Random rand = new Random();

            // 이 광산의 주요 광물(PrimaryMineral)에 따라 생성될 광물 블록 타입을 결정
            BlockType mineralBlockType = BlockType.Empty;
            if (PrimaryMineral == Items.Iron) mineralBlockType = BlockType.Iron;
            else if (PrimaryMineral == Items.Gold) mineralBlockType = BlockType.Gold;
            else if (PrimaryMineral == Items.Diamond) mineralBlockType = BlockType.Diamond;

            // 2차원 배열을 순회하며 각 좌표에 블록을 할당
            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    // 테두리를 벽으로 설정
                    if (y == 0 || y == MapHeight - 1 || x == 0 || x == MapWidth - 1)
                    {
                        _map[y, x] = BlockType.Wall;
                    }
                    else
                    {
                        // 10% 확률로 광물을 배치
                        if (rand.Next(1, 101) <= 10)
                        {
                            _map[y, x] = mineralBlockType;
                        }
                        else
                        {
                            _map[y, x] = BlockType.Dirt; // 나머지는 흙으로 채우기
                        }
                    }
                }
            }
            // 가운데를 빈 공간으로 설정
            _map[MapHeight / 2, MapWidth / 2] = BlockType.Empty;
        }

        // 맵 관리
        private void DisplayMap(Player player, int pX, int pY, bool fullDraw)
        {
            if (fullDraw) // true일 때만 전체 맵을 그린다.
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    for (int x = 0; x < MapWidth; x++)
                    {
                        DrawBlock(x, y);
                    }
                }
            }
            DrawPlayer(player, pX, pY); // 플레이어는 항상 바뀌기 때문에 항상 그리기
        }

        // 특정 좌표(x, y)에 해당하는 블록 하나를 그리는 메서드입니다.
        private void DrawBlock(int x, int y)
        {
            // Console.SetCursorPosition은 다음에 출력될 텍스트의 위치를 지정합니다.
            // x좌표에 2를 곱하는 이유는 콘솔에서 글자 하나가 세로 길이에 비해 가로 길이가 짧기 때문
            Console.SetCursorPosition(x * 2, y + 1);

            BlockType block = _map[y, x];
            char blockChar = ' ';

            // switch문으로 블록 타입에 맞는 색상과 문자를 설정
            switch (block)
            {
                case BlockType.Wall: Console.ForegroundColor = ConsoleColor.Red; blockChar = '■'; break;
                case BlockType.Dirt: Console.ForegroundColor = ConsoleColor.DarkYellow; blockChar = '▨'; break;
                case BlockType.Stone: Console.ForegroundColor = ConsoleColor.DarkGray; blockChar = '●'; break;
                case BlockType.Iron: Console.ForegroundColor = ConsoleColor.Gray; blockChar = '●'; break;
                case BlockType.Gold: Console.ForegroundColor = ConsoleColor.Yellow; blockChar = '●'; break;
                case BlockType.Diamond: Console.ForegroundColor = ConsoleColor.Cyan; blockChar = '●'; break;
                case BlockType.Empty:
                    Console.Write("  "); // 빈 공간은 공백 2칸으로 덮어쓴다
                    return; // 더 이상 그릴 필요 없으므로 메서드 종료
            }
            Console.Write(blockChar); // 선택된 문자를 출력한다
            Console.ResetColor(); // 다음 출력에 영향을 주지 않도록 색상을 초기화한다
        }

        // 플레이어 캐릭터 출력
        private void DrawPlayer(Player player, int pX, int pY)
        {
            Console.SetCursorPosition(pX * 2, pY + 1);
            Console.ForegroundColor = ConsoleColor.Green;
            char playerChar = ' ';
            // 플레이어가 바라보는 방향에 따라 다른 모양을 출력
            switch (player.PlayerDirection)
            {
                case Direction.Up: playerChar = '▲'; break;
                case Direction.Down: playerChar = '▼'; break;
                case Direction.Left: playerChar = '◀'; break;
                case Direction.Right: playerChar = '▶'; break;
            }
            Console.Write(playerChar);
            Console.ResetColor();
        }

        // 화면 업데이트
        private void UpdatePlayerPosition(Player player, int newX, int newY, int oldX, int oldY)
        {
            // 플레이어의 위치가 실제로 바뀌었다면
            if (newX != oldX || newY != oldY)
            {
                DrawBlock(oldX, oldY); // 이전 위치는 원래 블록으로 덮어서 지우고
                DrawPlayer(player, newX, newY); // 새로운 위치에 플레이어를 그린다.
            }
            // 위치는 같고 방향만 바뀌었다면
            else
            {
                DrawPlayer(player, newX, newY); // 플레이어만 다시 그려 방향 업데이트
            }
        }

        // 광산 내 하단 Guide UI
        private void Guide(Player player)
        {
            // UI 위치
            Console.SetCursorPosition(0, MapHeight + 2);

            // 인벤토리에서 필요한 정보들을 가져온다.
            int mineralCount = player.Inventory.GetMineralCount(PrimaryMineral);
            int drinkCount = player.Inventory.GetConsumableCount(Items.FatigueDrink);
            int bombCount = player.Inventory.GetConsumableCount(Items.Bomb);

            // 가져온 정보 출력
            Console.WriteLine($"피로도: {player.Fatigue} / {player.MaxFatigue} | [{PrimaryMineral.Name}]: {mineralCount}개");
            Console.WriteLine($"소모품: [드링크]: {drinkCount}개 / [폭탄]: {bombCount}개");
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("이동: 방향키 | 채굴: Q | 드링크: 1 | 폭탄: 2");
        }
    }
    // 광산 DB
    public static class Mines
    {
        public static readonly Mine IronMine = new Mine("철 광산", Items.StonePickaxe, Items.Iron);
        public static readonly Mine GoldMine = new Mine("금 광산", Items.IronPickaxe, Items.Gold);
        public static readonly Mine DiamondMine = new Mine("다이아몬드 광산", Items.GoldPickaxe, Items.Diamond);
    }
}