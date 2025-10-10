using System;
using System.Collections.Generic;
using System.Linq;

namespace MiningGame
{
    // 상점 클래스
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
                Console.WriteLine("3. 아이템 제작하기");
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
                    case ConsoleKey.D3:
                        Craft(player);
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

                // 플레이어 인벤토리에서 판매할 광물 목록을 가져옴
                // Dictionary를 List로 변환하여  접근하기 쉽게 만듦
                var mineralList = player.Inventory.GetMinerals().ToList();

                // 판매할 광물이 하나도 없다면
                if (mineralList.Count == 0)
                {
                    Console.WriteLine("\n판매할 광물이 없습니다.");
                    Console.ReadKey(true);
                    return;
                }

                Console.WriteLine("\n어떤 광물을 판매하시겠습니까?");
                // for 반복문을 사용하여 판매 가능한 광물 목록을 순서대로 출력
                for (int i = 0; i < mineralList.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {mineralList[i].Key.Name} (보유: {mineralList[i].Value}개, 개당: {mineralList[i].Key.SellPrice} G)");
                }
                Console.WriteLine("============================");
                Console.Write("판매할 광물의 번호를 입력하세요 (취소: Esc) >> ");

                string input = "";
                while (true)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.Enter) { Console.WriteLine(); break; }
                    if (keyInfo.Key == ConsoleKey.Escape) return; // ESC 누르면 즉시 메서드 종료
                    if (keyInfo.Key == ConsoleKey.Backspace && input.Length > 0)
                    {
                        input = input.Substring(0, input.Length - 1);
                        Console.Write("\b \b");
                    }
                    else if (char.IsDigit(keyInfo.KeyChar))
                    {
                        input += keyInfo.KeyChar;
                        Console.Write(keyInfo.KeyChar);
                    }
                }

                if (int.TryParse(input, out int choice) && choice > 0 && choice <= mineralList.Count)
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
                        Console.ReadKey(true);
                    }
                    else
                    {
                        Console.WriteLine("\n>> 잘못된 수량입니다.");
                        Console.ReadKey(true);
                    }
                }
                else if (!string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("\n>> 잘못된 입력입니다.");
                    Console.ReadKey(true);
                }
            }
        }

        // 아이템 구매
        public void Buy(Player player)
        {
            // 판매할 소모품 목록을 만듭니다.
            List<Consumable> saleList = new List<Consumable>
            {
                Items.FatigueDrink,
                Items.Bomb
            };

            while (true)
            {
                Console.Clear();
                Console.WriteLine("==========[아이템 구매]==========");
                Console.WriteLine($"\n보유 골드: {player.Gold} G");
                Console.WriteLine("\n어떤 아이템을 구매하시겠습니까?");

                for (int i = 0; i < saleList.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {saleList[i].Name} (가격: {saleList[i].Price} G)");
                }
                Console.WriteLine("===============================");
                Console.Write("구매할 아이템의 번호를 입력하세요 (취소: Esc) >> ");

                string input = "";
                while (true)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.Enter) { Console.WriteLine(); break; }
                    if (keyInfo.Key == ConsoleKey.Escape) return;
                    if (keyInfo.Key == ConsoleKey.Backspace && input.Length > 0)
                    {
                        input = input.Substring(0, input.Length - 1);
                        Console.Write("\b \b");
                    }
                    else if (char.IsDigit(keyInfo.KeyChar))
                    {
                        input += keyInfo.KeyChar;
                        Console.Write(keyInfo.KeyChar);
                    }
                }

                if (int.TryParse(input, out int choice) && choice > 0 && choice <= saleList.Count)
                {
                    Consumable selectedItem = saleList[choice - 1];

                    // 조건문으로 골드가 충분한지 확인
                    if (player.Gold >= selectedItem.Price)
                    {
                        player.Gold -= selectedItem.Price;
                        player.Inventory.AddItem(selectedItem);

                        Console.WriteLine($"\n>> {selectedItem.Name}을(를) 구매했습니다!");
                        Console.ReadKey(true);
                    }
                    else
                    {
                        Console.WriteLine("\n>> 골드가 부족합니다.");
                        Console.ReadKey(true);
                    }
                }
                else if (!string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("\n>> 잘못된 입력입니다.");
                    Console.ReadKey(true);
                }
            }
        }

        // 아이템(곡괭이) 제작
        public void Craft(Player player)
        {
            var recipes = new Dictionary<Pickaxe, (Mineral material, int amount)>
            {
                { Items.IronPickaxe, (Items.Iron, 30) },
                { Items.GoldPickaxe, (Items.Gold, 30) },
                { Items.DiamondPickaxe, (Items.Diamond, 30) }
            };

            while (true)
            {
                Console.Clear();
                Console.WriteLine("==========[아이템 제작]==========");
                Console.WriteLine("\n제작할 아이템을 선택하세요.");

                int recipeIndex = 1;
                foreach (var recipe in recipes)
                {
                    Pickaxe targetPickaxe = recipe.Key;
                    Mineral material = recipe.Value.material;
                    int requiredAmount = recipe.Value.amount;
                    int currentAmount = player.Inventory.GetMineralCount(material);

                    // 조건에 따라 다른 문자열을 status 변수에 할당
                    string status = player.Inventory.HasPickaxe(targetPickaxe) ? "(보유중)" : $"({currentAmount}/{requiredAmount})";

                    Console.WriteLine($"{recipeIndex}. {targetPickaxe.Name} 제작하기 - 재료: {material.Name} {requiredAmount}개 {status}");
                    recipeIndex++;
                }

                Console.WriteLine("===============================");
                Console.Write("제작할 아이템의 번호를 입력하세요 (취소: Esc) >> ");

                string input = "";
                while (true)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.Enter) { Console.WriteLine(); break; }
                    if (keyInfo.Key == ConsoleKey.Escape) return;
                    if (keyInfo.Key == ConsoleKey.Backspace && input.Length > 0)
                    {
                        input = input.Substring(0, input.Length - 1);
                        Console.Write("\b \b");
                    }
                    else if (char.IsDigit(keyInfo.KeyChar))
                    {
                        input += keyInfo.KeyChar;
                        Console.Write(keyInfo.KeyChar);
                    }
                }

                if (int.TryParse(input, out int choice) && choice > 0 && choice <= recipes.Count)
                {
                    var selectedRecipe = recipes.ElementAt(choice - 1);
                    Pickaxe targetPickaxe = selectedRecipe.Key;
                    Mineral material = selectedRecipe.Value.material;
                    int requiredAmount = selectedRecipe.Value.amount;

                    if (player.Inventory.HasPickaxe(targetPickaxe))
                    {
                        Console.WriteLine("\n>> 이미 보유하고 있는 아이템입니다.");
                        Console.ReadKey(true);
                    }
                    else if (player.Inventory.GetMineralCount(material) >= requiredAmount)
                    {
                        player.Inventory.RemoveMaterial(material, requiredAmount); // 재료 제거
                        player.Inventory.AddItem(targetPickaxe); // 아이템 추가
                        Console.WriteLine($"\n>> {targetPickaxe.Name} 제작에 성공했습니다!");
                        Console.ReadKey(true);
                    }
                    else
                    {
                        Console.WriteLine("\n>> 재료가 부족합니다.");
                        Console.ReadKey(true);
                    }
                }
                else if (!string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("\n>> 잘못된 입력입니다.");
                    Console.ReadKey(true);
                }
            }
        }
    }
}