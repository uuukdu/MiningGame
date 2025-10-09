using System;

namespace MiningGame
{
    class Menu
    {
        private bool _isRunning = true; // 게임 실행 여부
        private Player _player; // 게임의 플레이어 객체를 저장할 변수
        private Shop _shop; // 상점 객체를 저장할 변수

        // 게임의 전체 흐름 메서드
        public void Run()
        {
            Init();
            ShowMainMenu();
            EndGame();
        }

        // 게임 초기화
        private void Init()
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(60, 35);
            Console.WriteLine("Mining Game");
            Console.Write("플레이어의 이름을 입력해주세요: ");
            string playerName = Console.ReadLine();

            _player = new Player(playerName);
            _shop = new Shop();
        }

        // 메인 메뉴
        private void ShowMainMenu()
        {
            // 게임이 실행중이라면
            while (_isRunning)
            {
                Console.Clear();
                Console.WriteLine("====================");
                Console.WriteLine("1. 광산 이동");
                Console.WriteLine("2. 상점 이동");
                Console.WriteLine("3. 내 정보 보기");
                Console.WriteLine("\nESC키를 누르면 게임을 종료합니다.");
                Console.WriteLine("====================");

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.D1:
                        ShowMineMenu();
                        break;
                    case ConsoleKey.D2:
                        _shop.ShowMenu(_player);
                        break;
                    case ConsoleKey.D3:
                        _player.ShowStatus();
                        Console.WriteLine("\n아무 키나 누르면 메뉴로 돌아갑니다.");
                        Console.ReadKey(true);
                        break;
                    case ConsoleKey.Escape:
                        _isRunning = false;
                        break;
                }
            }
        }

        //광산 메뉴
        private void ShowMineMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==========================");
                Console.WriteLine("1. 돌 광산");
                Console.WriteLine("2. 철 광산");
                Console.WriteLine("3. 금 광산");
                Console.WriteLine("4. 다이아몬드 광산");
                Console.WriteLine("\n뒤로 가려면 ESC키를 눌러주세요.");
                Console.WriteLine("==========================");

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.D1:
                        EnterMine(Mines.StoneMine);
                        break;
                    case ConsoleKey.D2:
                        EnterMine(Mines.IronMine);
                        break;
                    case ConsoleKey.D3:
                        EnterMine(Mines.GoldMine);
                        break;
                    case ConsoleKey.D4:
                        EnterMine(Mines.DiamondMine);
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }

        // 광산 입장
        private void EnterMine(Mine mine)
        {
            Console.Clear();
            // 플레이어의 인벤토리에 필요한 곡괭이가 있다면
            if (_player.Inventory.HasPickaxe(mine.RequiredPickaxe))
            {
                // 해당 광산 입장
                mine.EnterMine(_player);
            }
            // 곡괭이가 없다면?
            else
            {
                Console.WriteLine($"\n>> '{mine.RequiredPickaxe.Name}'이(가) 필요합니다.");
                Console.ReadKey(true);
            }
        }

        // 게임 종료
        private void EndGame()
        {
            Console.Clear();
            Console.WriteLine("\n게임을 종료합니다.");
        }
    }
}