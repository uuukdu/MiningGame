using System;

namespace MiningGame
{
    class Menu
    {
        private bool _isRunning = true; // 게임 실행 여부
        private Player _player; // 게임의 플레이어 객체를 저장할 변수
        private Shop _shop; // 상점 객체를 저장할 변수

        // 게임의 전체 흐름을 담당하는 메인 메서드
        public void Run()
        {
            Init();
            ShowMainMenu();
            EndGame();
        }

        // 게임 초기화
        private void Init()
        {
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
                Console.WriteLine("4. 게임 종료");
                Console.WriteLine("====================");
                Console.Write(">> ");
                string choice = Console.ReadLine(); // 사용자 입력 받기

                switch (choice)
                {
                    case "1": ShowMineMenu(); break; // 광산 메뉴 메서드 호출
                    case "2":
                        Console.Clear();
                        // 기능 구현해야함
                        Console.WriteLine("\n>> 상점"); 
                        Console.ReadKey();
                        break;
                    case "3":
                        _player.ShowStatus(); // Player 클래스의 ShowStatus() 메서드 호출
                        Console.WriteLine("\n아무 키나 누르면 메뉴로 돌아갑니다.");
                        Console.ReadKey();
                        break;
                    case "4": 
                        _isRunning = false; // while문을 탈출하기 위해 false로 변경
                        break;
                    default: // 다른 입력을 했을 경우
                        Console.WriteLine("\n>> 잘못된 입력입니다. 다시 선택해주세요.");
                        Console.ReadKey();
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
                Console.WriteLine("5. 뒤로가기");
                Console.WriteLine("==========================");
                Console.Write(">> ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": 
                        EnterMine(Mines.StoneMine); 
                        break;
                    case "2": 
                        EnterMine(Mines.IronMine); 
                        break;
                    case "3": 
                        EnterMine(Mines.GoldMine); 
                        break;
                    case "4": 
                        EnterMine(Mines.DiamondMine); 
                        break;
                    case "5": 
                        return;
                    default:
                        Console.WriteLine("\n>> 잘못된 입력입니다. 다시 선택해주세요.");
                        Console.ReadKey();
                        break;
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
                // 있다면 해당 광산의 EnterMine 메서드 호출 (광산 입장)
                mine.EnterMine(_player);
            }
            // 곡괭이가 없다면?
            else
            {
                Console.WriteLine($"\n>> '{mine.RequiredPickaxe.Name}'이(가) 필요합니다.");
                Console.ReadKey();
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