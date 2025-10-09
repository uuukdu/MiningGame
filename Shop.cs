using System;
using System.Collections.Generic;
using System.Linq;

namespace MiningGame
{
    public class Shop
    {
        // 상점 메인 메뉴
        public void ShowMenu(Player player)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==========[상점]==========");
                Console.WriteLine($"\n어서 오세요, {player.Name} 님!");
                Console.WriteLine($"보유 골드: {player.Gold} G");
                Console.WriteLine("\n무엇을 하시겠습니까?");
                Console.WriteLine("1. 아이템 판매하기");
                Console.WriteLine("2. 아이템 구매하기");
                Console.WriteLine("\n뒤로 가려면 ESC키를 눌러주세요.");
                Console.WriteLine("==========================");

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.D1:
                        Sell(player);
                        break;
                    case ConsoleKey.D2:
                        Buy(player);
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }

        // 아이템 판매
        public void Sell(Player player)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==========[광물 판매]==========");

                // 플레이어의 인벤토리에서 광물 목록을 가져와 리스트로 변환
                var mineralList = player.Inventory.GetMinerals().ToList();

                // 판매할 광물이 하나도 없다면
                if (mineralList.Count == 0)
                {
                    Console.WriteLine("\n판매할 광물이 없습니다.");
                    Console.WriteLine("\nESC키를 누르세요");
                    Console.WriteLine("============================");
                    Console.ReadKey(true);
                    return;
                }

                Console.WriteLine("\n어떤 광물을 판매하시겠습니까?");
                // 판매 가능한 광물 목록을 순서대로 출력
                for (int i = 0; i < mineralList.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {mineralList[i].Key.Name} (보유: {mineralList[i].Value}개, 개당: {mineralList[i].Key.SellPrice} G)");
                }
                Console.WriteLine("============================");
                Console.Write("판매할 광물의 번호를 입력하세요 (취소: 0) >> ");

                if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= mineralList.Count)
                {
                    // 사용자가 입력한 번호에 해당하는 광물 정보를 가져옴 (리스트는 0부터 시작하므로 -1)
                    var selectedEntry = mineralList[choice - 1];
                    Mineral selectedMineral = selectedEntry.Key;
                    int ownedAmount = selectedEntry.Value;

                    Console.Write($"\n판매할 {selectedMineral.Name}의 수량을 입력하세요 (최대: {ownedAmount}) >> ");

                    // 입력한 수량을 정수로 변환, 1개 이상이며 보유 수량 이하라면
                    if (int.TryParse(Console.ReadLine(), out int sellAmount) && sellAmount > 0 && sellAmount <= ownedAmount)
                    {
                        // 총 판매 가격 계산 (개당 가격 * 수량)
                        int totalPrice = selectedMineral.SellPrice * sellAmount;
                        // 플레이어의 골드를 증가시킴
                        player.Gold += totalPrice;
                        // 인벤토리에서 판매한 수량만큼 광물을 제거
                        player.Inventory.RemoveItem(selectedMineral, sellAmount);

                        Console.WriteLine($"\n>> {selectedMineral.Name} {sellAmount}개를 판매하여 {totalPrice} G를 얻었습니다!");
                        Console.ReadKey(true);
                    }
                    // 수량 입력이 잘못된 경우
                    else
                    {
                        Console.WriteLine("\n>> 잘못된 수량입니다.");
                        Console.ReadKey(true);
                    }
                }
                else
                {
                    return; // 0 또는 잘못된 번호를 입력하면 상점 메뉴로 돌아감
                }
            }
        }

        // 아이템 구매
        public void Buy(Player player)
        {
            // 상점에서 판매할 곡괭이 목록
            List<Pickaxe> saleList = new List<Pickaxe>
            {
                Items.IronPickaxe,
                Items.GoldPickaxe,
                Items.DiamondPickaxe
            };

            while (true)
            {
                Console.Clear();
                Console.WriteLine("==========[곡괭이 구매]==========");
                Console.WriteLine($"\n보유 골드: {player.Gold} G");
                Console.WriteLine("\n어떤 곡괭이를 구매하시겠습니까?");

                // 판매중인 곡괭이 목록을 순서대로 출력
                for (int i = 0; i < saleList.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {saleList[i].Name} (가격: {saleList[i].Price} G)");
                }
                Console.WriteLine("===============================");
                Console.Write("구매할 아이템의 번호를 입력하세요 (취소: 0) >> ");

                if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= saleList.Count)
                {
                    Pickaxe selectedPickaxe = saleList[choice - 1];

                    // 이미 가지고 있는지, 돈은 충분한지 확인
                    if (player.Inventory.HasPickaxe(selectedPickaxe))
                    {
                        Console.WriteLine("\n>> 이미 보유하고 있는 곡괭이입니다.");
                        Console.ReadKey(true);
                    }
                    else if (player.Gold >= selectedPickaxe.Price)
                    {
                        // 플레이어 골드에서 아이템 가격만큼 차감
                        player.Gold -= selectedPickaxe.Price;
                        // 플레이어 인벤토리에 구매한 곡괭이 추가
                        player.Inventory.AddItem(selectedPickaxe);
                        // 구매 후 바로 장착
                        player.EquippedPickaxe = selectedPickaxe;

                        Console.WriteLine($"\n>> {selectedPickaxe.Name}을(를) 구매했습니다! (자동으로 장착됩니다)");
                        Console.ReadKey(true);
                    }
                    else
                    {
                        Console.WriteLine("\n>> 골드가 부족합니다.");
                        Console.ReadKey(true);
                    }
                }
                else
                {
                    return; // 0 또는 잘못된 번호를 입력하면 상점 메뉴로 돌아감
                }
            }
        }
    }
}