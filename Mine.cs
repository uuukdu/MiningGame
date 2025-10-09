using System;
using System.Text;

namespace MiningGame
{
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

    public class Mine
    {
        public string Name { get; private set; }
        public Pickaxe RequiredPickaxe { get; private set; }
        public Mineral PrimaryMineral { get; private set; }

        // 맵 크기 30x30
        private const int MapWidth = 30;
        private const int MapHeight = 30;

        private BlockType[,] _map;

        internal Mine(string name, Pickaxe requiredPickaxe, Mineral primaryMineral)
        {
            Name = name;
            RequiredPickaxe = requiredPickaxe;
            PrimaryMineral = primaryMineral;
        }

        // 광산 플레이 메인 메서드
        public void EnterMine(Player player)
        {
            // 맵 생성
            GenerateMap();

            // 플레이어 시작 위치를 중앙으로 설정
            int playerX = MapWidth / 2;
            int playerY = MapHeight / 2;

            int prevPlayerX = playerX;
            int prevPlayerY = playerY;

            // 게임 시작 시 화면을 한번만 그림
            Console.Clear();
            Console.WriteLine($"[{Name}] (나가기: Esc)");
            DisplayMap(player, playerX, playerY, true); // true를 주어 전체 맵을 그리도록 함

            // 게임 시작 시 하단 UI
            Guide(player);

            bool isInMine = true;

            while (isInMine)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                // 4. 키 입력 처리
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow: // 위
                        player.PlayerDirection = Direction.Up;
                        prevPlayerX = playerX; prevPlayerY = playerY;
                        PlayerMove(player, ref playerX, ref playerY);
                        UpdatePlayerPosition(player, playerX, playerY, prevPlayerX, prevPlayerY);
                        break;
                    case ConsoleKey.DownArrow: // 아래
                        player.PlayerDirection = Direction.Down;
                        prevPlayerX = playerX; prevPlayerY = playerY;
                        PlayerMove(player, ref playerX, ref playerY);
                        UpdatePlayerPosition(player, playerX, playerY, prevPlayerX, prevPlayerY);
                        break;
                    case ConsoleKey.LeftArrow: // 왼쪽
                        player.PlayerDirection = Direction.Left;
                        prevPlayerX = playerX; prevPlayerY = playerY;
                        PlayerMove(player, ref playerX, ref playerY);
                        UpdatePlayerPosition(player, playerX, playerY, prevPlayerX, prevPlayerY);
                        break;
                    case ConsoleKey.RightArrow: // 오른쪽
                        player.PlayerDirection = Direction.Right;
                        prevPlayerX = playerX; prevPlayerY = playerY;
                        PlayerMove(player, ref playerX, ref playerY);
                        UpdatePlayerPosition(player, playerX, playerY, prevPlayerX, prevPlayerY);
                        break;
                    case ConsoleKey.Q: // Q
                        Mining(player, playerX, playerY);
                        break;
                    case ConsoleKey.Escape: // ESC
                        isInMine = false;
                        break;
                }
            }
        }

        // 플레이어 이동 메서드
        private void PlayerMove(Player player, ref int pX, ref int pY)
        {
            int targetX = pX;
            int targetY = pY;

            switch (player.PlayerDirection)
            {
                case Direction.Up: 
                    targetY--; 
                    break;
                case Direction.Down: 
                    targetY++; 
                    break;
                case Direction.Left: 
                    targetX--; 
                    break;
                case Direction.Right: 
                    targetX++; 
                    break;
            }

            // 빈 공간일 때만 이동
            if (_map[targetY, targetX] == BlockType.Empty)
            {
                pX = targetX;
                pY = targetY;
            }
        }

        // 보고있는 방향의 블럭을 채굴하는 메서드
        private void Mining(Player player, int pX, int pY)
        {
            int targetX = pX;
            int targetY = pY;

            switch (player.PlayerDirection)
            {
                case Direction.Up:
                    targetY--;
                    break;
                case Direction.Down:
                    targetY++;
                    break;
                case Direction.Left:
                    targetX--;
                    break;
                case Direction.Right:
                    targetX++;
                    break;
            }

            BlockType block = _map[targetY, targetX];

            if (block == BlockType.Dirt) // 흙
            {
                _map[targetY, targetX] = BlockType.Empty; // 빈 공간으로 변경
                DrawBlock(targetX, targetY); // 캐낸 블록 위치만 업데이트
                Guide(player); // 상태창 업데이트
            }
            else if (block != BlockType.Wall && block != BlockType.Empty) // 광물
            {
                _map[targetY, targetX] = BlockType.Empty; // 빈 공간으로 변경
                DrawBlock(targetX, targetY); // 캐낸 블록 위치만 업데이트
                player.Inventory.AddItem(PrimaryMineral); // 인벤토리에 광물 추가
                Guide(player); // 상태창 업데이트
            }
        }

        // 맵을 생성하는 메서드
        private void GenerateMap()
        {
            _map = new BlockType[MapHeight, MapWidth];
            Random rand = new Random();

            BlockType mineralBlockType = BlockType.Stone;
            if (PrimaryMineral == Items.Iron)
            {
                mineralBlockType = BlockType.Iron;
            }
            else if (PrimaryMineral == Items.Gold)
            {
                mineralBlockType = BlockType.Gold;
            }
            else if (PrimaryMineral == Items.Diamond)
            {
                mineralBlockType = BlockType.Diamond;
            }

            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    if (y == 0 || y == MapHeight - 1 || x == 0 || x == MapWidth - 1)
                    {
                        _map[y, x] = BlockType.Wall;
                    }
                    else
                    {
                        if (rand.Next(1, 101) <= 10)
                        {
                            _map[y, x] = mineralBlockType;
                        }
                        else
                        {
                            _map[y, x] = BlockType.Dirt;
                        }
                    }
                }
            }
            // 플레이어 시작 지점인 가운데를 빈 공간으로 설정
            _map[MapHeight / 2, MapWidth / 2] = BlockType.Empty;
        }

        // 전체 맵
        private void DisplayMap(Player player, int pX, int pY, bool fullDraw)
        {
            if (fullDraw) // 전체 그리기
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    for (int x = 0; x < MapWidth; x++)
                    {
                        DrawBlock(x, y);
                    }
                }
            }
            DrawPlayer(player, pX, pY); // 플레이어는 항상 그림
        }

        // 블록
        private void DrawBlock(int x, int y)
        {
            // 커서를 해당 위치로 이동 (가로는 2칸씩 차지하므로 x*2)
            Console.SetCursorPosition(x * 2, y + 1); // 상단 헤더 때문에 y + 1

            BlockType block = _map[y, x];
            char blockChar = ' ';

            switch (block)
            {
                case BlockType.Wall:
                    Console.ForegroundColor = ConsoleColor.Red;
                    blockChar = '■';
                    break;
                case BlockType.Dirt:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    blockChar = '▨';
                    break;
                case BlockType.Stone:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    blockChar = '●';
                    break;
                case BlockType.Iron:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    blockChar = '●';
                    break;
                case BlockType.Gold:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    blockChar = '●';
                    break;
                case BlockType.Diamond:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    blockChar = '●';
                    break;
                case BlockType.Empty:
                    Console.Write("  "); // 빈 공간은 2칸 공백
                    return;
            }
            Console.Write(blockChar); // 블록 문자 출력
            Console.ResetColor();
        }

        // 플레이어
        private void DrawPlayer(Player player, int pX, int pY)
        {
            Console.SetCursorPosition(pX * 2, pY + 1);
            Console.ForegroundColor = ConsoleColor.Green;
            char playerChar = ' ';
            switch (player.PlayerDirection)
            {
                case Direction.Up:
                    playerChar = '▲';
                    break;
                case Direction.Down:
                    playerChar = '▼';
                    break;
                case Direction.Left:
                    playerChar = '◀';
                    break;
                case Direction.Right:
                    playerChar = '▶';
                    break;
            }
            Console.Write(playerChar);
            Console.ResetColor();
        }

        // 플레이어 이동 시 이전 위치와 현재 위치만 업데이트
        private void UpdatePlayerPosition(Player player, int newX, int newY, int oldX, int oldY)
        {
            // 플레이어의 X, Y좌표 중 하나라도 달라졌다면?
            if (newX != oldX || newY != oldY)
            {
                DrawBlock(oldX, oldY); // 이전 위치를 맵의 원래 블록으로 덮어씀
                DrawPlayer(player, newX, newY); // 새 위치에 플레이어를 그림
            }
            else
            {
                DrawPlayer(player, newX, newY); // 방향만 바뀐 경우 플레이어만 다시 그림
            }
        }

        // 키 설명
        private void Guide(Player player)
        {
            // 커서를 맵 아래로 이동
            Console.SetCursorPosition(0, MapHeight + 1);
            Console.WriteLine("\n이동: 방향키 / 채굴: Q (바라보는 방향)");
        }
    }

    // 광산 DB
    public static class Mines
    {
        public static readonly Mine StoneMine = new Mine("돌 광산", Items.StonePickaxe, Items.Stone);
        public static readonly Mine IronMine = new Mine("철 광산", Items.IronPickaxe, Items.Iron);
        public static readonly Mine GoldMine = new Mine("금 광산", Items.GoldPickaxe, Items.Gold);
        public static readonly Mine DiamondMine = new Mine("다이아몬드 광산", Items.DiamondPickaxe, Items.Diamond);
    }
}