using System;

namespace MiningGame
{
    // 메뉴 클래스
    class Menu
    {
        private bool _isRunning = true; // 게임 실행 여부를 저장하는 변수
        private Player _player; // 게임의 플레이어 객체를 저장할 변수
        private Shop _shop; // 상점 객체를 저장할 변수

        // 게임의 전체 흐름을 순서대로 정의한 메서드입니다.
        public void Run()
        {
            Init(); // 1. 게임 초기화
            ShowMainMenu(); // 2. 메인 메뉴 루프 시작
            EndGame(); // 3. 메인 메뉴 루프가 끝나면 게임 종료 처리
        }

        // 게임 시작에 필요한 초기 설정 메서드
        private void Init()
        {
            Console.CursorVisible = false; // 콘솔 창에서 깜빡이는 커서를 숨김
            Console.SetWindowSize(60, 40);
            Console.Write("플레이어의 이름을 입력해주세요: ");
            string playerName = Console.ReadLine();

            // 입력받은 이름으로 Player 객체와 Shop 객체를 생성
            _player = new Player(playerName);
            _shop = new Shop();
        }

        // 메인 메뉴
        private void ShowMainMenu()
        {
            while (_isRunning)
            {
                Console.Clear();
                Console.WriteLine("==========[메인 메뉴]==========");
                Console.WriteLine("\n1. 광산 이동");
                Console.WriteLine("2. 상점 이동");
                Console.WriteLine("3. 내 정보 보기");
                Console.WriteLine("\nX키를 누르면 게임을 종료합니다.");
                Console.WriteLine("==============================");

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
                    case ConsoleKey.X:
                        _isRunning = false;
                        break;
                }
            }
        }

        // 광산 선택 메뉴
        private void ShowMineMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==========[광산 메뉴]==========");
                Console.WriteLine("\n1. 철 광산");
                Console.WriteLine("2. 금 광산");
                Console.WriteLine("3. 다이아몬드 광산");
                Console.WriteLine("\n뒤로 가려면 ESC키를 눌러주세요.");
                Console.WriteLine("==============================");

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.D1:
                        EnterMine(Mines.IronMine);
                        break;
                    case ConsoleKey.D2:
                        EnterMine(Mines.GoldMine);
                        break;
                    case ConsoleKey.D3:
                        EnterMine(Mines.DiamondMine);
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }

        // 광산 입장 메서드
        private void EnterMine(Mine mine)
        {
            Console.Clear();
            // 플레이어의 인벤토리에 해당 광산에서 요구하는 곡괭이가 있는지 확인
            if (_player.Inventory.HasPickaxe(mine.RequiredPickaxe))
            {
                // 곡괭이가 있다면, 해당 광산의 EnterMine 메서드를 호출
                mine.EnterMine(_player);
            }
            // 곡괭이가 없다면
            else
            {
                Console.WriteLine($"\n>> '{mine.RequiredPickaxe.Name}'이(가) 필요합니다.");
                Console.ReadKey(true);
            }
        }

        // 게임 종료 메서드
        private void EndGame()
        {
            Console.Clear();
            Console.WriteLine("\n게임을 종료합니다.");
        }
    }
}