using System;
using System.Collections.Generic;
using System.Linq;

namespace MiningGame
{
    // Inventory 클래스
    public class Inventory
    {
        // Mineral(Key), 보유 개수 int(Value)
        private Dictionary<Mineral, int> _minerals;
        private List<Pickaxe> _pickaxes;
        private Dictionary<Consumable, int> _consumables;

        public Inventory()
        {
            _minerals = new Dictionary<Mineral, int>();
            _pickaxes = new List<Pickaxe>();
            _consumables = new Dictionary<Consumable, int>();
            // 게임 시작 시 기본 아이템인 돌 곡괭이를 추가
            _pickaxes.Add(Items.StonePickaxe);
        }

        // 아이템 추가
        public void AddItem(Item item)
        {
            // item이 Mineral 타입이라면, mineral 변수에 형변환하여 할당, 그리고 if문 실행
            if (item is Mineral mineral)
            {
                // 이미 해당 광물이 인벤토리에 있는지 확인
                if (_minerals.ContainsKey(mineral))
                {
                    _minerals[mineral]++; // 있다면 개수만 1 증가
                }
                else
                {
                    _minerals.Add(mineral, 1); // 없다면 새로 추가하고 개수는 1로 설정
                }
            }
            // item이 Pickaxe 타입인지 확인
            else if (item is Pickaxe pickaxe)
            {
                // 이미 해당 곡괭이를 가지고 있는지 확인
                if (!_pickaxes.Contains(pickaxe))
                {
                    _pickaxes.Add(pickaxe);
                }
            }
            // item이 Consumable 타입인지 확인
            else if (item is Consumable consumable)
            {
                if (_consumables.ContainsKey(consumable))
                {
                    _consumables[consumable]++;
                }
                else
                {
                    _consumables.Add(consumable, 1);
                }
            }
        }

        // 광물을 판매했을 때 광물 제거
        public void RemoveItem(Mineral mineral, int amount)
        {
            if (_minerals.ContainsKey(mineral))
            {
                _minerals[mineral] -= amount; // 요청된 수량만큼 개수를 뺀다
                if (_minerals[mineral] <= 0)
                {
                    _minerals.Remove(mineral); // 남은 개수가 0 이하면 해당 항목을 완전히 제거
                }
            }
        }

        // 아이템 제작시 광물 재료 제거
        public void RemoveMaterial(Mineral material, int amount)
        {
            if (_minerals.ContainsKey(material) && _minerals[material] >= amount)
            {
                _minerals[material] -= amount;
                if (_minerals[material] <= 0)
                {
                    _minerals.Remove(material);
                }
            }
        }

        // 소모품을 사용했을 때 인벤토리에서 제거
        public void UseConsumable(Consumable consumable)
        {
            if (_consumables.ContainsKey(consumable))
            {
                _consumables[consumable]--;
                if (_consumables[consumable] <= 0)
                {
                    _consumables.Remove(consumable);
                }
            }
        }

        // 인벤토리의 내용 출력
        public void ShowInventory()
        {
            Console.WriteLine("\n=========[보유 광물]=========");

            // 아이템 종류의 수를 확인
            if (_minerals.Count == 0)
            {
                Console.WriteLine("보유한 광물이 없습니다.");
            }
            else
            {
                foreach (var mineralPair in _minerals)
                {
                    Console.WriteLine($"- {mineralPair.Key.Name}: {mineralPair.Value}개");
                }
            }
            Console.WriteLine("============================");

            Console.WriteLine("\n=========[소모품]=========");
            if (_consumables.Count == 0)
            {
                Console.WriteLine("보유한 소모품이 없습니다.");
            }
            else
            {
                foreach (var consumablePair in _consumables)
                {
                    Console.WriteLine($"- {consumablePair.Key.Name}: {consumablePair.Value}개");
                }
            }
            Console.WriteLine("============================");
        }

        // 플레이어가 특정 곡괭이를 가지고 있는지 확인
        public bool HasPickaxe(Pickaxe requiredPickaxe)
        {
            return _pickaxes.Contains(requiredPickaxe);
        }

        // 특정 소모품을 가지고 있는지 확인
        public bool HasConsumable(Consumable consumable)
        {
            return _consumables.ContainsKey(consumable) && _consumables[consumable] > 0;
        }

        // 특정 소모품의 개수를 반환 (광산 Guide UI 표시에 사용)
        public int GetConsumableCount(Consumable consumable)
        {
            if (_consumables.ContainsKey(consumable))
            {
                return _consumables[consumable];
            }
            return 0; // 없다면 0을 반환
        }

        // 특정 광물 개수를 반환
        public int GetMineralCount(Mineral mineral)
        {
            if (_minerals.ContainsKey(mineral))
            {
                return _minerals[mineral];
            }
            return 0;
        }

        // 판매 목록을 보여주기 위해 광물 딕셔너리를 통째로 반환
        public Dictionary<Mineral, int> GetMinerals()
        {
            return _minerals;
        }
    }
}