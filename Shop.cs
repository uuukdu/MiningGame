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
                Console.WriteLine("1. 광물 판매하기");
                Console.WriteLine("2. 곡괭이 구매하기");
                Console.WriteLine("3. 나가기");
                Console.WriteLine("==========================");
                Console.Write(">> ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Sell(player); // 판매 기능 호출
                        break;
                    case "2":
                        Buy(player); // 구매 기능 호출
                        break;
                    case "3":
                        return; // 메서드를 종료하고 메인 메뉴로 돌아감
                    default:
                        Console.WriteLine("\n>> 잘못된 입력입니다. 다시 선택해주세요.");
                        Console.ReadKey();
                        break;
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

                // 인벤토리에서 판매할 수 있는 광물 목록을 가져옴
                var mineralList = player.Inventory.GetMinerals().ToList();

                if (mineralList.Count == 0)
                {
                    Console.WriteLine("\n판매할 광물이 없습니다.");
                    Console.WriteLine("============================");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("\n어떤 광물을 판매하시겠습니까?");
                for (int i = 0; i < mineralList.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {mineralList[i].Key.Name} (보유: {mineralList[i].Value}개, 개당: {mineralList[i].Key.SellPrice} G)");
                }
                Console.WriteLine("============================");
                Console.Write("판매할 광물의 번호를 입력하세요 (취소: 0) >> ");

                if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= mineralList.Count)
                {
                    var selectedEntry = mineralList[choice - 1];
                    Mineral selectedMineral = selectedEntry.Key;
                    int ownedAmount = selectedEntry.Value;

                    Console.Write($"\n판매할 {selectedMineral.Name}의 수량을 입력하세요 (최대: {ownedAmount}) >> ");
                    if (int.TryParse(Console.ReadLine(), out int sellAmount) && sellAmount > 0 && sellAmount <= ownedAmount)
                    {
                        int totalPrice = selectedMineral.SellPrice * sellAmount;
                        player.Gold += totalPrice;
                        player.Inventory.RemoveItem(selectedMineral, sellAmount);

                        Console.WriteLine($"\n>> {selectedMineral.Name} {sellAmount}개를 판매하여 {totalPrice} G를 얻었습니다!");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("\n>> 잘못된 수량입니다.");
                        Console.ReadKey();
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
                        Console.ReadKey();
                    }
                    else if (player.Gold >= selectedPickaxe.Price)
                    {
                        player.Gold -= selectedPickaxe.Price;
                        player.Inventory.AddItem(selectedPickaxe);
                        player.EquippedPickaxe = selectedPickaxe; // 구매 후 바로 장착

                        Console.WriteLine($"\n>> {selectedPickaxe.Name}을(를) 구매했습니다! (자동으로 장착됩니다)");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("\n>> 골드가 부족합니다.");
                        Console.ReadKey();
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